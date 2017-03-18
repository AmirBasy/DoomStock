using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Grid {

    /// <summary>
    /// Class that keep track of players and their arrival order in place
    /// </summary>
    public class ArrivalQueue {

        /// <summary>
        /// Players tracked
        /// </summary>
        PlayerPosition[] playersCurrentPositions = new PlayerPosition[4];
        PlayerPosition[] playersOldPositions = new PlayerPosition[4];
        PlayerQueue[][] playersQueue = new PlayerQueue[2][];

        /// <summary>
        /// Add a player to track
        /// </summary>
        /// <param name="_player">Player to track</param>
        public void AddPlayer (Player _player)
        {
            for (int i = 0; i < playersCurrentPositions.Length; i++)
            {
                if(playersCurrentPositions[i].ID == null)
                {
                    playersCurrentPositions[i].ID = _player.ID;
                    playersCurrentPositions[i].coordinates = _player.GetCurrentGridPosition();
                    return;
                }
            }
        }

        void CheckOtherPlayerPosition(string _playerID)
        {
            PlayerPosition player = new PlayerPosition();
            // recupero informazioni del player che ha chiamato la funzione
            for (int i = 0; i < playersCurrentPositions.Length; i++)
            {
                if (playersCurrentPositions[i].ID == _playerID)
                {
                    player = playersCurrentPositions[i];
                }
            }

            // controllo se ci sono altri player che hanno la stessa posizione
            for (int i = 0; i < playersCurrentPositions.Length; i++)
            {
                if (playersCurrentPositions[i].ID != _playerID)
                {
                    if ((playersCurrentPositions[i].coordinates[0] == player.coordinates[0]) && (playersCurrentPositions[i].coordinates[1] == player.coordinates[1]))
                    {
                        int queuePosition = CheckIfQueueAlreadyExist(player.coordinates[0], player.coordinates[1]);
                        if (queuePosition == -1)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if(playersQueue[queuePosition][j] == null)
                                playersQueue[queuePosition][1] = new PlayerQueue() { ArrivalNumber = j + 1, PlayerPosition = player };
                            }
                        }
                        else
                        {
                            queuePosition = GetFirstEmptyQueue();
                            playersQueue[queuePosition] = new PlayerQueue[4];
                            playersQueue[queuePosition][0] = new PlayerQueue() { ArrivalNumber = 1, PlayerPosition = playersCurrentPositions[i] };
                            playersQueue[queuePosition][1] = new PlayerQueue() { ArrivalNumber = 2, PlayerPosition = player };
                        }
                    }
                }
            }
        }

        int CheckIfQueueAlreadyExist(int _x, int _y)
        {
            for (int i = 0; i < playersQueue.Length; i++)
            {
                if ((playersQueue[i][0].PlayerPosition.coordinates[0] == _x) && (playersQueue[i][0].PlayerPosition.coordinates[1] == _y))
                    return i;
            }
            return -1;
        }

        int GetFirstEmptyQueue()
        {
            for (int i = 0; i < playersQueue.Length; i++)
            {
                if (playersQueue[i] == null)
                    return i;
            }
            return -1;
        }

        public void SetNewPosition (string _playerID, int _x, int _y)
        {
            SaveOldPosition(_playerID);
            for (int i = 0; i < playersCurrentPositions.Length; i++)
            {
                if(playersCurrentPositions[i].ID == _playerID)
                {
                    playersCurrentPositions[i].coordinates[0] = _x;
                    playersCurrentPositions[i].coordinates[1] = _y;
                    return;
                }                    
            }
            CheckOtherPlayerPosition(_playerID);
        }

        void SaveOldPosition(string _playerID)
        {
            // se il player è gia nella lista delle precendi posizioni la aggiorna
            for (int i = 0; i < playersCurrentPositions.Length; i++)
            {
                if (playersCurrentPositions[i].ID == _playerID)
                {
                    playersOldPositions[i].coordinates[0] = playersCurrentPositions[i].coordinates[0];
                    playersOldPositions[i].coordinates[1] = playersCurrentPositions[i].coordinates[1];
                    return;
                }
            }

            // se invece non c'è aggiunge il player alla lista e salva la posizione attuale
            for (int i = 0; i < playersCurrentPositions.Length; i++)
            {
                if (playersOldPositions[i].ID == null)
                {
                    playersOldPositions[i].ID = _playerID;
                    playersOldPositions[i].coordinates[0] = playersCurrentPositions[i].coordinates[0];
                    playersOldPositions[i].coordinates[1] = playersCurrentPositions[i].coordinates[1];
                    return;
                }
            }
        }
        
        struct PlayerPosition
        {
            public string ID;
            // [0] = x ; [1] = y;
            public int[] coordinates; 
        }

        class PlayerQueue
        {
            public int ArrivalNumber;
            public PlayerPosition PlayerPosition;
        }
    }
}

