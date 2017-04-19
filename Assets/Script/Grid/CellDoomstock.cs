using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;

public class CellDoomstock : Cell {
    public BuildingData building;
    public CellStatus Status = CellStatus.Empty;
   
    //BuildingView _buildingView;
    private List<Player> playersQueue = new List<Player>();

    public List<Player> PlayersQueue {
        get { return playersQueue; }
        set { playersQueue = value; }
    }


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
