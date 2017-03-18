using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;

public class Player : PlayerBase {
    /// <summary>
    /// Elenco dei BuildingView che sono stati istanziati nella scena di gioco
    /// </summary>
    [HideInInspector]public List<BuildingView> BuildingsInScene;
    public List<BuildingData> BuildingsDataPrefabs = new List<BuildingData>();
    public BuildingView CurrentBuildView;
    private void Start()
    {
        Population = 0;
        //GameManager.I.UIPlayerManager.SendBuildingDataToMenuBuilding(BuildingsDataPrefabs, this);
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
    public void SetupGrid(int _initialX, int _initialY) {
        MoveToGridPosition(_initialX, _initialY);
    }
    #endregion

    #region population

    /// <summary>
    /// Aggiungie la risorsa Population all'Edificio
    /// </summary>
    /// <param name="_buildingView"></param>
    public void AddPopulation(BuildingView _buildingView) {
        if (GameManager.I.populationManager.MaxPopulation > 0) {
            GameManager.I.populationManager.MaxPopulation -= 1;
            _buildingView.Data.Population++;
            if (_buildingView != null)
                _buildingView.gameObject.GetComponent<BuildingView>().UpdateGraphic();
        }
    }

    public void RemovePopulation() {
        Population -= 1;
        GameManager.I.populationManager.MaxPopulation += 1;
        if (Population <= 0)
            Population = 0;
        UpdateGraphic("people: " + Population + " press Q to add, E to remove");
    }


    
    #endregion

    #region Buildings
    /// <summary>
    /// Istanzia una nuovo oggetto Building
    /// </summary>
    public override void DeployBuilding()
    {
        base.DeployBuilding();
        BuildingView newInstanceOfView = GameManager.I.buildingManager.CreateBuild(BuildingsDataPrefabs[0]);
        BuildingsInScene.Add(newInstanceOfView);
        CurrentBuildView = newInstanceOfView;
        CurrentBuildView.player = this;
        GameManager.I.populationManager.IncreaseMaxPopulation();
    }

    #endregion

    #region grid Movement
    /// <summary>
    /// Muove il player sulla griglia alla posizione indicata.
    /// </summary>
    public void MoveToGridPosition(int _x, int _y) {
        Cell target = GridController.Grid.Cells[_x, _y];
        
        if (!target.IsValidPosition)
            return;

        transform.DOMove(GridController.Grid.GetCellWorldPosition(_x,_y),
                    0.1f).OnComplete(delegate {
                        Debug.LogFormat("Movimento player {0} verso {1}", gameObject.name, target);
                    }).SetEase(Ease.OutBounce);

        currentGridPosition[0] = _x;
        currentGridPosition[1] = _y;
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
            MoveToGridPosition(currentGridPosition[0],currentGridPosition[1] + 1);
        }
        if (Input.GetKeyDown(inputData.Left)) {
            MoveToGridPosition(currentGridPosition[0] - 1, currentGridPosition[1]);
        }
        if (Input.GetKeyDown(inputData.Down)) {
            MoveToGridPosition(currentGridPosition[0], currentGridPosition[1] - 1);
        }
        if (Input.GetKeyDown(inputData.Right)) {
            MoveToGridPosition(currentGridPosition[0] + 1, currentGridPosition[1] + 1);
        }
        if (Input.GetKeyDown(inputData.AddBuilding)) {
            DeployBuilding();
        }
        if (Input.GetKeyDown(inputData.AddPopulation)) {
            AddPopulation(CurrentBuildView);         
        }
        if (Input.GetKeyDown(inputData.RemovePopulation)) {
            RemovePopulation();
        }
        //FulvioTestUI
        if (Input.GetKeyDown(inputData.OpenMenu))
        {
            ActiveMenuInUIManager();
        }
    }

    #endregion
    
    void Update()
    {
        checkInputs();        
    }



    #region Fulvio Test UI
    
    /// <summary>
    /// FulvioTestUI
    /// Va a richiamare la funzione ActiveMenu, presente nel UIPlayer, passando se stesso.
    /// </summary>
    void ActiveMenuInUIManager()
    {
        GameManager.I.UIPlayerManager.ActiveMenu(this);
        
    }

    #endregion

}


