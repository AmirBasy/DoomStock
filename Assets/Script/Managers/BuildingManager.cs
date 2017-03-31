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
        return NewIstanceView;
    }

    /// <summary>
    /// Aumento della risorsa
    /// </summary>
    /// <param name="_buildingData"></param>
    public void IncreaseResources(BuildingView _buildingview)
    {
        if (_buildingview.Data.Population >0)
        {
            foreach (BaseResourceData resource in _buildingview.Data.BaseResources)
            {
                if (_buildingview.Data.BaseResources != null)
                {
                    for (int i = 0; i < GameManager.I.BaseResource.Length; i++)
                    {
                        if (resource.ID == GameManager.I.BaseResource[i])
                        {
                            switch (GameManager.I.BaseResource[i])
                            {
                                case "Food":
                                    GameManager.I.Food += _buildingview.Data.Population * resource.Value;
                                    Debug.Log("Actual Resources = " + GameManager.I.BaseResource[i] + " il valore e' di "+ GameManager.I.Food);
                                    break;
                                case "Wood":
                                    GameManager.I.Wood += _buildingview.Data.Population * resource.Value;
                                    Debug.Log("Actual Resources = " + GameManager.I.BaseResource[i] + "il valore e'" + GameManager.I.Wood);
                                    break;
                                case "Stone":
                                    GameManager.I.Stone += _buildingview.Data.Population * resource.Value;
                                    Debug.Log("Actual Resources = " + GameManager.I.BaseResource[i] + "il valore e'" + GameManager.I.Stone);
                                    break;
                                case "Faith":
                                    GameManager.I.Faith += _buildingview.Data.Population * resource.Value;
                                    Debug.Log("Actual Resources = " + GameManager.I.BaseResource[i] + "il valore e'" + GameManager.I.Faith);
                                    break;
                                case "Spirit":
                                    GameManager.I.Spirit += _buildingview.Data.Population * resource.Value;
                                    Debug.Log("Actual Resources = " + GameManager.I.BaseResource[i] + "il valore e'" + GameManager.I.Spirit);
                                    break;
                               
                                default:
                                    break;
                            }
                            
                            
                        }
                    } 
                }
            }
        }
    }

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

