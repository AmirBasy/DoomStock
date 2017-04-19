using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Grid { 
    [System.Serializable]
    public class Cell {
        public Vector2 GridPosition;
        /// <summary>
        /// The Cell position in world space
        /// </summary>
        public Vector3 WorldPosition;
        /// <summary>
        /// Is a valid position for player
        /// </summary>
        public bool IsValidPosition;
        public int Cost = 1;
    }

}
