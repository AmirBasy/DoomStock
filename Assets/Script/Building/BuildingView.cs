using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour {

    public TextMesh TextActualPeople;

    public BuildingData Data;

    public Player player;
    private void Start()
    {
        Debug.Log("Actual Life " + this.Data.BuildingLife);
        //TextActualPeople.text = "People: " + player.Population;

    }
    public void Init(BuildingData _buildingData)
    {
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
            switch (ev.ID) {
                case "FineMese":
                    Data.BuildingLife = Data.BuildingLife - Data.DecreaseBuildingLife;
                    Debug.Log("Actual Life " + this.Data.BuildingLife);
                    break;
                case "FoodProduction":
                    GameManager.I.buildingManager.IncreaseResources(this);
                    break;
                case "FineAnno":
                    break;
                default:
                    break;
            }
        }

        Debug.LogFormat("Edificio {0} si è decrementato di {1} ({2})", Data.ID, Data.DecreaseBuildingLife , Data.BuildingLife);
    }

    private void FixedUpdate()
    {
        //GameManager.I.buildingManager.DestroyBuildingsInScene(this);
    }
    public void UpdateGraphic() {
        TextActualPeople.text = "People: " + Data.Population;
        
    }

    private void OnDisable() {
        TimeEventManager.OnEvent -= OnUnitEvent;
    }
    
}
