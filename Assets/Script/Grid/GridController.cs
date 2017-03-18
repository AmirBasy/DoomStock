using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Framework.Grid;

namespace Framework.Grid {

    public class GridController : MonoBehaviour {

        public Transform TilePrefab;
        public int[] GridSize = new int[2] { 3, 3 };
        public static Cell[,] Cells;
        public List<Cell> GridInvalidPositions = new List<Cell>();

        static List<PlayerPosition> PlayerGridPostions = new List<PlayerPosition>()
        {
            new PlayerPosition("PlayerOne"),
            new PlayerPosition("PlayerTwo"),
            new PlayerPosition("PlayerThree"),
            new PlayerPosition("PlayerFour"),
        };

        List<Queue> PlayersOnSamePositions = new List<Queue>();

        // Use this for initialization
        void Awake() {
            Cells = new Cell[GridSize[0],GridSize[1]];
            DontDestroyOnLoad(this.gameObject);
            GenerateMap();
        }
        /// <summary>
        /// Generates la griglia utilizzando il TilePrefab
        /// </summary>
        public void GenerateMap() {

            for (int x = 0; x < GridSize[0]; x++) {
                for (int y = 0; y < GridSize[1]; y++)
                {
                    Vector3 tilePosition = new Vector3(x, 0, y);
                    Transform newTile = Instantiate(TilePrefab, tilePosition, Quaternion.identity, this.transform).transform;
                    newTile.name = string.Format("{0}", Cells[x, y]);
                    Cells[x, y].WorldPosition = newTile.position;
                }
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
        public bool IsValidPosition(int _x, int _y) {
            if (_x < 0 || _y < 0)
                // posizone del cursore negativa
                return false;
            else if (_x >= GridSize[0] || _y >= GridSize[1])
                // posizone del cursore oltre le dimensioni della griglia
                return false;
            else
                return Cells[_x, _y].IsValidPosition;
        }

        #region helper 

        /// <summary>
        /// Mi restituisce la posizione griglia secondo la direzione in cui voglio andare.
        /// </summary>
        public static Cell GetGridPositionByDirection(int _x, int _y, Direction _direction) {
            switch (_direction) {
                case Direction.up:
                    return Cells[_x,_y + 1];
                case Direction.left:
                    return Cells[_x - 1, _y];
                case Direction.down:
                    return Cells[_x, _y - 1];
                case Direction.right:
                    return Cells[_x + 1, _y];
                default:
                    return Cells[_x,_y];
            }
        }

        /// <summary>
        /// Setta la posizone corrente del player sulla griglia
        /// </summary>
        /// <param name="_payerID"></param>
        /// <param name="_currentGridPosition"></param>
        public static void SetCurrentPlayerPosition(string _payerID, int _x, int _y)
        {
            foreach (PlayerPosition playerPosition in PlayerGridPostions)
            {
                if (playerPosition.PlayerID == _payerID)
                {
                    playerPosition.CurrentGridPosition = Cells[_x,_y];
                    CheckPlayerPostionOnGrid(_payerID);
                }
            }
        }
        #endregion

        static void CheckPlayerPostionOnGrid(string _payerID)
        {
            for (int i = 0; i < PlayerGridPostions.Count; i++)
            {
                for (int j = 0; j < PlayerGridPostions.Count; j++)
                {
                    if (PlayerGridPostions[i].PlayerID != PlayerGridPostions[j].PlayerID)
                    {
                        if (PlayerGridPostions[i].CurrentGridPosition == PlayerGridPostions[j].CurrentGridPosition)
                        {
                            
                        }

                    }
                }
            }

        }
    }

    public enum Direction {
        up,
        left,
        down,
        right,
    }

    class PlayerPosition
    {
        public string PlayerID;
        public Vector2 CurrentGridPosition;
    
        public PlayerPosition(string _payerID)
        {
            PlayerID = _payerID;
        }
    }

}