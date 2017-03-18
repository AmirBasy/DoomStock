using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Grid { 
    [System.Serializable]
    public struct Cell {
        /// <summary>
        /// The Cell position in world space
        /// </summary>
        public Vector3 WorldPosition;
        /// <summary>
        /// Is a valid position for player
        /// </summary>
        public bool IsValidPosition;
        /// <summary>
        /// Arrival order of players
        /// </summary>
        public string[] PlayerArrivalOrder;
    }

}
