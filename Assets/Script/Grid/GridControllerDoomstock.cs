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
    public override void MoveToGridPosition(int _x, int _y, Player _player)
    {
        if (_x < 0 || _y < 0 || _x > GridSize.x - 1 || _y > GridSize.y - 1)
            return;
        if (Cells[_x, _y].PlayersQueue.Contains(_player))
            Cells[_x, _y].PlayersQueue.Remove(_player);
        base.MoveToGridPosition(_x, _y, _player);
        Cells[_x, _y].PlayersQueue.Add(_player);
    }
    public bool CanUseMenu(Player _player)
    {
        if (Cells[_player.XpositionOnGrid, _player.YpositionOnGrid].PlayersQueue.LastIndexOf(_player) == 0)
            return true;
        else { return false; }
    }

}


