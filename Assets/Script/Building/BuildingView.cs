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
        Animator _animator = new Animator();
        if (GetComponent<Animator>())
        {
            _animator = GetComponent<Animator>();
        }

        switch (Data.currentState)
        {
            case BuildingData.BuildingState.Construction:
                if (rend.material != null)
                {
                    rend.material = Materials[1];
                }
                transform.DOMoveY(transform.position.y + 1, Data.BuildingTime).OnComplete(() => { });
                break;
            case BuildingData.BuildingState.Built:
                if (rend.material != null)
                {
                    rend.material = Materials[0];
                }
                break;
            case BuildingData.BuildingState.Debris:
                if (rend.material != null)
                {
                    rend.material = Materials[2];
                }
                transform.DOMoveY(transform.position.y - 0.5f, 2).OnComplete(() => { });
                if (_animator)
                    _animator.enabled = false;
                break;
            case BuildingData.BuildingState.Producing:
                if (rend.material != null)
                {
                    rend.material = Materials[0];
                }
                Data.IsBuildingProducing();
                if (Data.IsBuildingProducing() == true)
                {
                    if (_animator)
                        _animator.enabled = true;
                }
                else
                {
                    if (_animator)
                        _animator.enabled = false;
                }
                break;
            case BuildingData.BuildingState.Ready:
                if (_animator)
                    _animator.enabled = false;
                rend.material = Materials[1];
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
        switch (_eventData.ID)
        {
            case "Production":
                if (Data.currentState == BuildingData.BuildingState.Producing)
                {
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += (int)(Data.Population.Count * 5);
                        LimitReached(res);
                        switch (res.ID) {
                            case "Faith":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FaithProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
                                break;
                            case "Wood":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.WoodProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
                                break;
                            case "Stone":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.StoneProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
                                break;
                            case "Spirit":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.SpiritProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
                                break;
                            case "Food":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FoodProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
                                break;
                            default:
                                break;
                        }
                    }
                }
                break;

            case "Costruzione":
                if (Data.currentState == BuildingData.BuildingState.Construction)
                {
                    Data.BuildingTime--;
                    if (Data.BuildingTime == 0)
                    {
                        Data.currentState = BuildingData.BuildingState.Built;
                        UpdateAspect();
                    }
                }

                break;
            default:
                break;
        }
        if (Data.Population.Count <= 0)
            UpdateAspect();

    }

    #endregion


    void LimitReached(BaseResourceData res)
    {
        if (res.Value >= res.Limit)
        {
            res.Value = 0;
            Data.currentState = BuildingData.BuildingState.Ready;
            //TODO : da inserire messaggio con icona fissa quando termina la produzione
            
            UpdateAspect();
        }
    }
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
