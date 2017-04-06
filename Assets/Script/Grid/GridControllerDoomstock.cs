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

    protected override void GenerateMap(bool createView = false)
    {
        base.GenerateMap(createView);
        Cells[(int)(GridSize.x / 2), (int)(GridSize.y / 2)].SetStatus(CellDoomstock.CellStatus.Hole);
    }


    protected override GameObject CreateGridTileView(Vector3 tilePosition, CellDoomstock cellData)
    {
        GameObject returnCellView = base.CreateGridTileView(tilePosition, cellData);
        returnCellView.GetComponent<CellView>().Init(cellData as CellDoomstock);
        return returnCellView;
    }
}


