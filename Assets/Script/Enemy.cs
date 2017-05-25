using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Enemy : MonoBehaviour, IPathFindingMover {

    public enemyType Type;
    public int Life, Attck, RangeView;
    public float AttackSpeed, MovementSpeed;
    public BuildingData Priority1;
    public BuildingData Priority2;

    public CellDoomstock CurrentPosition { get; set; }

    private BuildingData _currenTarget;
    public BuildingData CurrentTarget {
        get { return _currenTarget; }
        set {
            _currenTarget = value;
            if (_currenTarget == null) {
                _currenTarget = FindTarget();
            }

            if (_currenTarget) {
                CurrentPath = this.Find(CurrentPosition, _currenTarget.Cell);
                this.DoMove();
            }
        }
    }

    public List<INode> CurrentPath {
        get;

        set;
    }

    public int CurrentNodeIndex {
        get;

        set;
    }

    /// <summary>
    /// Identfica il buildingTarget secondo le regole prestabilite
    /// </summary>
    /// <returns></returns>
    public BuildingData FindTarget() {
        
        List<BuildingData> targetTypeList = new List<BuildingData>();
        //controlla nel suo raggio se è presente una priorità 1.
        targetTypeList = FindPrioritiesInRange(Priority1);
        if (targetTypeList.Count > 0) {
            return NearestBuildingPriority(targetTypeList);
        }
        //controlla nel suo raggio se è presente una priorità 2.
        targetTypeList = FindPrioritiesInRange(Priority2);
        if (targetTypeList.Count > 0) {
            return NearestBuildingPriority(targetTypeList);
        }
        //controllare edifici nella mappa di Priority1
        foreach (var item in GameManager.I.buildingManager.GetAllBuildingInScene()) {
            if (item.Data.ID == Priority1.ID) {
                targetTypeList.Add(item.Data);
            }
        }
        if (targetTypeList.Count > 0) {
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
        List<CellDoomstock> neightbours = GameManager.I.gridController.GetNeighboursStar(CurrentPosition, RangeView);
        foreach (CellDoomstock item in neightbours) {
            if (item.building != null)
                if (item.building.ID == _building.ID) {
                    returnList.Add(item.building);
                }
        }
        return returnList;
    }

    /// <summary>
    /// Passo la lista di building e ricevo il building piu vicino a me.
    /// </summary>
    public BuildingData NearestBuildingPriority(List<BuildingData> _buildings) {

        int lowerDistance = 100000000;
        BuildingData closestBuilding = null;
        switch (Type) {
            case enemyType.Tank:
                foreach (var _building in _buildings) {
                    int distance = this.Find(CurrentPosition, _building.Cell, true).Count;
                    if (distance < lowerDistance) {
                        lowerDistance = distance;
                        closestBuilding = _building;
                    }
                }
                return closestBuilding;

            case enemyType.Combattenti:
                foreach (var _building in _buildings) {
                    int distance = this.Find(CurrentPosition, _building.Cell).Count;
                    if (distance < lowerDistance) {
                        lowerDistance = distance;
                        closestBuilding = _building;
                    }
                }
                return closestBuilding;


            default:
                Debug.Log("Oh fuuuck");
                break;
        }

        //TODO : Se è Combattente
        return null;
    }

    public void DoMoveStep(INode _step) {
        transform.DOMove(_step.GetWorldPosition(), MovementSpeed).OnComplete(() => {
            CurrentPosition = _step as CellDoomstock;
            this.DoMove();
        });
    }

    public void Init(CellDoomstock _startPos) {
       
        transform.DOMove(_startPos.GetWorldPosition(), MovementSpeed).OnComplete(() => {
            CurrentPosition = _startPos;
            CurrentTarget = FindTarget();
        });
        

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){ 
            if (CurrentPosition != null && CurrentTarget == null) { 
                BuildingData findedBuilding = FindTarget();
                CurrentTarget = findedBuilding;
            }
        }
    }
    void OnEnable() {
        GameManager.OnGridCreated += GridCreated;
    }

    private void GridCreated() {
        Init(GameManager.I.gridController.Cells[3, 3]);
    }

    void OnDisable() {
        GameManager.OnGridCreated -= GridCreated;
    }
}

public enum enemyType { Tank, Combattenti }

