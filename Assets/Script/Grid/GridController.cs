﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;
using System;

namespace Framework.Grid {

    public class GridController<T> : MonoBehaviour where T : Cell {

        public GameObject TilePrefab;
        public Vector2 GridSize = new Vector2(3,3);
        public T[,] Cells;
        public List<Vector2> GridInvalidPositions = new List<Vector2>();
        [HideInInspector]
        public ArrivalQueue playersInQueue = new ArrivalQueue();

        /// <summary>
        /// Generates la griglia utilizzando il TilePrefab
        /// </summary>
        protected virtual void GenerateMap(bool createView = false) {

            for (int x = 0; x < GridSize.x; x++) {
                for (int y = 0; y < GridSize.y; y++)
                {
                    Vector3 tilePosition = new Vector3(x, 0, y);
                    Cells[x, y] = Activator.CreateInstance<T>();
                    Cells[x, y].WorldPosition = tilePosition;
                    Cells[x, y].GridPosition = new Vector2(x,y);
                    Cells[x, y].IsValidPosition = true;
                    if (createView)
                        CreateGridTileView(tilePosition, Cells[x, y]);
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
            Vector3 tileSize = TilePrefab.transform.localScale;
            return Cells[_x, _y].WorldPosition + new Vector3(0, 0, tileSize.y);
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
    }

    
    public enum Direction {
        up,
        left,
        down,
        right,
    }
}