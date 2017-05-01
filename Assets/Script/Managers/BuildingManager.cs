using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    #region Events

    public delegate void BuildingEvent();

    public static BuildingEvent OnLimitReached;

    #endregion

    #region API
    /// <summary>
    /// Lista di BuildingView che i player Istanziano nella scena.
    /// </summary>
    /// <returns></returns>
    public List<BuildingView> GetAllBuildingInScene()
    {
        List<BuildingView> newBuildingList = new List<BuildingView>();
        foreach (Player currentPlayer in GameManager.I.Players)
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
    public BuildingView CreateBuild(BuildingData _buildingDataPrefab)
    {
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
    public void IncreaseResources(BuildingView _buildingview, BaseResourceData _resource)
    {
      
        if (_buildingview.Data.Population.Count == 0)
        {
            return;
        }
        if (_buildingview.Data.Population.Count > 0)
        {

            if (_buildingview.Data.BuildingResources != null)
            {
               
            }

        }
    }

    /// <summary>
    /// Controlla la lista di tutti gli edifici in scena.Se la vita e' 0, distrugge gli edifici
    /// </summary>
    /// <param name="_buildingView"></param>
    public void DestroyBuildingsInScene(BuildingView _buildingView)
    {
        foreach (BuildingView building in GetAllBuildingInScene())
        {
            if (_buildingView.Data.BuildingLife <= 0)
            {
                Destroy(_buildingView.gameObject);
                Debug.Log("Ho distrutto l edifico" + _buildingView.Data.ID);
            }
        }
    }

    /// <summary>
    /// toglie vita all'edificio.
    /// </summary>
    /// <param name="_buildingView"></param>
    public void RemoveLife(BuildingView _buildingView)
    {
        _buildingView.Data.BuildingLife -= _buildingView.Data.DecreaseBuildingLife;
    }

    /// <summary>
    /// Restituisce il totale degli edifici in scena.
    /// </summary>
    /// <returns></returns>
    public int GetIdBuildingInScene()
    {
        int buildingNumber = GetAllBuildingInScene().Count;
        return buildingNumber;
    }

    /// <summary>
    /// Rimuove il building in scene con il parametro UNIQUE ID
    /// </summary>
    /// <param name="id"></param>
    public void DestroyBuildingInScene(string id)
    {

        if (GetBuildingView(id) != null)
            Destroy(GetBuildingView(id));
    }

    /// <summary>
    /// restituisce l'edificio a cui appartiene l'unità passatagli.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public Vector3 GetBuildingContainingUnit(PopulationData unit)
    {
        foreach (BuildingView b in GetAllBuildingInScene())
        {
            if (b.Data.Population.Exists(u => u.UniqueID == unit.UniqueID))
                return b.transform.position;
        }
        return GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition;
    }

    /// <summary>
    /// restituisce la view dell'edificio con l'ID passatogli.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildingView GetBuildingView(string id)
    {
        foreach (BuildingView building in GetAllBuildingInScene())
        {
            if (building.Data.UniqueID == id)
            {
                return building;
            }
        }
        return null;
    }

    #endregion

    #region Unique ID

    int counter = 0;

    /// <summary>
    /// Genera un id univoco.
    /// </summary>
    /// <returns></returns>
    public int GetUniqueId()
    {
        counter++;
        return counter;
    }

    #endregion
}

