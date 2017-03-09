using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {


    /// <summary>
    /// Lista di BuildingView che i player Istanziano nella scena.
    /// </summary>
    /// <returns></returns>
    public List<BuildingView> GetAllBuildingInScene() {
        List<BuildingView> newBuildingList = new List<BuildingView>();
        if (GameManager.I.player.BuildingsInScene !=null)
        {
            foreach (BuildingView building in GameManager.I.player.BuildingsInScene)
            {
                newBuildingList.Add(building);
            } 
        }
        return newBuildingList;
    }

 
    private void Start() {
       
    }

    private void Update()
    {
    }

    /// <summary>
    /// Crea una BuildData restituisce una nuova istanza appena creata
    /// </summary>
    public BuildingView CreateBuild(BuildingData _buildingDataPrefab){
        BuildingData NewIstanceBuildingData;
        NewIstanceBuildingData = Instantiate(_buildingDataPrefab);
        BuildingView NewIstanceView = Instantiate(NewIstanceBuildingData.BuildPrefab);
        NewIstanceView.Init(NewIstanceBuildingData);
        return NewIstanceView;
    }

    
}

