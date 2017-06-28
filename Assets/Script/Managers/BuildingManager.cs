using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    public BuildingData[] buildingsData;
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
        foreach (var item in GameManager.I.forestInScene)
        {
            newBuildingList.Add(item);
        }
        //foreach (var item in GameManager.I.MeravigliasInScene)
        //{
        //    newBuildingList.Add(item);
        //}
        if (GameManager.I.MeravigliasInScene != null)
        {
            newBuildingList.Add(GameManager.I.MeravigliasInScene);
        }
        return newBuildingList;
    }

    /// <summary>
    /// Restituisce la view del data passato in parametro.
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
            }
        }
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

    /// <summary>
    /// restituisce true se c'è posto nelle case.
    /// </summary>
    /// <returns></returns>
    public bool IsThereAnySpace()
    {
        foreach (var item in GetAllBuildingInScene())
        {
            if (item.Data.ID == "Casa" && item.Data.PopulationLimit > item.Data.Population.Count  && item.Data.CurrentState == BuildingState.Built)
                return true;
                
        }
        return false;
    }


    public BuildingView GetFirstOpening()
    {
        List<BuildingView> houses = new List<BuildingView>();
        foreach (var item in GetAllBuildingInScene())
        {
            if (item.Data.ID == "Casa")
                houses.Add(item);
        }

        foreach (var item in houses)
        {
            if (item.Data.Population.Count <= item.Data.PopulationLimit)
                return item;
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

