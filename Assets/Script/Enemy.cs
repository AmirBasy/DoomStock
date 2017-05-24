using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enemyType Type;
    public int Life, Attck, RangeView;
    public float AttackSpeed, MovementSpeed;
    public BuildingData Priority1;
    public BuildingData Priority2;

    public CellDoomstock CurrentPosition { get; set; }

    private BuildingData _currenPriority;
    public BuildingData CurrentPriority
    {
        get { return _currenPriority; }
        set { _currenPriority = value;
            if (_currenPriority==null)
            {
                _currenPriority = FindTarget();
            }
        }
    }
    /// <summary>
    /// Identfica il buildingTarget secondo le regole prestabilite
    /// </summary>
    /// <returns></returns>
    public BuildingData FindTarget() {
        List<BuildingData> targetTypeList = new List<BuildingData>();  
        //controlla nel suo raggio se è presente una priorità 1.
        targetTypeList = FindPrioritiesInRange(Priority1);
        if (targetTypeList.Count > 0)
        {
            return NearestBuildingPriority(targetTypeList);
        }
        //controlla nel suo raggio se è presente una priorità 2.
        targetTypeList = FindPrioritiesInRange(Priority2);
        if (targetTypeList.Count > 0)
        {
            return NearestBuildingPriority(targetTypeList);
        }
        //controllare edifici nella mappa di Priority1
        foreach (var item in GameManager.I.buildingManager.GetAllBuildingInScene())
        {
            if (item.Data.ID == Priority1.ID)
            {
                targetTypeList.Add(item.Data);
            }
        }
        if (targetTypeList.Count > 0)
        {
            return NearestBuildingPriority(targetTypeList);
        }
        return null;
    }

    /// <summary>
    /// Restituisce tutti i Buildingdata del tipo passato come parametro presenti nel RangeView
    /// Se non trova Building la lista è vuota.
    /// </summary>
    /// <param name="_building"></param>
    /// <returns></returns>
    public List<BuildingData> FindPrioritiesInRange(BuildingData _building) {
        List<BuildingData> returnList = new List<BuildingData>();
        foreach (CellDoomstock item in GameManager.I.gridController.GetNeighboursStar(CurrentPosition, RangeView))
        {
            if (item.building != null)
            {
                returnList.Add(item.building); 
            }
        }
        return returnList;
    }

    /// <summary>
    /// Passo la lista di building e ricevo il building piu vicino a me.
    /// </summary>
    public BuildingData NearestBuildingPriority(List<BuildingData> _building) {
        //TODO : Se è tank calcolare la distanza euristica INODE
        //TODO : Se è Combattente
        return null;
    }

}

public enum enemyType {Tank, Combattenti }

