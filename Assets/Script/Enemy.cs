using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Enemy : MonoBehaviour, IPathFindingMover
{

    #region settings
    public enemyType Type;
    public int Life, _attack, RangeView;
    public float AttackSpeed;
    public float MovementSpeed = 0.8f;
    public BuildingData Priority1;
    public BuildingData Priority2;
    public List<BuildingData> Priorities2;
    #endregion

    #region Runtime properties and variables
    enemyState currentState = enemyState.Searching;

    private CellDoomstock _currentPosition;
    public CellDoomstock CurrentPosition
    {
        get { return _currentPosition; }
        set
        {
            _currentPosition = value;
        }
    }

    private BuildingData _currenTarget;
    public BuildingData CurrentTarget
    {
        get { return _currenTarget; }
        set
        {
            _currenTarget = value;
            if (_currenTarget != null)
            {
                List<INode> pathNodes = this.FindPath(CurrentPosition, _currenTarget.Cell, pathFindingSettings);
                CurrentPath = pathNodes;
                currentState = enemyState.MovingToTarget;
            }
        }
    }

    private List<INode> _currentPath = null;
    public List<INode> CurrentPath
    {
        get { return _currentPath; }
        set
        {
            _currentPath = value;
            if (_currentPath != null && _currentPath.Count > 0)
            {
                CurrentNodeIndex = 0;
                AttackNextStep();
                this.DoMoveToCurrentPathStep();
            }
        }
    }

    public int CurrentNodeIndex
    {
        get;

        set;
    }

    public GridControllerDoomstock grid { get { return GameManager.I.gridController; } }

    PathFindingSettings pathFindingSettings;
    #endregion

    #region IPathFindingMover inteface
    /// <summary>
    /// Restituisce i neighbous del nodo.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<INode> GetNeighboursStar(INode node)
    {
        List<CellDoomstock> neighbours = grid.GetNeighboursStar(node as CellDoomstock);
        List<INode> returnInterfaceList = new List<INode>();
        foreach (var n in neighbours)
        {
            returnInterfaceList.Add(n);
        }
        return returnInterfaceList;
    }
    #endregion

    #region AI

    /// <summary>
    /// Identfica il buildingTarget secondo le regole prestabilite
    /// </summary>
    /// <returns></returns>
    public BuildingData FindTarget()
    {

        // reset old paths
        CurrentPath = null;

        List<BuildingData> targetTypeList = new List<BuildingData>();
        ////controlla nel suo raggio se è presente una priorità 1.
        targetTypeList = FindPrioritiesInRange(Priority1);
        if (targetTypeList.Count > 0)
        {
            return NearestBuildingPriority(targetTypeList);
        }
        ////controlla nel suo raggio se è presente una priorità 2.
       
        int randomindex = UnityEngine.Random.Range(0, Priorities2.Count);
        Priority2 = Priorities2[randomindex];
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
                if (item.Data.CanBeAttacked())
                    targetTypeList.Add(item.Data);
            }
            else if(item.Data.ID == Priority2.ID)
            {
                if (item.Data.CanBeAttacked())
                    targetTypeList.Add(item.Data);
            }

        }
        if (targetTypeList.Count > 0)
        {
            return NearestBuildingPriority(targetTypeList.Where(b => b.CanBeAttacked() == true).ToList());
        }

        // Altrimenti ritorna nullo e attende
        return null;
    }

    /// <summary>
    /// Restituisce tutti i Buildingdata del tipo passato come parametro presenti nel RangeView
    /// Se non trova Building la lista è vuota.
    /// </summary>
    /// <param name="_building"></param>
    /// <returns></returns>
    public List<BuildingData> FindPrioritiesInRange(BuildingData _building)
    {
        List<BuildingData> returnList = new List<BuildingData>();
        List<CellDoomstock> neightbours = GameManager.I.gridController.GetNeighboursStar(CurrentPosition, RangeView);
        foreach (CellDoomstock item in neightbours)
        {
            if (item.building != null)
                if (item.building.ID == _building.ID)
                {
                    if (item.building.CanBeAttacked())
                        returnList.Add(item.building);
                }
        }
        return returnList;
    }

    /// <summary>
    /// Passo la lista di building e ricevo il building piu vicino a me.
    /// </summary>
    public BuildingData NearestBuildingPriority(List<BuildingData> _buildings)
    {

        int lowerDistance = 100000000;
        BuildingData closestBuilding = null;
        switch (Type)
        {
            case enemyType.Tank:
                foreach (var _building in _buildings)
                {
                    //CurrentPosition = GameManager.I.gridController.Cells[0, 0];
                    int distance = IPathFindingExtension.GetDistance(CurrentPosition, _building.Cell, pathFindingSettings);
                    if (distance < lowerDistance)
                    {
                        lowerDistance = distance;
                        closestBuilding = _building;
                    }
                }
                return closestBuilding;

            case enemyType.Combattenti:
                foreach (var _building in _buildings)
                {
                    int distance = IPathFindingExtension.GetDistance(CurrentPosition, _building.Cell, pathFindingSettings);
                    if (distance < lowerDistance)
                    {
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

    public void DoMoveStep(INode _step)
    {
        transform.DOMove(_step.GetWorldPosition(), MovementSpeed).OnComplete(() =>
        {
            CurrentNodeIndex++;
            int lastNode = pathFindingSettings.MoveToLastButOne ? CurrentPath.Count - 2 : CurrentPath.Count - 1;
            CurrentPosition = _step as CellDoomstock;
            if (CurrentNodeIndex > lastNode)
            {
                // ha raggiunto l'obbiettivo, attacca
                if (Attack(CurrentTarget))
                {
                    if (CurrentTarget.BuildingLife <= 0)
                    {
                        currentState = enemyState.Searching;
                        resetTarget();
                    }
                    else
                    {
                        Attack(CurrentTarget);
                    }
                }
                else
                    currentState = enemyState.Attack;


            }
            else
            {
                AttackNextStep();
                if (currentState == enemyState.MovingToTarget)
                    // prossimo step di movimento
                    this.DoMoveToCurrentPathStep();
            }

        });
    }

    /// <summary>
    /// Attacca il prossimo step se è presente un building.
    /// </summary>
    void AttackNextStep()
    {
        CellDoomstock nextStep = CurrentPath[CurrentNodeIndex] as CellDoomstock;
        if (Type == enemyType.Tank)
        {

            if (nextStep.isTraversable == false)
            {
                Attack(nextStep.building);
            }
        }
        else
        {
            if (nextStep.isTraversable == false)
            {
                resetTarget();
                currentState = enemyState.Searching;
            }
        }
    }
    public bool Attack(BuildingData target)
    {
        currentState = enemyState.Attack;
        // TODO: al momento l'attacco ditrugge immediatamente l'edificio.
        //GameManager.I.buildingManager.GetBuildingView(target.UniqueID).destroyMe();

        if (target)
        {
            target.BuildingLife -= _attack; 
        }

        if (target.BuildingLife <= 0)
        {
            GameManager.I.buildingManager.GetBuildingView(target.UniqueID).destroyMe();
            currentState = enemyState.Searching;
            return false;
        }

        return true;
    }

    #endregion

    #region Lifecycle
    public void Init(CellDoomstock _startPos)
    {

        switch (Type)
        {
            case enemyType.Tank:
                pathFindingSettings = PathFindingSettings.Tank;
                break;
            case enemyType.Combattenti:
                pathFindingSettings = PathFindingSettings.Combattente;
                break;
        }

        transform.DOMove(_startPos.GetWorldPosition(), MovementSpeed).OnComplete(() =>
        {
            CurrentPosition = _startPos;
        });


    }

    //void OnEnable() {
    //    GameManager.OnGridCreated += GridCreated;
    //}

    //private void GridCreated() {
    //    Init(GameManager.I.gridController.Cells[3, 3]);
    //}

    //void OnDisable() {
    //    GameManager.OnGridCreated -= GridCreated;
    //}
    #endregion

    #region debug

    float waitTimeToFindTarget = 0;
    float waitTimeToAttackTarget = 0;

    void Update()
    {
        

        switch (currentState)
        {
            case enemyState.Searching:
                waitTimeToFindTarget -= Time.deltaTime;
                if (waitTimeToFindTarget < 0)
                {
                    if (CurrentTarget == null)
                    {
                        CurrentTarget = FindTarget();
                    }
                    waitTimeToFindTarget = 0.5f;
                }
                break;
            case enemyState.Attack:
                waitTimeToAttackTarget -= Time.deltaTime;
                if (waitTimeToAttackTarget < 0)
                {
                   
                        if (!Attack(CurrentTarget))
                        {
                            currentState = enemyState.Searching;
                            resetTarget();
                        }
                    
                    waitTimeToAttackTarget = AttackSpeed;
                }
                break;
        }





    }

    void resetTarget()
    {
        CurrentPath = null;
        CurrentTarget = null;
        CurrentNodeIndex = 0;
    }

    private void OnDrawGizmos()
    {
        Vector3 gizmoDimension = new Vector3(0.5f, 0.5f, 0.5f);
        if (this.CurrentPath == null || this.CurrentPath.Count < 2)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(CurrentPath[0].GetWorldPosition(), gizmoDimension);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(CurrentTarget.Cell.GetWorldPosition(), gizmoDimension);
        Gizmos.color = Color.yellow;
        foreach (var item in CurrentPath)
        {
            Gizmos.DrawCube(item.GetWorldPosition() + new Vector3(0f, 0.5f, 0f), gizmoDimension);
        }
        Gizmos.color = Color.black;
        Gizmos.DrawCube(CurrentPosition.GetWorldPosition() + new Vector3(0f, 1f, 0f), gizmoDimension);

    }

    public enum enemyState { Searching, MovingToTarget, Attack }

    #endregion
}

public enum enemyType { Tank, Combattenti }

