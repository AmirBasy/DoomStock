using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;

public class Player : PlayerBase
{
    /// <summary>
    /// Elenco dei BuildingView che sono stati istanziati nella scena di gioco
    /// </summary>
    public List<BuildingView> BuildingsInScene;
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
    public void SetupInput(PlayerInputData _inputData)
    {
        inputData = _inputData;
    }

    /// <summary>
    /// Setta la griglia di pertinenza del player per i movimenti.
    /// </summary>
    /// <param name="_grid"></param>
    public void SetupGrid(int _initialX, int _initialY)
    {
        MoveToGridPosition(_initialX, _initialY);
    }
    #endregion

    #region Menu

    void OpenMenuPopulation()
    {
        GameManager.I.uiManager.ShowMenu(MenuTypes.AddPopulation, this);
    }

    void OpenMenuPlayerID()
    {
        GameManager.I.uiManager.ShowMenu(MenuTypes.Player, this);
    }

    #endregion

    #region population

    /// <summary>
    /// Aggiungie la risorsa Population all'Edificio
    /// </summary>
    /// <param name="_buildingView"></param>
    public void AddPopulation(BuildingData _building, PopulationData _unitToAdd)
    {
        //if (GameManager.I.populationManager.MainPopulation() > 0) {
        //    if (BuildingsInScene.Count >= 1)
        //    {
        //       // GameManager.I.populationManager.MainPopulation -= 1;   
        //        _building.Population++; 
        //    }

        //    // TODO: aggiungere il popolano passato come parametro alla lista dei popolani del building e rimuoverlo dalla lista dei disponibili.
        //}
    }

    public void RemovePopulation()
    {
        //Population -= 1;
        ////GameManager.I.populationManager.MainPopulation += 1;
        //if (Population <= 0)
        //    Population = 0;
        //UpdateGraphic("people: " + Population + " press Q to add, E to remove");
    }



    #endregion

    #region Buildings
    /// <summary>
    /// Istanzia una nuovo oggetto Building
    /// </summary>
    public override void DeployBuilding(BuildingData building)
    {
        base.DeployBuilding(building);

        if (CheckResources(building) == true)
        {
            BuildingView newInstanceOfView = GameManager.I.buildingManager.CreateBuild(building);
            CurrentBuildView = newInstanceOfView;
            CurrentBuildView.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            //if (newInstanceOfView.CheckRenderer(newInstanceOfView.gameObject.GetComponent<Renderer>()) == true)
            //{
            BuildingsInScene.Add(newInstanceOfView);
            CurrentBuildView.player = this;
            GameManager.I.populationManager.IncreaseMaxPopulation();
            //}
        }
    }

    /// <summary>
    /// Controlla le risorse necessarie per costruire l'edificio
    /// </summary>
    public bool CheckResources(BuildingData newBuildingData)
    {
        if (GameManager.I.Wood > 0 && GameManager.I.Stone > 0)
        {
            if (newBuildingData.WoodToBuild <= GameManager.I.Wood &&
                newBuildingData.StoneToBuild <= GameManager.I.Stone)
            {
                GameManager.I.RemoveResource(newBuildingData);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Tramite la ui, distrugge un edificio
    /// </summary>
    /// <param name="id"></param>
    public void DestroyBuilding(string id)
    {
        for (int i = 0; i < BuildingsInScene.Count; i++)
        {
            if (id == BuildingsInScene[i].Data.UniqueID)
            {
                Destroy(BuildingsInScene[i].gameObject);
                BuildingsInScene.RemoveAt(i);
            }
        }
    }
    #endregion



    #region grid Movement
    /// <summary>
    /// Muove il player sulla griglia alla posizione indicata.
    /// </summary>
    public void MoveToGridPosition(int _x, int _y)
    {

        if (_x < 0 || _y < 0 || _x > GridController.Grid.GridSize.x - 1 || _y > GridController.Grid.GridSize.y - 1)
            return;
        Cell target = GridController.Grid.Cells[_x, _y];
        if (!target.IsValidPosition)
            return;

        //Actual translation
        transform.DOMove(GridController.Grid.GetCellWorldPosition(_x, _y),
                    0.1f).OnComplete(delegate
                    {
                        Debug.LogFormat("Movimento player {0} - [{1}, {2}]", ID, _x, _y);
                    }).SetEase(Ease.OutSine);

        XpositionOnGrid = _x;
        YpositionOnGrid = _y;

        //Update of the ArrivalQueue
        GridController.Grid.playersInQueue.SetArrivalOrder(this, _x, _y);
    }
    #endregion

    #region input

    /// <summary>
    /// Controlla se vengono premuti degli input da parte del player.
    /// </summary>
    void checkInputs()
    {
        if (inputData == null)
            return;

        if (Input.GetKeyDown(inputData.Up))
        {
            MoveToGridPosition(XpositionOnGrid, YpositionOnGrid + 1);
        }
        if (Input.GetKeyDown(inputData.Left))
        {
            MoveToGridPosition(XpositionOnGrid - 1, YpositionOnGrid);
        }
        if (Input.GetKeyDown(inputData.Down))
        {
            MoveToGridPosition(XpositionOnGrid, YpositionOnGrid - 1);
        }
        if (Input.GetKeyDown(inputData.Right))
        {
            MoveToGridPosition(XpositionOnGrid + 1, YpositionOnGrid);
        }
        if (Input.GetKeyDown(inputData.Confirm))
        {
            // DeployBuilding();
            // Z
            OpenMenuPlayerID();
        }
        if (Input.GetKeyDown(inputData.PopulationMenu))
        {
            //AddPopulation(CurrentBuildView.Data, null);     
            // X    
            OpenMenuPopulation();
        }
        if (Input.GetKeyDown(inputData.GoBack))
        {
            RemovePopulation();
        }
        
    }

    #endregion

    void Update()
    {
        checkInputs();
    }

    #region Events subscription

    private void OnEnable()
    {
        BuildingView.OnDestroy += OnBuildingDestroyed;
    }

    void OnBuildingDestroyed(BuildingView _buildingView)
    {
        if (BuildingsInScene.Contains(_buildingView))
            BuildingsInScene.Remove(_buildingView);
    }

    private void OnDisable()
    {
        BuildingView.OnDestroy -= OnBuildingDestroyed;
    }
}
    #endregion





