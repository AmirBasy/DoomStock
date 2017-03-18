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
        List<PlayerPosition> playersPositions = new List<PlayerPosition>();        

        /// <summary>
        /// Add a player to track
        /// </summary>
        /// <param name="_player">Player to track</param>
        public void AddPlayer (Player _player)
        {
            playersPositions.Add(new PlayerPosition(_player));            
        }

        /// <summary>
        /// Set the arrival order of a certain player onto a certain coordinate
        /// </summary>
        /// <param name="_player">Player arrived</param>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void SetArrivalOrder(Player _player, int _x, int _y)
        {
            if (playersPositions.Count == 0)
                return;

            Vector2 tmpPos;
            PlayerPosition tmp = null;
            int z=0; //Arrival position
            //Count how many are arrived before
            for (int i = 0; i < playersPositions.Count; i++)
            {
                tmpPos = playersPositions[i].player.GetCurrentGridPosition();
                if (tmpPos.x == _x && tmpPos.y == _y && playersPositions[i].player != _player)
                {
                    z++;
                }else if(playersPositions[i].player == _player)
                {
                    tmp = playersPositions[i];
                }
            }
            if(tmp != null)
                tmp.QueuePosition = z;

            TrimArrivalOrder();
        }

        /// <summary>
        /// Trim all the playersPositions elements to fill the possible gaps
        /// </summary>
        public void TrimArrivalOrder()
        {
            for (int i = 0; i < playersPositions.Count; i++)
            {
                for (int j = 0; j < playersPositions.Count; j++)
                {
                    if (i != j
                    && playersPositions[i].player.GetCurrentGridPosition() == playersPositions[j].player.GetCurrentGridPosition()
                    && playersPositions[i].QueuePosition > 0 && playersPositions[i].QueuePosition <= playersPositions[j].QueuePosition)
                    {
                        playersPositions[i].QueuePosition--;
                    }
                }
                Debug.Log(playersPositions[i].player.ID + playersPositions[i].player.GetCurrentGridPosition() + " ArrivalOrder:" + playersPositions[i].QueuePosition);
            }
        }

        /// <summary>
        /// Get how many players are arrived before where _player(parameter) is
        /// </summary>
        /// <param name="_player"></param>
        /// <returns></returns>
        public int GetArrivalOrder(Player _player)
        {
            foreach (PlayerPosition pPos in playersPositions)
            {
                if (pPos.player == _player)
                    return pPos.QueuePosition;
            }
            return -1;
        }

        class PlayerPosition
        {
            public Player player = new Player();
            public Vector2 playerPos = new Vector2();
            public int QueuePosition=0;

            public PlayerPosition(Player _player)
            {
                player = _player;
                playerPos = _player.GetCurrentGridPosition();
            }
        }
    }
}

