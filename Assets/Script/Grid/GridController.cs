using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;
using System;

namespace Framework.Grid {

    public class GridController<T> : MonoBehaviour where T : Cell {

        public float CellSize = 1;
        public GameObject TilePrefab;
        public Vector2 GridSize = new Vector2(3,3);
        public T[,] Cells;
        public List<Vector2> GridInvalidPositions = new List<Vector2>();
        [HideInInspector]
        //public ArrivalQueue playersInQueue = new ArrivalQueue();

        /// <summary>
        /// Generates la griglia utilizzando il TilePrefab
        /// </summary>
        protected virtual void GenerateMap(bool createView = false) {

            for (int x = 0; x < GridSize.x; x++) {
                for (int z = 0; z < GridSize.y; z++)
                {
                    Vector3 tilePosition = new Vector3(x * CellSize, 0, z * CellSize);
                    Cells[x, z] = Activator.CreateInstance<T>();
                    Cells[x, z].WorldPosition = tilePosition;
                    Cells[x, z].GridPosition = new Vector2(x,z);
                    Cells[x, z].IsValidPosition = true;
                    if (createView)
                        CreateGridTileView(tilePosition, Cells[x, z]);
                }
            }
            //Set the choosen invalid positions
            foreach (Vector2 pos in GridInvalidPositions)
            {
                Cells[(int)pos.x, (int)pos.y].IsValidPosition = false;
            }
        }

        protected virtual GameObject CreateGridTileView(Vector3 tilePosition, T cellData) {
            GameObject newTile = Instantiate<GameObject>(TilePrefab, tilePosition, Quaternion.identity, this.transform);
            newTile.name = "Cell["+ tilePosition.x+","+ tilePosition.y + "]";
            return newTile;
        }

        #region API
        /// <summary>
        /// Restituisce la posizione world della cella alla grid position richiesta
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCellWorldPosition(int _x, int _y) {
            //Vector3 tileSize = TilePrefab.transform.localScale;
            return Cells[_x, _y].WorldPosition + new Vector3(0, -CellSize, CellSize / 2);
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
        public void CreateMap(int x, int y, bool withView = false) {
            GridSize.x = x;
            GridSize.y = y;
            Cells = new T[(int)GridSize.x, (int)GridSize.y];
            GenerateMap(withView);
        }


        #endregion
        #region grid Movement
        /// <summary>
        /// Muove il player sulla griglia alla posizione indicata.
        /// </summary>
        public virtual void MoveToGridPosition(int _x, int _y, Player _player)
        {

            if (_x < 0 || _y < 0 || _x > GridSize.x - 1 || _y > GridSize.y - 1)
                return;
            Cell target = Cells[_x, _y];
            if (_player.MaxCellCost < target.Cost)
                return;

            //Actual translation
            _player.transform.DOMove(GetCellWorldPosition(_x, _y),
                        0.3f).OnComplete(delegate
                        {
                            Debug.LogFormat("Movimento player {0} - [{1}, {2}]", _player.ID, _x, _y);
                        }).SetEase(Ease.OutBack);

            _player.XpositionOnGrid = _x;
            _player.YpositionOnGrid = _y;



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