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

    void OnUnitEvent(string _eventName) {
        foreach (string eventName in Data.TimedEvents) {
            if (eventName == _eventName) {

            }
        } 
    }


    public void UpdateGraphic() {
        TextActualPeople.text = "People: " + Data.Population;
        
    }


}
