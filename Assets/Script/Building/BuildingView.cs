using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BuildingView : MonoBehaviour
{

    #region Proprietà

    /// <summary>
    /// Dato della view.
    /// </summary>
    public BuildingData Data;

    /// <summary>
    /// Materiali momentanei della view.
    /// </summary>
    Renderer rend;
    public Material[] Materials;

    /// <summary>
    /// animazione della view.
    /// </summary>
    public Animation anim; 

    #endregion

    #region LifeCycle
    private void Start()
    {
        Data.Init();
        //PopulationBarCounter = 0;
        anim = GetComponent<Animation>();
        rend = GetComponent<Renderer>();
        UpdateAspect();

    }


    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnUnitEvent;
        
    } 
    #endregion

    #region API

    /// <summary>
    /// Distrugge il building.
    /// </summary>
    public void destroyMe()
    {
        if (OnDestroy != null)
            OnDestroy(this);
        CellDoomstock cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
        cell.SetStatus(CellDoomstock.CellStatus.Debris, cell.building);
        //GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Debis);
        //toglie tutti i popolani dall'edificio e le rimette in POZZA
        Data.RemoveAllPopulationFromBuilding();
        TimeEventManager.OnEvent -= OnUnitEvent;
        Data.currentState = BuildingData.BuildingState.Debris;
        transform.DOPunchScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            UpdateAspect();
        });

    }

    /// <summary>
    /// aggiorna la grafica del building
    /// </summary>
    /// 
    public void UpdateAspect()
    {
        Animator _animator = GetComponent<Animator>();
        switch (Data.currentState)
        {
            case BuildingData.BuildingState.Construction:
                GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Construction);
                rend.material = Materials[1];
                transform.DOMoveY(transform.position.y + 1, Data.BuildingTime).OnComplete(() => { });
                break;
            case BuildingData.BuildingState.Built:
                GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Builded);
                rend.material = Materials[0];

                break;
            case BuildingData.BuildingState.Debris:
                GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Debris);
                rend.material = Materials[2];
                transform.DOMoveY(transform.position.y - 0.5f, 2).OnComplete(() => { });
                _animator.enabled = false;
                break;
            case BuildingData.BuildingState.Producing:
                Data.IsBuildingProducing();
                if (Data.IsBuildingProducing() == true)
                {
                    _animator.enabled = true;
                }
                else
                {
                    _animator.enabled = false;
                }
                break;
            case BuildingData.BuildingState.Ready:
                _animator.enabled = false;
                GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Ready);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Toglie le macerie, rende libera la cella e recupera 1/4 del materiale.
    /// </summary>
    public void RemoveDebris()
    {
        GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y].SetStatus(CellDoomstock.CellStatus.Empty);
        GameManager.I.GetResourceDataByID("Wood").Value += Data.WoodToBuild / 4;
        GameManager.I.GetResourceDataByID("Stone").Value += Data.StoneToBuild / 4;
        if (OnRemoveDebris != null)
            OnRemoveDebris(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// inizializzazione del buildingData.
    /// </summary>
    /// <param name="_buildingData"></param>
    public void Init(BuildingData _buildingData)
    {

        Data = _buildingData;
        TimeEventManager.OnEvent += OnUnitEvent;

    }

    #endregion

    #region Events

    public delegate void BuildingEvent(BuildingView _buildingView);
    public static BuildingEvent OnDestroy;
    public static BuildingEvent OnRemoveDebris;

    void OnUnitEvent(TimedEventData _eventData)
    {
        #region Event
        if (_eventData.ID == "Costruzione" && Data.currentState == BuildingData.BuildingState.Construction)
        {
            Data.BuildingTime--;
            if (Data.BuildingTime == 0)
            {
                Data.currentState = BuildingData.BuildingState.Built;
                UpdateAspect();
            }
        }
        if (Data.currentState == BuildingData.BuildingState.Producing)
        {
            switch (_eventData.ID)
            {
                case "FoodProduction":
                    foreach (var res in Data.BuildingResources)
                    {
                        if (res.ID == "Food")
                            res.Value += (int)(Data.Population.Count * 5);

                        if (res.Value >= res.Limit)
                        {
                            res.Value = 0;
                            Data.currentState = BuildingData.BuildingState.Ready;
                            UpdateAspect();
                            //if (OnLimitReached != null)
                            //    OnLimitReached();
                        }
                    }
                    if (Data.Population.Count <= 0)
                        UpdateAspect();
                    break;

                case "WoodProduction":
                    foreach (var res in Data.BuildingResources)
                    {
                        if (res.ID == "Wood")
                            GameManager.I.buildingManager.IncreaseResources(this, res);
                    }
                    break;
                case "StoneProduction":
                    foreach (var res in Data.BuildingResources)
                    {
                        if (res.ID == "Stone")
                            GameManager.I.buildingManager.IncreaseResources(this, res);
                    }
                    break;
                case "FaithProduction":
                    foreach (var res in Data.BuildingResources)
                    {
                        if (res.ID == "Faith")
                            GameManager.I.buildingManager.IncreaseResources(this, res);
                    }
                    break;
                case "SpiritProduction":
                    foreach (var res in Data.BuildingResources)
                    {
                        if (res.ID == "Spirit")
                            GameManager.I.buildingManager.IncreaseResources(this, res);
                    }
                    break;
                default:
                    break;
            }
        }
        Debug.LogFormat("Edificio {0} ha ricevuto l'evento {1}", Data.ID, _eventData.ID);
        #endregion
    }

    #endregion

    #region BARRA commentata
    public Image BuildingLifeBar;
    public Image PopulationBar;
    private int populationBarCounter;

    public int PopulationBarCounter
    {
        get { return populationBarCounter; }
        set
        {
            populationBarCounter = value;
            //SetPopulationBar();
        }
    }
    //public void DecreasePopulationBar()
    //{
    //    if (PopulationBarCounter < 1)
    //    {
    //        return;

    //    }
    //    else
    //    {
    //        PopulationBarCounter -= 1;
    //    }
    //}

    //public void SetPopulationBar() {
    //    if (Data.Population.Count < Data.PopulationLimit)
    //    {
    //        PopulationBarCounter ++;
    //    }
    //    else if (populationBarCounter <1)
    //    {
    //        return ;
    //    }
    //    else if (Data.Population.Count >= Data.PopulationLimit)
    //    {
    //        PopulationBarCounter--;
    //    }
    //    PopulationBar.fillAmount = (float)PopulationBarCounter/Data.PopulationLimit;
    //}
    #endregion
}
