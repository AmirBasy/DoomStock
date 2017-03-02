using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridController : MonoBehaviour {

    public Transform TilePrefab;
    public Vector2 GridSize = new Vector2(3,3);
    public List<Cell> Cells = new List<Cell>();
    public Cell cell = new Cell();


    // Use this for initialization
    void Awake() {
      
        GenerateMap();
        MoveToGridPosition(actualGridPosition);
    }
    /// <summary>
    /// Generates la griglia utilizzando il TilePrefab
    /// </summary>
    public void GenerateMap() {

        for (int x = 0; x < GridSize.x; x++) {
            for (int y = 0; y < GridSize.y; y++) {
                Vector3 tilePosition = new Vector3(-GridSize.x + 1 + x, 0, -GridSize.y + 1 + y);
                Transform newTile = Instantiate(TilePrefab, tilePosition, Quaternion.Euler(Vector3.right)) as Transform;
                newTile.parent = this.transform;
                newTile.name = string.Format("({0},{1}) - ({2})",x,y, newTile.transform.position);
                Cells.Add(new Cell(){GridPosition = new Vector2 (x,y), WorldPosition = newTile.transform.position});
            }

        }
    }

    /// <summary>
    /// Restituisce la posizione world della cella alla grid position richiesta
    /// </summary>
    /// <param name="_gridPosition">grid position richiesta</param>
    /// <returns></returns>
    public Vector3 GetWorldCellPosition(Vector2 _gridPosition) {
        foreach (Cell cell in Cells) {
            if (cell.GridPosition == _gridPosition)
                return cell.WorldPosition;
        }
        return Vector3.zero;
    }

    public void MoveToGridPosition(Vector2 _gridPotision) {
        DebugElement.transform.DOMove(GetWorldCellPosition(actualGridPosition), 0.7f).OnComplete(delegate {
            Debug.Log("So arivato!");
        }).SetEase(Ease.OutBounce);
    }

    #region debug 
    [Header("Debug Grid position")]
    public Vector2 actualGridPosition = Vector2.zero;
    public Transform DebugElement;

    private void Update() {
        //if (DebugElement)
        //    MoveToGridPosition(actualGridPosition);
    }


    #endregion

 
}
