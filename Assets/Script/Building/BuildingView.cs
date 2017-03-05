using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour {

    public TextMesh ActualPeople;

    public BuildingData Data;

    public Player player;
    private void Start()
    {
        ActualPeople.text = "People: " + player.population;

    }
    public void Init(BuildingData _buildingData)
    {
        Data = _buildingData;
        //UpdateGraphic(_buildingData);
    }

    public void UpdateGraphic() {
        ActualPeople.text = "Peopxxle: " + player.population;
    }


}
