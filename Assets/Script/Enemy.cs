using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Enemy : MonoBehaviour, IPathFindingMover {

    #region settings
    public enemyType Type;
    public int Life, Attck, RangeView;
    public float AttackSpeed;
    public float MovementSpeed = 0.8f;
    public BuildingData Priority1;
    public BuildingData Priority2;
    #endregion

    #region Runtime properties and variables
    private CellDoomstock _currentPosition;
    public CellDoomstock CurrentPosition {
        get { return _currentPosition; }
        set {
            _currentPosition = value;
        }
    }

    private BuildingData _currenTarget;
    public BuildingData CurrentTarget {
        get { return _currenTarget; }
        set {
            _currenTarget = value;
            if (_currenTarget == null) {
                //_currenTarget = FindTarget();
            } else {
                List<INode> pathNodes = this.Find(CurrentPosition, _currenTarget.Cell, true);
                pathNodes.Reverse();
                CurrentPath = pathNodes;
            }
        }
    }

    private List<INode> _currentPath = null;
    public List<INode> CurrentPath {
        get { return _currentPath; }
        set { _currentPath = value;
            if (_currentPath != null && _currentPath.Count > 0) {
                CurrentNodeIndex = 0;
                this.DoMoveToCurrentPathStep();
            }
        }
    }

    public int CurrentNodeIndex {
        get;

        set;
    }
    #endregion

    #region AI

    /// <summary>
    /// Identfica il buildingTarget secondo le regole prestabilite
    /// </summary>
    /// <returns></returns>
    public BuildingData FindTarget() {

        // reset old paths
        CurrentPath = null;

        List<BuildingData> targetTypeList = new List<BuildingData>();
        ////controlla nel suo raggio se è presente una priorità 1.
        //targetTypeList = FindPrioritiesInRange(Priority1);
        //if (targetTypeList.Count > 0) {
        //    return NearestBuildingPriority(targetTypeList);
        //}
        ////controlla nel suo raggio se è presente una priorità 2.
        //targetTypeList = FindPrioritiesInRange(Priority2);
        //if (targetTypeList.Count > 0) {
        //    return NearestBuildingPriority(targetTypeList);
        //}
        //controllare edifici nella mappa di Priority1
        foreach (var item in GameManager.I.buildingManager.GetAllBuildingInScene()) {
            if (item.Data.ID == Priority1.ID) {
                targetTypeList.Add(item.Data);
            }
            
        }
        if (targetTypeList.Count > 0) {
            return NearestBuildingPriority(targetTypeList.Where(b => b.CanBeAttacked() == true).ToList());
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
                    //CurrentPosition = GameManager.I.gridController.Cells[0, 0];
                    int distance = this.Find(CurrentPosition, _building.Cell, true).Count;
                    if (distance < lowerDistance) {
                        lowerDistance = distance;
                        closestBuilding = _building;
                    }
                }
                return closestBuilding;

            case enemyType.Combattenti:
                foreach (var _building in _buildings) {
                    int distance = this.Find(CurrentPosition, _building.Cell, true).Count;
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

    #endregion

    #region Abilities

    public void DoMoveStep(INode _step) {
        transform.DOMove(_step.GetWorldPosition(), MovementSpeed).OnComplete(() => {
            CurrentNodeIndex++;
            if (CurrentNodeIndex > CurrentPath.Count - 1) {
                // ha raggiunto l'obbiettivo, attacca
                Attack(CurrentTarget);
                CurrentTarget = FindTarget();
            } else {
                // prossimo step di movimento
                CurrentPosition = _step as CellDoomstock;
                this.DoMoveToCurrentPathStep();
            }

        });
    }

    public bool Attack(BuildingData target) {
        // TODO: al momento l'attacco ditrugge immediatamente l'edificio.
        GameManager.I.buildingManager.GetBuildingView(target.UniqueID).destroyMe();
        return true;
    }

    #endregion

    #region Lifecycle
    public void Init(CellDoomstock _startPos) {

        transform.DOMove(_startPos.GetWorldPosition(), MovementSpeed).OnComplete(() => {
            CurrentPosition = _startPos;
            //CurrentTarget = FindTarget();
        });


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
    #endregion

    #region debug

    float waitTimeToFindTarget = 0 ;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (CurrentPosition != null && CurrentTarget == null) {
                BuildingData findedBuilding = FindTarget();
                CurrentTarget = findedBuilding;
            }
        }

        waitTimeToFindTarget -= Time.deltaTime;
        if (waitTimeToFindTarget < 0) {
            if (CurrentTarget == null) {
                CurrentTarget = FindTarget();
            }
            waitTimeToFindTarget = 2.0f;
        }
        



    }

    private void OnDrawGizmos() {
        Vector3 gizmoDimension = new Vector3(0.5f, 0.5f, 0.5f);
        if (this.CurrentPath == null || this.CurrentPath.Count < 2)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(CurrentPath[0].GetWorldPosition(), gizmoDimension);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(CurrentTarget.Cell.GetWorldPosition(), gizmoDimension);
        Gizmos.color = Color.yellow;
        foreach (var item in CurrentPath) {
            Gizmos.DrawCube(new Vector3(item.GetWorldPosition().x, item.GetWorldPosition().y + 1, item.GetWorldPosition().z), gizmoDimension);
        }
    }
    #endregion
}

public enum enemyType { Tank, Combattenti }

