using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;

namespace Framework.Grid {

    public class GridController : MonoBehaviour {

        public Transform TilePrefab;
        public Vector2 GridSize = new Vector2(3,3);
        public Cell[,] Cells;
        public List<Vector2> GridInvalidPositions = new List<Vector2>();
        [HideInInspector]
        public int[] gridSize;

        public static GridController Grid;

        // Use this for initialization
        void Awake() {
            //Singleton paradigm
            if (Grid != null)
                DestroyImmediate(this);
            else
                Grid = this;

            gridSize = new int[2] {(int)GridSize.x, (int)GridSize.y };
            Cells = new Cell[gridSize[0],gridSize[1]];

            DontDestroyOnLoad(this.gameObject);
            GenerateMap();
        }
        /// <summary>
        /// Generates la griglia utilizzando il TilePrefab
        /// </summary>
        public void GenerateMap() {

            for (int x = 0; x < gridSize[0]; x++) {
                for (int y = 0; y < gridSize[1]; y++)
                {
                    Vector3 tilePosition = new Vector3(x, 0, y);
                    Transform newTile = Instantiate(TilePrefab, tilePosition, Quaternion.identity, this.transform).transform;
                    newTile.name = "Cell["+x+","+y+"]";
                    Cells[x, y].WorldPosition = newTile.position;
                    Cells[x, y].IsValidPosition = true;
                    Cells[x, y].PlayerArrivalOrder = new string[4];
                }
            }
            //Set the choosen invalid positions
            foreach (Vector2 pos in GridInvalidPositions)
            {
                Cells[(int)pos.x, (int)pos.y].IsValidPosition = false;
            }
        }

        /// <summary>
        /// Restituisce la posizione world della cella alla grid position richiesta
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCellWorldPosition(int _x, int _y) {

            return Cells[_x, _y].WorldPosition;
        }

        /// <summary>
        /// Restituisce true se la posizione richiesta è valida.
        /// </summary>
        /// <returns></returns>
        public bool IsValidPosition(int _x, int _y)
        {
            if (_x < 0 || _y < 0)
                // posizone del cursore negativa
                return false;
            else if (_x >= gridSize[0] || _y >= gridSize[1])
                // posizone del cursore oltre le dimensioni della griglia
                return false;
            else
                return Cells[_x, _y].IsValidPosition;
        }        
    }

    public enum Direction {
        up,
        left,
        down,
        right,
    }
}