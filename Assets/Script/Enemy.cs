using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Enemy : MonoBehaviour, IPathFindingMover
{
    #region Animation
    [Header("Animazioni")]
    #region Property
    Animator _animator;
    AnimationType _animationType = AnimationType.Walking;
    AnimationType animationType
    {
        get { return _animationType; }
        set
        {
            _animationType = value;
            _animator.SetInteger("AnimationState", (int)animationType);
        }
    }

    //public AnimationClip Camminata, Attacco, Morte;
    #endregion

    #endregion


    #region settings
    public enemyType Type;
    public int Life, _attack, RangeView;
    public float AttackSpeed;
    public float MovementSpeed = 0.8f;
    public BuildingData Priority1;
    public BuildingData Priority2;
    public List<BuildingData> Priorities2;
    #endregion

    public delegate void EnemyEvent(Enemy enemy);
    public static EnemyEvent OnStep;

    #region Runtime properties and variables
    enemyState currentState = enemyState.Searching;
    CellDoomstock lastPos;
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
            else if (item.Data.ID == Priority2.ID)
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
                    //Debug.Log(item.building.UniqueID);
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
        if (lastPos != null)
        {
            /*lastPos.Status != CellDoomstock.CellStatus.Filled && */

            if (lastPos.Type != CellDoomstock.CellType.Forest)
            {
                if (lastPos.Status == CellDoomstock.CellStatus.Enemy)
                {
                    lastPos.SetStatus(CellDoomstock.CellStatus.Empty);
                    Debug.Log(lastPos.Type);
                    lastPos.EnemiesInCell.Remove(this); 
                }
            }


            lastPos = CurrentPosition;
        }
        transform.DOLookAt(_step.GetWorldPosition(), MovementSpeed, AxisConstraint.Y);
        transform.DOMove((new Vector3(_step.GetWorldPosition().x, _step.GetWorldPosition().y - 0.8f, _step.GetWorldPosition().z + 0.4f)), MovementSpeed).OnComplete(() =>
        {
            CurrentNodeIndex++;
            int lastNode = pathFindingSettings.MoveToLastButOne ? CurrentPath.Count - 2 : CurrentPath.Count - 1;
            CurrentPosition = _step as CellDoomstock;
            lastPos = CurrentPosition;
            if (CurrentPosition.Type != CellDoomstock.CellType.Forest)
            {
                CurrentPosition.SetStatus(CellDoomstock.CellStatus.Enemy);
                CurrentPosition.EnemiesInCell.Add(this);
            }



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
            if (OnStep != null)
                OnStep(this);
            
        });
       
    }

    void StopAI()
    {
        _attack = 0;
        AttackSpeed = 0;
        MovementSpeed = 0;
    }

    public void OnDead()
    {
        if (Life <= 0)
        {

            if (lastPos.Type != CellDoomstock.CellType.Forest)
            {
                lastPos.EnemiesInCell.Remove(this);
                lastPos.SetStatus(CellDoomstock.CellStatus.Empty);
            }

            if (CurrentPosition.Type != CellDoomstock.CellType.Forest)
            {
                CurrentPosition.SetStatus(CellDoomstock.CellStatus.Empty);
                CurrentPosition.SetStatus(CellDoomstock.CellStatus.Empty);
            }

            AnimationDead();
            StopAI();
        }
    }

    public void AnimationDead()
    {
        animationType = AnimationType.Dead;
        StartCoroutine(CorourineAnimationDead(4));
    }

    IEnumerator CorourineAnimationDead(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
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
                if (nextStep.building)
                {
                    Attack(nextStep.building);
                }
                currentState = enemyState.Attack;
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
        if (target.Cell.GetWorldPosition().x != -1 && target.Cell.GetWorldPosition().y != -1)
        {
            transform.DOLookAt(target.Cell.GetWorldPosition(), MovementSpeed, AxisConstraint.Y);
        }
        currentState = enemyState.Attack;
        animationType = AnimationType.Attack;
        if (target)
        {
            target.BuildingLife -= _attack;
            target.GetParticlesEffect();
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
        //transform.position = new Vector3(_startPos.GetWorldPosition().x, _startPos.GetWorldPosition().y - 0.5f, _startPos.GetWorldPosition().z);
        _animator = GetComponent<Animator>();
        switch (Type)
        {
            case enemyType.Tank:
                pathFindingSettings = PathFindingSettings.Tank;
                break;
            case enemyType.Combattenti:
                pathFindingSettings = PathFindingSettings.Combattente;
                break;
        }
        transform.DOMove(new Vector3(_startPos.GetWorldPosition().x, _startPos.GetWorldPosition().y - 0.5f, _startPos.GetWorldPosition().z - 0.4f), MovementSpeed).OnComplete(() =>
            {
                CurrentPosition = _startPos;
            });
    }
    #endregion

    #region debug

    float waitTimeToFindTarget = 2;
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
        animationType = AnimationType.Walking;
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
       
        Gizmos.DrawCube(CurrentPosition.GetWorldPosition() + new Vector3(0f, 1f, 0f), gizmoDimension);
        Gizmos.color = Color.black;
    }

    public enum enemyState { Searching, MovingToTarget, Attack }

    #endregion

}

public enum enemyType { Tank, Combattenti }

public enum AnimationType { Walking = 0, Attack = 1, Dead = 2 }