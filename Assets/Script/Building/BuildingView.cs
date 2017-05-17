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
        if (Data.ID != "Foresta")
        {
            SetBuildingStatus(BuildingState.Construction);
        }
        else
        {
            SetBuildingStatus(BuildingState.Producing);
        }

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
        // Data.currentState = BuildingData.BuildingState.Debris;
        //transform.DOPunchScale(Vector3.one, 0.5f).OnComplete(() =>
        //{
        //    UpdateAspect();
        //});

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


    public void SetBuildingStatus(BuildingState _status)
    {

        Data.currentState = _status;
        OnStatusChanged();
    }
    /// <summary>
    /// Chiamata ogni volta che viene settato lo status di un edificio.
    /// </summary>
    public void OnStatusChanged()
    {
        switch (Data.currentState)
        {
            case BuildingState.Construction:
                transform.DOMoveY(transform.position.y + 1, Data.BuildingTime).OnComplete(() => { SetBuildingStatus(BuildingState.Built); });
                break;
            case BuildingState.Built:
                //TODO : //GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Construction);
                break;
            case BuildingState.Producing:
                if (Data.ID != "Foresta")
                {
                    if (rend.material != null)
                    {
                        rend.material = Materials[0];
                    }

                }
                else if (Data.ID == "Foresta")
                {

                    transform.DOMoveY(transform.position.y + 0.05f, 1.5f).OnComplete(() => { });
                }
                //TODO : //GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Construction);
                break;
            case BuildingState.Ready:
               // rend.material = Materials[1];
                //TODO : //GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Construction);
                break;
            default:
                break;
        }
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
                if (Data.currentState == BuildingState.Producing && Data.ID != "Foresta")
                {
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += (int)(Data.Population.Count * 1);
                        LimitReached(res);
                        switch (res.ID)
                        {
                            case "Faith":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FaithProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
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
                else if (Data.currentState == BuildingState.Producing && Data.ID == "Foresta")
                {
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += 1;
                        LimitReached(res);
                        if (res.ID == "Wood")
                        {
                            // GameManager.I.messagesManager.ShowiInformation(MessageLableType.WoodProduction, GameManager.I.buildingManager.GetBuildingView(this.Data.UniqueID).transform.position);
                        }
                    }
                }
                break;

            //case "Costruzione":
            //    if (Data.currentState == BuildingData.BuildingState.Construction)
            //    {
            //        Data.BuildingTime--;
            //        if (Data.BuildingTime == 0)
            //        {
            //            Data.SetBuildingStatus(BuildingData.BuildingState.Built);
            //            if (Data.ID == "Foresta")
            //                Data.currentState = BuildingData.BuildingState.Producing;
            //        }
            //    }

             //   break;
            default:
                break;
        }
        //if (Data.Population.Count <= 0)
        //    if (Data.ID != "Foresta")
        //    {
        //        Data.SetBuildingStatus(BuildingData.BuildingState.Built);
        //    }

    }

    #endregion


    void LimitReached(BaseResourceData res)
    {
        if (res.Value >= res.Limit)
        {
            res.Value = 0;
            SetBuildingStatus(BuildingState.Ready);
            //TODO : da inserire messaggio con icona fissa quando termina la produzione
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
