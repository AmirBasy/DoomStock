using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour, IPathFinding {
    public Vector2 startPos;
    public Vector2 lastPos;
    CellDoomstock startCell;
    CellDoomstock lastCell;
    List<INode> path = new List<INode>();
   
    private void OnDrawGizmos() {
        if (startCell == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(startCell.WorldPosition, new Vector3(0.2f, 0.2f, 0.2f));
        Gizmos.color = Color.green;
        Gizmos.DrawCube(lastCell.WorldPosition, new Vector3(0.2f, 0.2f, 0.2f));
        Gizmos.color = Color.yellow;
        foreach (var item in path) {

            Gizmos.DrawCube(new Vector3(item.GetWorldPosition().x, item.GetWorldPosition().y +1, item.GetWorldPosition().z), new Vector3(0.2f, 0.2f, 0.2f));  
        }
    }
    private void OnEnable() {
        GameManager.OnGridCreated += GridCreated;
    }

    private void GridCreated() {
        transform.position = GameManager.I.gridController.GetCellWorldPosition((int)startPos.x, (int)startPos.y);
        startCell = GameManager.I.gridController.Cells[(int)startPos.x, (int)startPos.y];
        lastCell = GameManager.I.gridController.Cells[(int)lastPos.x, (int)lastPos.y];
        path = this.Find(startCell, lastCell);
        
    }

    private void OnDisable() {
        GameManager.OnGridCreated -= GridCreated;
    }
}
