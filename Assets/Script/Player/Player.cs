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
    /// Mette il player nella posizione iniziale
    /// </summary>
    /// <param name="_grid"></param>
    public void SetUpPosition(int _initialX, int _initialY, float _size) {
        transform.localScale = new Vector3(_size, _size, _size);
        GameManager.I.gridController.MoveToGridPosition(_initialX, _initialY, this);
    }
    #endregion

    #region Menu
    /// <summary>
    /// Apre il menu della popolazione libera
    /// </summary>
    IMenu OpenMenuPopulation()
    {
        if (!GameManager.I.gridController.CanUseMenu(this))
            return null;
        return GameManager.I.uiManager.ShowMenu(MenuTypes.PopulationMenu, this);
    }
    /// <summary>
    /// Apre il Menu del Player
    /// </summary>
    IMenu OpenMenuPlayerID()
    {
        if (!GameManager.I.gridController.CanUseMenu(this))
            return null;
        return GameManager.I.uiManager.ShowMenu(MenuTypes.Player, this);
    }

    /// <summary>
    /// Accade quando viene chiuso un menù.
    /// </summary>
    /// <param name="_menuClosed"></param>
    public void OnMenuClosed(IMenu _menuClosed)
    {
        currentMenu = null;
    }

    #endregion

    #region population

    /// <summary>
    /// Aggiungie la risorsa Population all'Edificio
    /// </summary>
    /// <param name="_buildingView"></param>
    public void AddPopulation(BuildingData _building, string _unitIDToAdd)
    {

        // TO DO : SPOSTARE IL CONTROLLO(FORSE) DECIDETEVI
        //if ( _building.Population.Count <_building.PopulationLimit )
        //{
            if (GameManager.I.populationManager.GetAllFreePeople().Count > 0)
            {
                PopulationData pdata = GameManager.I.populationManager.GetUnit(_unitIDToAdd);
                _building.Population.Add(pdata);

            }
            if (GameManager.I.populationManager.GetPopulationDataByID(_unitIDToAdd).Ambition == _building.Ambition)
            {
                GameManager.I.populationManager.GetPopulationDataByID(_unitIDToAdd).IndividualHappiness = true;
                GameManager.I.GetResourceDataByID("Happiness").Value++;
            }
            else { GameManager.I.populationManager.GetPopulationDataByID(_unitIDToAdd).IndividualHappiness = false; } 
       // }
        //return;
    }


    /// <summary>
    /// Rimuove un popolano dalla lista Population del building
    /// </summary>
    /// <param name="_unitToRemove"></param>
    public void RemovePopulationFromBuilding(string _unitToRemove, BuildingData _buildingData)
    {
        _buildingData.RemoveUnitOfPopulationFromBuilding(_unitToRemove);
        GameManager.I.populationManager.GetPopulationDataByID(_unitToRemove).IndividualHappiness = false;
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
            newInstanceOfView.Data.PlayerOwner = this;
            CurrentBuildView = newInstanceOfView;
            CurrentBuildView.transform.localScale = new Vector3(GameManager.I.CellSize, GameManager.I.CellSize, GameManager.I.CellSize);
            CurrentBuildView.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].SetStatus(CellDoomstock.CellStatus.Filled, newInstanceOfView.Data);
            BuildingsInScene.Add(newInstanceOfView);
            GameManager.I.populationManager.IncreaseMaxPopulation();
        }
        CurrentBuildView = null;
    }

    /// <summary>
    /// Controlla le risorse necessarie per costruire l'edificio
    /// </summary>
    public bool CheckResources(BuildingData newBuildingData)
    {
        if (GameManager.I.GetResourceDataByID("Wood").Value > 0 && GameManager.I.GetResourceDataByID("Stone").Value > 0)
        {
            if (newBuildingData.WoodToBuild <= GameManager.I.GetResourceDataByID("Wood").Value &&
                newBuildingData.StoneToBuild <= GameManager.I.GetResourceDataByID("Stone").Value)
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
                BuildingsInScene[i].destroyMe();
            }
        }
    }
    public void RemoveBuildingDebris(BuildingData _building) {
        if (GameManager.I.buildingManager.GetBuildingView(_building.UniqueID))
            GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).RemoveDebris();

    }

    public bool CanRemoveDebris() {
        if (ID == "PlayerTwo")
            return true;
        else
            return false;
    }
    #endregion



    #region input

    // TODO: rifattorizzare creando state machine player
    IMenu currentMenu = null;

    /// <summary>
    /// Controlla se vengono premuti degli input da parte del player.
    /// </summary>
    void checkInputs()
    {
        if (inputData == null)
            return;

        if (currentMenu == null)
        {
            if (Input.GetKeyDown(inputData.Up))
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid, YpositionOnGrid + 1, this);
            }
            if (Input.GetKeyDown(inputData.Left))
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid - 1, YpositionOnGrid, this);
            }
            if (Input.GetKeyDown(inputData.Down))
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid, YpositionOnGrid - 1, this);
            }
            if (Input.GetKeyDown(inputData.Right))
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid + 1, YpositionOnGrid, this);
            }
            if (Input.GetKeyDown(inputData.Confirm))
            {
                currentMenu = OpenMenuPlayerID();
            }
            if (Input.GetKeyDown(inputData.PopulationMenu))
            {
                if (GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].Status == CellDoomstock.CellStatus.Hole)
                {
                    currentMenu = OpenMenuPopulation();
                }
            }
            if (Input.GetKeyDown(inputData.GoBack))
            {

            }
        }
        else
        {  // Menu mode
            if (Input.GetKeyDown(inputData.Up))
            {
                currentMenu.MoveToPrevItem();
            }
            if (Input.GetKeyDown(inputData.Left))
            {
            }
            if (Input.GetKeyDown(inputData.Down))
            {
                currentMenu.MoveToNextItem();
            }
            if (Input.GetKeyDown(inputData.Right))
            {
            }
            if (Input.GetKeyDown(inputData.Confirm))
            {
                currentMenu.AddSelection(currentMenu.PossibiliScelteAttuali[currentMenu.IndiceDellaSelezioneEvidenziata]);
            }
            if (Input.GetKeyDown(inputData.PopulationMenu))
            {

            }
            if (Input.GetKeyDown(inputData.GoBack))
            {
                currentMenu.GoBack();
            }
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
        BuildingView.OnRemoveDebris += OnBuildingDebrisRemove;
    }

    void OnBuildingDebrisRemove(BuildingView _buildingView)
    {
        if (BuildingsInScene.Contains(_buildingView))
            BuildingsInScene.Remove(_buildingView);
    }

    private void OnDisable()
    {
        BuildingView.OnRemoveDebris -= OnBuildingDebrisRemove;
    }
    #endregion
}





