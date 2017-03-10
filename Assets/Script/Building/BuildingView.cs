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
        UpdateGraphic();
    }

    public void UpdateGraphic() {
        TextActualPeople.text = "People: " + Data.Population;
        
    }


}
