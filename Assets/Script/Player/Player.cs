using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;
using XInputDotNetPure;

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
        playerInput = new PlayerInput(InputPlayerIndex);
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
    public void SetUpPosition(int _initialX, int _initialY, float _size)
    {
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

    #region Population

    /// <summary>
    /// Aggiungie la risorsa Population all'Edificio
    /// </summary>
    /// <param name="_building"></param>
    public void AddPopulation(BuildingData _building, string _unitIDToAdd)
    {
        // TO DO : SPOSTARE IL CONTROLLO(FORSE) DECIDETEVI
        if (GameManager.I.populationManager.GetAllFreePeople().Count > 0)
        {
            PopulationData pdata = GameManager.I.populationManager.GetUnit(_unitIDToAdd);
            _building.Population.Add(pdata);
            _building.currentState = BuildingData.BuildingState.Producing;
            GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).UpdateAspect();
            //GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).SetPopulationBar();
            GameManager.I.messagesManager.ShowMessage(pdata, PopulationMessageType.AddToBuilding, GameManager.I.buildingManager.GetBuildingView(_building.UniqueID));
            GameManager.I.messagesManager.ShowBuildingMessage(GameManager.I.buildingManager.GetBuildingView(_building.UniqueID), BuildingMessageType.PeopleAdded);

        }
        if (GameManager.I.populationManager.GetPopulationDataByID(_unitIDToAdd).Ambition == _building.Ambition)
        {
            GameManager.I.populationManager.GetPopulationDataByID(_unitIDToAdd).IndividualHappiness = true;
            GameManager.I.GetResourceDataByID("Happiness").Value++;
        }
        else { GameManager.I.populationManager.GetPopulationDataByID(_unitIDToAdd).IndividualHappiness = false; }
    }


    /// <summary>
    /// Rimuove un popolano dalla lista Population del building
    /// </summary>
    /// <param name="_unitToRemove"></param>
    public void RemovePopulationFromBuilding(string _unitToRemove, BuildingData _buildingData)
    {
        _buildingData.RemoveUnitOfPopulationFromBuilding(_unitToRemove);
        GameManager.I.messagesManager.ShowBuildingMessage(GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID), BuildingMessageType.PeopleRemoved);
        GameManager.I.populationManager.GetPopulationDataByID(_unitToRemove).IndividualHappiness = false;
        //GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).SetPopulationBar();
        if (_buildingData.Population.Count<1)
        {
            GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).UpdateAspect();
            _buildingData.currentState = BuildingData.BuildingState.Built;
        }
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
            CurrentBuildView.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - (GameManager.I.CellSize / 2) - 1, this.transform.position.z);
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
    public void RemoveBuildingDebris(BuildingData _building)
    {
        if (GameManager.I.buildingManager.GetBuildingView(_building.UniqueID))
            GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).RemoveDebris();

    }

    public bool CanRemoveDebris()
    {
        if (ID == "PlayerTwo")
            return true;
        else
            return false;
    }
    #endregion

    #region Input
    /// <summary>
    /// Indice che viene passato alla classe PlayerInput per determinare quale input si sta ricevendo
    /// </summary>
    public PlayerIndex InputPlayerIndex;
    /// <summary>
    /// Classe che legge gli input sia da tastiera che da controller
    /// </summary>
    PlayerInput playerInput;

    /// <summary>
    /// Variabile che controlla se la levetta sinistra (movimento verticale) è stata rilasciata per evitare che il movimento sia continuo
    /// </summary>
    bool isReleasedHorizontal = true;
    /// <summary>
    /// Variabile che controlla se la levetta sinistra (movimento orizzontale) è stata rilasciata per evitare che il movimento sia continuo
    /// </summary>
    bool isReleasedVertical = true;

    // TODO: rifattorizzare creando state machine player
    IMenu currentMenu = null;

    /// <summary>
    /// Controlla se vengono premuti degli input da parte del player.
    /// </summary>
    void CheckInputStatus(InputStatus _inputStatus)
    {
        if (currentMenu == null)
        {
            // controllo che la levetta sia stata rilasciata nei due sensi o quasi
            if (_inputStatus.LeftThumbSticksAxisY <= 0.2 && _inputStatus.LeftThumbSticksAxisY >= -0.2)
                isReleasedVertical = true;
            if (_inputStatus.LeftThumbSticksAxisX <= 0.2 && _inputStatus.LeftThumbSticksAxisX >= -0.2)
                isReleasedHorizontal = true;

            if ((_inputStatus.DPadUp == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisY >= 0.5) && isReleasedVertical) // GO UP
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid, YpositionOnGrid + 1, this);
                isReleasedVertical = false;
            }
            if ((_inputStatus.DPadLeft == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisX <= -0.5) && isReleasedHorizontal)  // GO LEFT
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid - 1, YpositionOnGrid, this);
                isReleasedHorizontal = false;
            }
            if ((_inputStatus.DPadDown == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisY <= -0.5) && isReleasedVertical) // GO DOWN
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid, YpositionOnGrid - 1, this);
                isReleasedVertical = false;
            }
            if ((_inputStatus.DPadRight == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisX >= 0.5) && isReleasedHorizontal) // GO RIGHT
            {
                GameManager.I.gridController.MoveToGridPosition(XpositionOnGrid + 1, YpositionOnGrid, this);
                isReleasedHorizontal = false;
            }
            if (_inputStatus.A == ButtonState.Pressed && GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].Status != CellDoomstock.CellStatus.Hole) // SELECT
            {
                currentMenu = OpenMenuPlayerID();
            }
            if (_inputStatus.X == ButtonState.Pressed) // POPULATION MENU
            {
                if (GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].Status == CellDoomstock.CellStatus.Hole)
                {
                    currentMenu = OpenMenuPopulation();
                }
            }
            if (_inputStatus.B == ButtonState.Pressed) // DESELECT
            {

            }
        }
        else
        {
            // Menu mode

            // controllo che la levetta sia stata rilasciata nei due sensi o quasi
            if (_inputStatus.LeftThumbSticksAxisY <= 0.2 && _inputStatus.LeftThumbSticksAxisY >= -0.2)
                isReleasedVertical = true;
            if (_inputStatus.LeftThumbSticksAxisX <= 0.2 && _inputStatus.LeftThumbSticksAxisX >= -0.2)
                isReleasedHorizontal = true;

            if ((_inputStatus.DPadUp == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisY >= 0.5) && isReleasedVertical) // GO UP
            {
                currentMenu.MoveToPrevItem();
                isReleasedVertical = false;
            }
            if ((_inputStatus.DPadLeft == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisX <= -0.5) && isReleasedHorizontal)// GO LEFT
            {
                isReleasedHorizontal = false;
            }
            if ((_inputStatus.DPadDown == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisY <= -0.5) && isReleasedVertical) // GO DOWN
            {
                currentMenu.MoveToNextItem();
                isReleasedVertical = false;
            }
            if ((_inputStatus.DPadRight == ButtonState.Pressed || _inputStatus.LeftThumbSticksAxisX >= 0.5) && isReleasedHorizontal) // GO RIGHT
            {
                isReleasedHorizontal = false;
            }
            if (_inputStatus.A == ButtonState.Pressed)// SELECT
            {
                currentMenu.AddSelection(currentMenu.PossibiliScelteAttuali[currentMenu.IndiceDellaSelezioneEvidenziata]);
            }
            if (_inputStatus.X == ButtonState.Pressed)// POPULATION MENU
            {

            }
            if (_inputStatus.B == ButtonState.Pressed)// DESELECT
            {
                currentMenu.GoBack();
            }
        }


    }

    #endregion

    void Update()
    {
        // chiamata alla funzione che legge gli input
        CheckInputStatus(playerInput.GetPlayerInputStatus());
    }

    #region Events Subscription

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





