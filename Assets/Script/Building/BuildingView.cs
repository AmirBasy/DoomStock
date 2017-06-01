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


    #endregion

    #region LifeCycle
    private void Start()
    {
        Data.Init();
        //PopulationBarCounter = 0;
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
        Data.CurrentState = BuildingState.Destroyed;
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => {
            //UpdateAspect();
        });

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

        Data.CurrentState = _status;
        OnStatusChanged();
    }
    /// <summary>
    /// Chiamata ogni volta che viene settato lo status di un edificio.
    /// </summary>
    public void OnStatusChanged()
    {
        switch (Data.CurrentState)
        {
            case BuildingState.Construction:
                transform.DOMoveY(transform.position.y + 1, Data.BuildingTime).OnComplete(() => {
                        SetBuildingStatus(BuildingState.Built);
                });
                break;
            case BuildingState.Built:
                //TODO : //GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Construction);
                break;
            case BuildingState.Producing:
                if (Data.ID != "Foresta")
                {
                    //if (rend.material != null)
                    //{
                    //    rend.material = Materials[0];
                    //}

                }
                else if (Data.ID == "Foresta")
                {

                   // transform.DOMoveY(transform.position.y + 0.1f, 1.5f).OnComplete(() => { });
                }
                //TODO : //GameManager.I.messagesManager.ShowBuildingMessage(this, BuildingMessageType.Construction);
                break;
            case BuildingState.Ready:
                CellDoomstock cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
                switch (Data.ID) {
                    case "Foresta":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitWood, cell);
                        break;
                    case "Cava":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitStone, cell);
                        break;
                    case "Chiesa":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitFaith, cell);
                        break;
                    case "Fattoria":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitFood, cell);
                        break;
                    case "Muro":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitSpirit, cell);
                        break;
                    case "Torretta":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitSpirit, cell);
                        break;
                    case "Casa":
                       // GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitPopulation, this.transform.position);
                        break;
                    default:
                        break;
                }
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
                CellDoomstock cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
                if (Data.CurrentState == BuildingState.Producing && Data.ID != "Foresta")
                {
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += (int)(Data.Population.Count * 1);
                        LimitReached(res);
                        switch (res.ID)
                        {
                            case "Faith":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FaithProduction, cell);
                                break;

                            case "Stone":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.StoneProduction, cell);
                                break;
                            case "Spirit":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.SpiritProduction, cell);
                                break;
                            case "Food":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FoodProduction, cell);
                                break;
                            default:
                                break;
                        }
                    }

                }
                else if (Data.CurrentState == BuildingState.Producing && Data.ID == "Foresta")
                {
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += 1;
                        LimitReached(res);
                        if (res.ID == "Wood")
                        {
                             GameManager.I.messagesManager.ShowiInformation(MessageLableType.WoodProduction, cell);
                        }
                    }
                }
                break;

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


    public void LimitReached(BaseResourceData res)
    {
        if (res.Value >= res.Limit)
        {
            res.Value = 0;
            SetBuildingStatus(BuildingState.Ready);
            
        }
    }
    #region BARRA commentata
    //public Image BuildingLifeBar;
    //public Image PopulationBar;
    //private int populationBarCounter;

    //public int PopulationBarCounter
    //{
    //    get { return populationBarCounter; }
    //    set
    //    {
    //        populationBarCounter = value;
    //        //SetPopulationBar();
    //    }
    //}
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
