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
        
        ActualPeople.text = "People: " + player.Population;

    }
    public void Init(BuildingData _buildingData)
    {
        Data = _buildingData;
        UpdateGraphic();
    }

    private void Update()
    {
        UpdateGraphic();
    }

    public void UpdateGraphic() {
        ActualPeople.text = "People: " + player.Population;
        
    }


}
