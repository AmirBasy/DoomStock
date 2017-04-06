using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingView : MonoBehaviour
{

    //[HideInInspector] public Player player;

    public TextMesh TextActualPeople;

    public BuildingData Data;

    private void Start()
    {
        //Debug.Log("Actual Life " + this.Data.BuildingLife);
        //TextActualPeople.text = "People: " + player.Population;  
    }
    public void Init(BuildingData _buildingData)
    {
        //CheckRenderer(GetComponent<Renderer>());
        Data = _buildingData;
        TimeEventManager.OnEvent += OnUnitEvent;
        UpdateGraphic();
    }
 

    void OnUnitEvent(TimedEventData _eventData)
    {
        #region DebugEvent
        foreach (TimedEventData ev in Data.TimedEvents) {
            if (ev.ID == _eventData.ID) {
                Debug.LogFormat("Edificio {0} ha ricevuto l'evento {1}", Data.ID, _eventData.ID);
            }
        }
        #endregion


        switch (_eventData.ID)
            {
                case "FineMese":
                        
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
                case "HealthcareProduction":
                        GameManager.I.Healthcare++;
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


    public void UpdateGraphic()
    {
        TextActualPeople.text = "People: " + Data.Population;
    }

    //public void OnTimedEventDataChanged(TimedEventData _timedEventData) {

    //    if (_timedEventData.IsEnded == true)
    //    {
    //        Data.isBuilt = true;
    //        CheckRenderer(GetComponent<Renderer>());
    //    }
    //}
    

    /// <summary>
    /// Attiva o Disattiva la Mesh dell'Oggetto
    /// </summary>
    /// <param name="_renderer"></param>
    //public bool CheckRenderer(Renderer _renderer)
    //{
    //    _renderer = Data.BuildPrefab.GetComponent<Renderer>();
    //    if (Data.isBuilt == true)
    //    {
    //        _renderer.enabled = true;
    //        return true;
    //    }
    //    _renderer.enabled = false;
    //    return false;
    //}

    /// <summary>
    /// API che distrugge il building.
    /// </summary>
    public void destroyMe()
    {
        GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y].SetStatus(CellDoomstock.CellStatus.Empty);
        //toglie tutti i popolani dall'edificio e le rimette in POZZA
        Data.RemoveAllPopulationFromBuilding();
        transform.DOPunchScale(Vector3.one, 0.5f).OnComplete(() =>
        {
         if (OnDestroy != null)
            OnDestroy(this);
            Destroy(gameObject);
        });
        
    }


    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnUnitEvent;

    }

    #region Events
    public delegate void BuildingEvent(BuildingView _buildingView);
    public static BuildingEvent OnDestroy;
    
    #endregion
}
