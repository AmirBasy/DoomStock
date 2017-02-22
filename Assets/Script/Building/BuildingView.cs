using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour {

    public Text ActualPeople;

    public BuildingData Data;

    public void Init(BuildingData _buildingData)
    {
        Data = _buildingData;
        UpdateGraphic(_buildingData);
    }

    public void UpdateGraphic(BuildingData bd) {
        ActualPeople.text = bd.MyPeopleLimit.ToString();
    }
}
