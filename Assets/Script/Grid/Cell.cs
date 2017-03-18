using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Grid { 
    [System.Serializable]
    public struct Cell {

        public Vector3 WorldPosition;
        public bool IsValidPosition;
    }

}
