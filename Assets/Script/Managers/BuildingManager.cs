using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
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
    public void IncreaseResources(BuildingView _buildingview)
    {
        if (_buildingview.Data.Population.Count == 0)
        {
            return;
        }
        if (_buildingview.Data.Population.Count > 0)
        {
            foreach (BaseResourceData resource in _buildingview.Data.BaseResources)
            {
                if (_buildingview.Data.BaseResources != null)
                {
                    for (int i = 0; i < GameManager.I.resourcesManager.resourcesPrefabs.Count; i++)
                    {
                        if (resource.ID== GameManager.I.resourcesManager.resourcesPrefabs[i].ID)
                        {
                            GameManager.I.GetResourceDataByID(resource.ID).Value += (int)(_buildingview.Data.Population.Count * 10)/4;
                            Debug.Log("Actual Resources = " + GameManager.I.resources[i].ID + " il valore e' di " + GameManager.I.resources[i].Value);

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

    public void RemoveLife(BuildingView _buildingView)
    {
        _buildingView.Data.BuildingLife -= _buildingView.Data.DecreaseBuildingLife;
        Debug.Log("Actual Life = " + _buildingView.Data.BuildingLife);
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
    public Vector3 GetBuildingContainingUnit(PopulationData unit)
    {
        foreach (BuildingView b in GetAllBuildingInScene())
        {
            if (b.Data.Population.Exists(u => u.UniqueID == unit.UniqueID))
                return b.transform.position;
        }
        return GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition;
    }
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

