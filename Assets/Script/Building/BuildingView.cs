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
                //Debug.LogFormat("Edificio {0} ha ricevuto l'evento {1}", Data.ID, _eventData.ID);
            }
        }

        foreach (TimedEventData ev in Data.TimedEvents) {
            switch (ev.ID) {
                case "Mese":
                    Data.BuildingLife = Data.BuildingLife - Data.DecreaseBuildingLife;
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

    private void OnDisable() {
        TimeEventManager.OnEvent -= OnUnitEvent;
    }
}
