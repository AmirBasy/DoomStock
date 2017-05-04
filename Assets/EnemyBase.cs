using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour, IPathFinding {


    public BuildingView _buildingTarget;
    public List<INode> PathNodes = new List<INode>();
    int _nodeindex;
    public Vector2 startpos = new Vector2(0,12);

    public void AttackBuilding(BuildingView _target, Vector2 _startPostion) {
        _buildingTarget = _target;
        //Vector2 _endPosition = _buildingTarget.Data.GetGridPosition();
        Vector2 _endPosition = new Vector2 (15,15);
        INode nodeStart = GameManager.I.gridController.Cells[(int)_startPostion.x,(int)_startPostion.y];
        INode nodeEnd = GameManager.I.gridController.Cells[(int)_endPosition.x, (int)_endPosition.y];
        PathNodes = this.Find(nodeStart, nodeEnd);
        _nodeindex = PathNodes.Count - 1;
        MoveToNextNode(PathNodes[_nodeindex]);
    }

    public void MoveToNextNode(INode _nextPosition) {
       
        transform.DOMove(_nextPosition.GetWorldPosition(), 1).OnComplete(() => {
            if (_nodeindex >0) {
                _nodeindex--;
                MoveToNextNode(PathNodes[_nodeindex]); 
            }
        });
    }

    private void Start() {
        AttackBuilding(null, startpos);
    }

}
