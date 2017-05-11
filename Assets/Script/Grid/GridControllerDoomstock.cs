using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;
public class GridControllerDoomstock : GridController<CellDoomstock> {

    public Texture2D heightmap, heightmapTerreinType;

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
        base.GenerateMap(false);
        Cells[(int)(GridSize.x / 2), (int)(GridSize.y / 2)].SetStatus(CellDoomstock.CellStatus.Hole);
        int cellWidth = heightmap.width / (int)GridSize.x;
        int cellHeight = heightmap.height / (int)GridSize.y;
      //  Debug.LogFormat("HM {0} x {1} -> {2} x {3}", heightmap.width, heightmap.height, cellWidth, cellHeight);
        foreach (CellDoomstock cell in Cells) {
            int x = (int)(cell.GridPosition.x * cellWidth) + (cellWidth / 2);
            int y = (int)(cell.GridPosition.y * cellHeight) + (cellHeight / 2);
            Color resultColor = heightmap.GetPixel(x, y);
            Color resultColorBL = heightmapTerreinType.GetPixel(x, y);
            //foresta: #000000ff
            //secco: #ff0000ff
            //erba: #00ff00ff
            //roccia: #0000ffff 
            //nullo: #ffffffff --> 1 1 1 1
            //525252FF
            //
            Color color;
            ColorUtility.TryParseHtmlString("#ffffffff", out color);
            Debug.Log(color);
            switch (ColorUtility.ToHtmlStringRGBA(resultColorBL))
            {
                case "ffffffff":
                    cell.SetType(CellDoomstock.CellType.Nullo);
                    cell.Cost = 10;
                    break;
                case "000000ff":
                    cell.SetType(CellDoomstock.CellType.Forest);
                    break;
                case "ff0000ff":
                    cell.SetType(CellDoomstock.CellType.Secco);
                    break;
                case "00ff00ff":
                    cell.SetType(CellDoomstock.CellType.Erba);
                    break;
                case "0000ffff":
                    cell.SetType(CellDoomstock.CellType.Roccia);
                    break;
                default:
                    Debug.Log("colore non trovato " + ColorUtility.ToHtmlStringRGBA(resultColorBL));
                    break;
            }
            //if (ColorUtility.ToHtmlStringRGBA(resultColorBL))
            //{
            //    cell.SetType(CellDoomstock.CellType.Nullo);
            //    cell.Cost = 10;
            //}
                
            float colorPixelValue = resultColor.grayscale;

           // Debug.LogFormat("Color {0} x {1} -> {2}", cell.GridPosition.x, cell.GridPosition.y, colorPixelValue);

            cell.WorldPosition += new Vector3(0, colorPixelValue * 5, 0);
            if(createView)
                CreateGridTileView(cell.WorldPosition, cell);
        }


    }


    protected override GameObject CreateGridTileView(Vector3 tilePosition, CellDoomstock cellData)
    {
        GameObject returnCellView = base.CreateGridTileView(tilePosition, cellData);
        returnCellView.GetComponent<CellView>().Init(cellData as CellDoomstock);
        returnCellView.transform.GetChild(0).transform.localScale = new Vector3(returnCellView.transform.GetChild(0).transform.localScale.x * GameManager.I.CellSize, returnCellView.transform.GetChild(0).transform.localScale.y * GameManager.I.CellSize, returnCellView.transform.GetChild(0).transform.localScale.z * GameManager.I.CellSize);
        return returnCellView;
    }
    public override void MoveToGridPosition(int Xnext, int Ynext, Player _player)
    {
        int XOldPlayer = _player.XpositionOnGrid ;
        int YOldPlayer = _player.YpositionOnGrid;
        if (Xnext < 0 || Ynext < 0 || Xnext > GridSize.x - 1 || Ynext > GridSize.y - 1)
            return;
        if (Cells[XOldPlayer, YOldPlayer].PlayersQueue.Contains(_player))
            Cells[XOldPlayer, YOldPlayer].PlayersQueue.Remove(_player);
        base.MoveToGridPosition(Xnext, Ynext, _player);
        Cells[Xnext, Ynext].PlayersQueue.Add(_player);
        
    }
    public bool CanUseMenu(Player _player)
    {
        if (Cells[_player.XpositionOnGrid, _player.YpositionOnGrid].PlayersQueue.LastIndexOf(_player) == 0)
            return true;
        else { return false; }
    }

    public CellDoomstock GetCellPositionByStatus (CellDoomstock.CellStatus status) {
        foreach (CellDoomstock item in Cells) {
            if (item.Status == status)
                return item;
        }
        return null;
    }

   
}


