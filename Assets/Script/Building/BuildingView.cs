using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildingView : MonoBehaviour {

    [HideInInspector] public Player player;

    public TextMesh TextActualPeople;

    public BuildingData Data;
    
    private void Start()
    {   
        Debug.Log("Actual Life " + this.Data.BuildingLife);
        //TextActualPeople.text = "People: " + player.Population;  
    }
    public void Init(BuildingData _buildingData)
    {
        CheckRenderer(GetComponent<Renderer>());
        Data = _buildingData;
        TimeEventManager.OnEvent += OnUnitEvent;
        UpdateGraphic();
    }

    void OnUnitEvent(TimedEventData _eventData) {
        foreach (TimedEventData ev in Data.TimedEvents) {
            if (ev.ID == _eventData.ID) {
                Debug.LogFormat("Edificio {0} ha ricevuto l'evento {1}", Data.ID, _eventData.ID);
            }
        }

        foreach (TimedEventData ev in Data.TimedEvents) {

            
                switch (ev.ID)
                {
                    case "FineMese":
                    if (Data.isBuilt == true)
                    {
                        GameManager.I.buildingManager.RemoveLife(this); 
                    }
                        break;
                    case "FoodProduction":
                    if (Data.isBuilt == true)
                    {
                        GameManager.I.buildingManager.IncreaseResources(this); 
                    }
                        break;
                    case "FineAnno":
                        break;
                    case "Degrado":
                    if (Data.isBuilt == true)
                    {
                        if (Data.BuildingLife < 1)
                            destroyMe(); 
                    }
                        break;
                    case "Costruzione":
                        if (ev.IsEnded == true)
                        {
                            Data.isBuilt = true;
                            CheckRenderer(GetComponent<Renderer>());
                        }
                        break;
                    default:
                        break;
                }
            
        }

        Debug.LogFormat("Edificio {0} si è decrementato di {1} ({2})", Data.ID, Data.DecreaseBuildingLife , Data.BuildingLife);
    }

  
    public void UpdateGraphic() {
        TextActualPeople.text = "People: " + Data.Population;
    }
    #region Resources API
    /// <summary>
    /// Controlla le risorse necessarie per costruire l'edificio
    /// </summary>
    public bool CheckResources()
    {
        if (Data.WoodToBuild <= GameManager.I.Wood &&
            Data.StoneToBuild <= GameManager.I.Stone)
            RemoveResource();
        return true;
    }
    /// <summary>
    /// Rimuove le risorse necessarie per costruire l'edificio
    /// </summary>
    public void RemoveResource()
    {
        GameManager.I.Wood -= Data.WoodToBuild;
        GameManager.I.Stone -= Data.StoneToBuild;
    } 
    #endregion

    /// <summary>
    /// Attiva o Disattiva la Mesh dell'Oggetto
    /// </summary>
    /// <param name="_renderer"></param>
    public bool CheckRenderer(Renderer _renderer)
    {
        _renderer = Data.BuildPrefab.GetComponent<Renderer>();
        if (Data.isBuilt == true)
        {
            _renderer.enabled = true;
            return true;
        }
        _renderer.enabled = false;
        return false;
    }

    /// <summary>
    /// Distrugge il guilding.
    /// </summary>
    void destroyMe() {
        if (OnDestroy != null)
            OnDestroy(this);
        transform.DOPunchScale(Vector3.one, 0.5f).OnComplete(() => {
            GameObject.Destroy(this.gameObject);
        });
    }

    private void OnDisable() {
        TimeEventManager.OnEvent -= OnUnitEvent;
    }

    #region Events
    public delegate void BuildingEvent(BuildingView _buildingView);

    public static BuildingEvent OnDestroy;
    #endregion

}
