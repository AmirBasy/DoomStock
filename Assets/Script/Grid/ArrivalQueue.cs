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
        PlayerPosition[] players = new PlayerPosition[4];

        /// <summary>
        /// Add a player to track
        /// </summary>
        /// <param name="_player">Player to track</param>
        public void AddPlayer (Player _player)
        {
            for (int i = 0; i < 4; i++)
            {
                if(players[i].ID == null)
                {
                    players[i].ID = _player.ID;
                    players[i].coordinates = _player.GetCurrentGridPosition();
                    return;
                }
            }
        }

        public void SetNewPosition (string _playerID, int _x, int _y)
        {
            for (int i = 0; i < 4; i++)
            {
                if(players[i].ID == _playerID)
                {
                    players[i].coordinates[0] = _x;
                    players[i].coordinates[1] = _y;
                    return;
                }                    
            }
        }
        
        struct PlayerPosition
        {
            public string ID;
            public int[] coordinates;
        }
    }
}

