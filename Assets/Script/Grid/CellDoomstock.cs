using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;
using System;

public class CellDoomstock : Cell, INode {
    public BuildingData building;
    public CellStatus Status = CellStatus.Empty;
   
    //BuildingView _buildingView;
    private List<Player> playersQueue = new List<Player>();

    public List<Player> PlayersQueue {
        get { return playersQueue; }
        set { playersQueue = value; }
    }

    #region INode
    public int G_Cost { get; set; }
    public int H_Cost { get; set; }
    public int F_Cost { get { return G_Cost + H_Cost; } }

    public INode parent { get; set; }
    public bool isTraversable {
        get {
            if (Status == CellStatus.Empty)
                return true;
            return false;
        }
       
        
    }

    public Vector2 GetGridPosition() {
        throw new NotImplementedException();
    }

    public Vector3 GetWorldPosition() {
        throw new NotImplementedException();
    }

    public List<INode> GetNeighbours() {
        throw new NotImplementedException();
    }
    #endregion

    public enum CellStatus {
        Empty,
        Filled,
        Hole,
        Debris
    }
	
    public void SetStatus(CellStatus status, BuildingData _building = null) {
        Status = status;
        building = _building;
        if (OnDataChanged != null)
            OnDataChanged(this);
    }

  

    #region Events
    public delegate void CellEvent(CellDoomstock data);

    public CellEvent OnDataChanged;
    #endregion

   
}
