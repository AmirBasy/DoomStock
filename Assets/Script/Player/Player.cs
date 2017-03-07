using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;

public class Player : PlayerBase {
    public List<BuildingView> BuildingsInScene;
    private void Start()
    {
        Population = 0;
    }

    #region Setup
    /// <summary>
    /// Setto l'input per questo player.
    /// </summary>
    /// <param name="_inputData"></param>
    public void SetupInput(PlayerInputData _inputData) {
        inputData = _inputData;
    }

    /// <summary>
    /// Setta la griglia di pertinenza del player per i movimenti.
    /// </summary>
    /// <param name="_grid"></param>
    public void SetupGrid(GridController _grid, Vector2 _gridStartPosition) {
        grid = _grid;
        MoveToGridPosition(_gridStartPosition);
    }
    #endregion

    #region population


    public void AddPopulation() {
        if (GameManager.I.populationManager.MaxPopulation > 0) {
            GameManager.I.populationManager.MaxPopulation -= 1;
            Population++;
            //// updategraphic("people: " + population + " press q to add, e to remove
            if (BuildingsInScene[0] != null)
                BuildingsInScene[0].gameObject.GetComponent<BuildingView>().UpdateGraphic();
        }
    }

    public void RemovePopulation() {
        Population -= 1;
        GameManager.I.populationManager.MaxPopulation += 1;
        if (Population <= 0)
            Population = 0;
        UpdateGraphic("people: " + Population + " press Q to add, E to remove");
    }


    /// <summary>
    /// Controlla se l'input del player intende aggiungere 1 risorsa popolo alla mia popolazione o
    /// se l'input del player intende rimuovere 1 risorsa popolo alla mia popolazione.
    /// </summary>
    //public override void UsePopulation()
    //{
    //    base.UsePopulation();
    //    //Con Q aggiungo a me 1 di popolazione e lo tolgo al GameManager
        

    //    //Con E tolgo 1 dalla mia popolazione

    //}
    #endregion

    #region Buildings
    public override void DeployBuilding()
    {
        base.DeployBuilding();
        Instantiate(Building[0].gameObject);
        BuildingsInScene.Add(Building[0]);
        BuildingsInScene[0].player = this;
    }

    public override void AddPeopleOnBuilding()
    {
        base.AddPeopleOnBuilding();
        if (Input.GetKeyDown(KeyCode.X) && Population > 0)
        {
            //Aggiungo la popolazione all'edificio
            
        }
    }
    #endregion

    #region grid Movement
    /// <summary>
    /// Muove il player sulla griglia alla posizione indicata.
    /// </summary>
    /// <param name="_gridPosition"></param>
    public void MoveToGridPosition(Vector2 _gridPosition) {
        if (!grid.IsValidPosition(_gridPosition))
            return;
        transform.DOMove(grid.GetWorldCellPosition(_gridPosition),
                    0.7f).OnComplete(delegate {
                        Debug.LogFormat("Movimento player {0} verso {1}", gameObject.name, _gridPosition);
                    }).SetEase(Ease.OutBounce);
        currentGridPosition = _gridPosition;
    }
    #endregion

    #region input

    /// <summary>
    /// Controlla se vengono premuti degli input da parte del player.
    /// </summary>
    void checkInputs() {
        if (inputData == null)
            return;

        if (Input.GetKeyDown(inputData.Up)) {
            MoveToGridPosition(GridController.GetGridPositionByDirection(currentGridPosition, Direction.up));
        }
        if (Input.GetKeyDown(inputData.Left)) {
            MoveToGridPosition(GridController.GetGridPositionByDirection(currentGridPosition, Direction.left));
        }
        if (Input.GetKeyDown(inputData.Down)) {
            MoveToGridPosition(GridController.GetGridPositionByDirection(currentGridPosition, Direction.down));
        }
        if (Input.GetKeyDown(inputData.Right)) {
            MoveToGridPosition(GridController.GetGridPositionByDirection(currentGridPosition, Direction.right));
        }
        if (Input.GetKeyDown(inputData.AddBuilding)) {
            DeployBuilding();
        }
        if (Input.GetKeyDown(inputData.AddPopulation)) {
            AddPopulation();         
        }
        if (Input.GetKeyDown(inputData.RemovePopulation)) {
            RemovePopulation();
        }
    }

    #endregion
    
    void Update()
    {
        checkInputs();
    }
}


