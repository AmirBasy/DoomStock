using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;

namespace Framework.Grid {

    public class GridController : MonoBehaviour {

        public Transform TilePrefab;
        public Vector2 GridSize = new Vector2(3, 3);
        public List<Cell> Cells = new List<Cell>();
        public Cell cell = new Cell();


        // Use this for initialization
        void Awake() {
            DontDestroyOnLoad(this.gameObject);
            GenerateMap();
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
                    newTile.name = string.Format("({0},{1}) - ({2})", x, y, newTile.transform.position);
                    Cells.Add(new Cell() { GridPosition = new Vector2(x, y), WorldPosition = newTile.transform.position });
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

        /// <summary>
        /// Restituisce true se la posizione richiesta è valida.
        /// </summary>
        /// <param name="_gridPosition"></param>
        /// <returns></returns>
        public bool IsValidPosition(Vector2 _gridPosition) {
            if (_gridPosition.x < 0 || _gridPosition.y < 0)
                // posizone del cursore negativa
                return false;
            else if(_gridPosition.x > GridSize.x-1 || _gridPosition.y > GridSize.y-1)
                // posizone del cursore oltre le dimensioni della griglia
                return false;
            else
                return true;
        }

        #region helper 

        /// <summary>
        /// Mi restituisce la posizione griglia secondo la direzione in cui voglio andare.
        /// </summary>
        public static Vector2 GetGridPositionByDirection(Vector2 _actualGridPosition, Direction _direction) {
            switch (_direction) {
                case Direction.up:
                    return new Vector2(_actualGridPosition.x, _actualGridPosition.y+1);
                case Direction.left:
                    return new Vector2(_actualGridPosition.x -1, _actualGridPosition.y);
                case Direction.down:
                    return new Vector2(_actualGridPosition.x, _actualGridPosition.y-1);
                case Direction.right:
                    return new Vector2(_actualGridPosition.x+1, _actualGridPosition.y);
                default:
                    // non valida
                    return _actualGridPosition;
            }
        }

        #endregion
    }

    public enum Direction { 
        up,
        left,
        down,
        right,
    }
}