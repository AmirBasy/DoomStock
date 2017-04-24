using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingView : MonoBehaviour
{
    public Image BuildingLifeBar;
    public Image PopulationBar;
    private int populationBarCounter;

    public int PopulationBarCounter
    {
        get { return populationBarCounter; }
        set { populationBarCounter = value;
            //SetPopulationBar();
        }
    }



    public BuildingData Data;
    Renderer rend;
    public Material[] Materials;
    public Animation anim;

    private void Start()
    {
        //PopulationBarCounter = 0;
        InvokeRepeating("DecreasePopulationBar", 0, 1);
        anim = GetComponent<Animation>();
        rend = GetComponent<Renderer>();
        UpdateAspect();

    }
    public void Init(BuildingData _buildingData)
    {

        Data = _buildingData;
        TimeEventManager.OnEvent += OnUnitEvent;

    }


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
        if (Data.currentState == BuildingData.BuildingState.Built)
        {
            foreach (TimedEventData ev in Data.TimedEvents)
            {
                switch (ev.ID)
                {
                    case "FineMese":
                        Debug.Log("Mese");
                        break;
                    case "FoodProduction":
                        GameManager.I.buildingManager.IncreaseResources(this);
                        break;
                    case "WoodProduction":
                        GameManager.I.buildingManager.IncreaseResources(this);
                        break;
                    case "StoneProduction":
                        GameManager.I.buildingManager.IncreaseResources(this);
                        break;
                    case "FaithProduction":
                        GameManager.I.buildingManager.IncreaseResources(this);
                        break;
                    case "SpiritProduction":
                        GameManager.I.buildingManager.IncreaseResources(this);
                        break;
                    case "FineAnno":
                        break;
                    case "Degrado":
                        GameManager.I.buildingManager.RemoveLife(this);
                        if (Data.BuildingLife < 1)
                            destroyMe();
                        break;
                    default:
                        break;
                }

            }
        }
        Debug.LogFormat("Edificio {0} ha ricevuto l'evento {1}", Data.ID, _eventData.ID);
        #endregion
    }



    /// <summary>
    /// API che distrugge il building.
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
            default:
                break;
        }
    }

   
    public void RemoveDebris()
    {
        GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y].SetStatus(CellDoomstock.CellStatus.Empty);
        GameManager.I.GetResourceDataByID("Wood").Value += Data.WoodToBuild / 4;
        GameManager.I.GetResourceDataByID("Stone").Value += Data.StoneToBuild / 4;
        if (OnRemoveDebris != null)
            OnRemoveDebris(this);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnUnitEvent;

    }

    public void DecreasePopulationBar() {
        if (PopulationBarCounter < 1)
        {
            return;

        }
        else
        {
            PopulationBarCounter -= 1;
        }
    }

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

    #region Events
    public delegate void BuildingEvent(BuildingView _buildingView);

    public static BuildingEvent OnDestroy;
    public static BuildingEvent OnRemoveDebris;

    #endregion
}
