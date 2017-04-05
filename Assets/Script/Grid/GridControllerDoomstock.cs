using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;
public class GridControllerDoomstock : GridController<CellDoomstock> {

public Vector2 GetBuildingPositionByUniqueID(string uniqueID) {
        foreach (CellDoomstock item in Cells) {
            if (item.building) {
                if (item.building.UniqueID == uniqueID)
                    return item.GridPosition; 
            }
        }
        return new Vector2 (-1,-1);
    }
}
