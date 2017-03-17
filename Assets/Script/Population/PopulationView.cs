using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationView : MonoBehaviour {


    public PopulationData Data;
     
    private void Start()
    {
        

    }
    public void Init(PopulationData _buildingData)
    {
        Data = _buildingData;

        UpdateGraphic();
    }

    public void UpdateGraphic()
    {
        
    }

}
