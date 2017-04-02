using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;

public class CellDoomstock : Cell {
    public BuildingData building;
    public CellStatus Status = CellStatus.Empty;
    
    public enum CellStatus {
        Empty,
        Filled,
        Hole
    }
	
    public void SetStatus(CellStatus status, BuildingData _building = null) {
        Status = status;
        building = _building;
    }
}
