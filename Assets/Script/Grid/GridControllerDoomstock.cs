using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Grid;
public class GridControllerDoomstock : GridController<CellDoomstock> {

    /// <summary>
    /// Texture da cui prendere i dati.
    /// </summary>
    public Texture2D heightmap, heightmapTerreinType;

    /// <summary>
    /// Restituisce la posizione del building dal suo ID.
    /// </summary>
    /// <param name="uniqueID"></param>
    /// <returns></returns>
    public Vector2 GetBuildingPositionByUniqueID(string uniqueID) {
        foreach (CellDoomstock item in Cells) {
            if (item.building) {
                if (item.building.UniqueID == uniqueID)
                    return item.GridPosition; 
            }
        }
        return new Vector2 (-1,-1);
    }

    public CellDoomstock GetCellFromBuilding(BuildingData _building) {
        Vector2 pos = GetBuildingPositionByUniqueID(_building.UniqueID);
        //TODO: controllare se la cella esiste
        return Cells[(int)pos.x, (int)pos.y];
    }
    /// <summary>
    /// Genera la mappa leggendo i dati dalle texture.
    /// </summary>
    /// <param name="createView"></param>
    protected override void GenerateMap(bool createView = false)
    {
        int heightAmount = 5;

        base.GenerateMap(false);
       

 

        Color[,] terrainTypeColors = GetGridDataFromTexture(heightmapTerreinType, (int)GridSize.x, (int)GridSize.y);
        Color[,] heightValues = GetGridDataFromTexture(heightmap, (int)GridSize.x, (int)GridSize.y);

        foreach (var cell in Cells) {
            // Calcolo altezza
            float colorPixelValue = heightValues[(int)cell.GetGridPosition().x, (int)cell.GetGridPosition().y].grayscale;
            cell.WorldPosition += new Vector3(0, colorPixelValue * heightAmount, 0);
            // Calcolo il tipo di terreno
            //foresta: #000000ff
            //secco: #ff0000ff
            //erba: #00ff00ff
            //roccia: #0000ffff 
            //nullo: #ffffffff
            string colorRGB = ColorUtility.ToHtmlStringRGB(terrainTypeColors[(int)cell.GetGridPosition().x, (int)cell.GetGridPosition().y]);
            switch (colorRGB) {
                case "FFFFFF":
                    cell.SetType(CellDoomstock.CellType.Nullo);
                    cell.Cost = 10;
                    break;
                case "000000":
                    cell.SetType(CellDoomstock.CellType.Forest);
                    break;
                case "FF0000":
                    cell.SetType(CellDoomstock.CellType.Secco);
                    break;
                case "00FF00":
                    cell.SetType(CellDoomstock.CellType.Erba);
                    break;
                case "0000FF":
                    cell.SetType(CellDoomstock.CellType.Roccia);
                    break;
                default:
                    //Debug.Log("colore non trovato " + colorRGB);
                    break;
                    
            }

            if (createView)
                CreateGridTileView(cell.WorldPosition, cell);
            //cell.SetStatus(CellDoomstock.CellStatus.Empty);
            Cells[(int)(GridSize.x / 2), (int)(GridSize.y / 2)].SetStatus(CellDoomstock.CellStatus.Hole);
        }

    }

    /// <summary>
    /// Legge i dati della texture
    /// </summary>
    /// <param name="_texture"></param>
    /// <param name="gridWidth"></param>
    /// <param name="gridHeight"></param>
    /// <returns></returns>
    Color[,] GetGridDataFromTexture(Texture2D _texture, int gridWidth, int gridHeight) {
        Color[,] returnColors = new Color[gridWidth, gridHeight];

        int cellWidth = _texture.width / gridWidth;
        int cellHeight = _texture.height / gridHeight;

        foreach (CellDoomstock cell in Cells) {
            int xPixelPosition = (int)(cell.GridPosition.x * cellWidth) + (cellWidth / 2);
            int yPixelPosition = (int)(cell.GridPosition.y * cellHeight) + (cellHeight / 2);
            Color resultColor = _texture.GetPixel(xPixelPosition, yPixelPosition);
            returnColors[(int)cell.GridPosition.x, (int)cell.GridPosition.y] = resultColor;

        }

        return returnColors;

    }

    /// <summary>
    /// Crea la view della tile.
    /// </summary>
    /// <param name="tilePosition"></param>
    /// <param name="cellData"></param>
    /// <returns></returns>
    protected override GameObject CreateGridTileView(Vector3 tilePosition, CellDoomstock cellData)
    {
        GameObject returnCellView = base.CreateGridTileView(tilePosition, cellData);
        returnCellView.GetComponent<CellView>().Init(cellData as CellDoomstock);
        returnCellView.transform.GetChild(0).transform.localScale = new Vector3(returnCellView.transform.GetChild(0).transform.localScale.x * GameManager.I.CellSize, returnCellView.transform.GetChild(0).transform.localScale.y * GameManager.I.CellSize, returnCellView.transform.GetChild(0).transform.localScale.z * GameManager.I.CellSize);
        return returnCellView;
    }

    /// <summary>
    /// Sposta _player nella posizione della griglia [Xnext, Ynext].
    /// </summary>
    /// <param name="Xnext"></param>
    /// <param name="Ynext"></param>
    /// <param name="_player"></param>
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

    /// <summary>
    /// se è vera, _player può aprire iL menu.
    /// </summary>
    /// <param name="_player"></param>
    /// <returns></returns>
    public bool CanUseMenu(Player _player)
    {
        if (Cells[_player.XpositionOnGrid, _player.YpositionOnGrid].PlayersQueue.LastIndexOf(_player) == 0)
            return true;
        else { return false; }
    }

    /// <summary>
    /// Restituisce la posizione della cella dato lo stato.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public CellDoomstock GetCellPositionByStatus (CellDoomstock.CellStatus status) {
        foreach (CellDoomstock item in Cells) {
            if (item.Status == status)
                return item;
        }
        return null;
    }
  
}


