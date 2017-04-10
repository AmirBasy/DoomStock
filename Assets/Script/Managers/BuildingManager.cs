using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {


    //private void Awake()
    //{
    //    TimeEventManager.OnEvent += OnUnitEvent;
    //}

    /// <summary>
    /// Lista di BuildingView che i player Istanziano nella scena.
    /// </summary>
    /// <returns></returns>
    public List<BuildingView> GetAllBuildingInScene() {
        List<BuildingView> newBuildingList = new List<BuildingView>();
        foreach (Player currentPlayer in GameManager.I.Players )
        {
            if (currentPlayer.BuildingsInScene != null)
            {
                foreach (BuildingView building in currentPlayer.BuildingsInScene)
                {
                    newBuildingList.Add(building);
                }
            } 
        }
        return newBuildingList;
    }


    /// <summary>
    /// Crea una BuildData restituisce una nuova istanza appena creata
    /// </summary>
    public BuildingView CreateBuild(BuildingData _buildingDataPrefab){
        BuildingData NewIstanceBuildingData;
        NewIstanceBuildingData = Instantiate(_buildingDataPrefab);
        BuildingView NewIstanceView = Instantiate(NewIstanceBuildingData.BuildPrefab);
        NewIstanceView.Init(NewIstanceBuildingData);
        NewIstanceView.Data.Population = new List<PopulationData>();
        return NewIstanceView;
    }

    /// <summary>
    /// Aumento della risorsa
    /// </summary>
    /// <param name="_buildingData"></param>
    public void IncreaseResources(BuildingView _buildingview)
    {
        //_buildingview.Data.Population = new List<PopulationData>();
        if (_buildingview.Data.Population.Count == 0)
        {
            return;
        }
        if  (_buildingview.Data.Population.Count > 0)
            {
                foreach (BaseResourceData resource in _buildingview.Data.BaseResources)
                {
                    if (_buildingview.Data.BaseResources != null)
                    {
                        for (int i = 0; i < GameManager.I.resources.Count; i++)
                        {
                            if (resource.ID == GameManager.I.resources[i].ID)
                            {
   
                                        GameManager.I.GetResourceDataByID(GameManager.I.resources[i].ID).Value += _buildingview.Data.Population.Count * resource.Value;
                                        Debug.Log("Actual Resources = " + GameManager.I.resources[i].ID + " il valore e' di " + GameManager.I.resources[i].Value);

                            }
                        }
                    }
                }


            }

        
    }

 

    //private void OnDisable()
    //{
    //    TimeEventManager.OnEvent -= OnUnitEvent;
    //}
    /// <summary>
    /// Controlla la lista di tutti gli edifici in scena.Se la vita e' 0, distrugge gli edifici
    /// </summary>
    /// <param name="_buildingView"></param>
    public void DestroyBuildingsInScene(BuildingView _buildingView) {
        foreach (BuildingView building in GetAllBuildingInScene())
        {
            if (_buildingView.Data.BuildingLife <= 0)
            {
                Destroy(_buildingView.gameObject);
                Debug.Log("Ho distrutto l edifico" + _buildingView.Data.ID);
            } 
        }
    }

    public void RemoveLife(BuildingView _buildingView){
        _buildingView.Data.BuildingLife -= _buildingView.Data.DecreaseBuildingLife;
        Debug.Log("Actual Life = " + _buildingView.Data.BuildingLife);
    }

    /// <summary>
    /// Restituisce il totale degli edifici in scena.
    /// </summary>
    /// <returns></returns>
    public int GetIdBuildingInScene() {
        int buildingNumber = GetAllBuildingInScene().Count;
        return buildingNumber;
    }

    /// <summary>
    /// Rimuove il building in scene con il parametro UNIQUE ID
    /// </summary>
    /// <param name="id"></param>
    public void DestroyBuildingInScene(string id) {
        foreach (BuildingView building in GetAllBuildingInScene())
        {
            if (building.Data.UniqueID == id)
            {
                Destroy(building);
            }
        }
    }

    #region Unique ID
    
    int counter = 0;
    /// <summary>
    /// Genera un id univoco.
    /// </summary>
    /// <returns></returns>
    public int GetUniqueId() {
        counter++;
        return counter;
    }
    #endregion
}

