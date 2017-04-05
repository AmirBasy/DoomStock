using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;
using System;

namespace Framework.Grid {

    public class GridController<T> : MonoBehaviour where T : Cell{

        public Transform TilePrefab;
        public Vector2 GridSize = new Vector2(3,3);
        public T[,] Cells;
        public List<Vector2> GridInvalidPositions = new List<Vector2>();
        [HideInInspector]
        public ArrivalQueue playersInQueue = new ArrivalQueue();

        /// <summary>
        /// Generates la griglia utilizzando il TilePrefab
        /// </summary>
        void GenerateMap() {

            for (int x = 0; x < GridSize.x; x++) {
                for (int y = 0; y < GridSize.y; y++)
                {
                    Vector3 tilePosition = new Vector3(x, 0, y);
                    Transform newTile = Instantiate(TilePrefab, tilePosition, Quaternion.identity, this.transform).transform;
                    newTile.name = "Cell["+x+","+y+"]";
                    Cells[x, y] = Activator.CreateInstance<T>();
                    Cells[x, y].WorldPosition = newTile.position;
                    Cells[x, y].GridPosition = new Vector2(x,y);
                    Cells[x, y].IsValidPosition = true;
                }
            }
            //Set the choosen invalid positions
            foreach (Vector2 pos in GridInvalidPositions)
            {
                Cells[(int)pos.x, (int)pos.y].IsValidPosition = false;
            }
        }

        #region API
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
        public bool IsValidPosition(int _x, int _y) {
            if (_x < 0 || _y < 0)
                // posizone del cursore negativa
                return false;
            else if (_x >= GridSize.x || _y >= GridSize.y)
                // posizone del cursore oltre le dimensioni della griglia
                return false;
            else
                //ritorna se la cella in cui si vuole andare è invalida o meno
                return Cells[_x, _y].IsValidPosition;
        }

        /// <summary>
        /// Api che crea la mappa con le dimensioni passate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CreateMap(int x, int y) {
            GridSize.x = x;
            GridSize.y = y;
            Cells = new T[(int)GridSize.x, (int)GridSize.y];
            GenerateMap();
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