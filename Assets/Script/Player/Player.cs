using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;
using XInputDotNetPure;

public class Player : PlayerBase
{
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

    private void Start()
    {
        playerInput = new PlayerInput(InputPlayerIndex);
        LoadResourcesProductionModifiers();
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
    /// Aggiunge all'edificio _building l'unità di popolazione con l'ID _unitIDToAdd
    /// </summary>
    /// <param name="_building"></param>
    public void AddPopulation(BuildingData _building, string _unitIDToAdd)
    {

        if (GameManager.I.populationManager.GetAllFreePeople().Count > 0)
        {
            //aggiunge il popolano all'edificio
            PopulationData pdata = GameManager.I.populationManager.GetUnit(_unitIDToAdd);
            _building.Population.Add(pdata);
            pdata.building = _building;

            //cambia lo stato del building a Producing.
            if (_building.Population.Count == 1 && _building.ID != "Casa" && _building.ID != "Foresta")
            {
                GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).SetBuildingStatus(BuildingState.Producing);
            }

            //GameManager.I.messagesManager.ShowMessage(pdata, PopulationMessageType.AddToBuilding, GameManager.I.buildingManager.GetBuildingView(_building.UniqueID));
            //GameManager.I.messagesManager.ShowBuildingMessage(GameManager.I.buildingManager.GetBuildingView(_building.UniqueID), BuildingMessageType.PeopleAdded);
            GameManager.I.messagesManager.ShowiInformation(MessageLableType.AddPopulation, GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).transform.position, true);

        }

    }

    /// <summary>
    /// Rimuove un popolano dalla lista Population del building
    /// </summary>
    /// <param name="_unitToRemove"></param>
    public void RemovePopulationFromBuilding(string _unitToRemove, BuildingData _buildingData)
    {
        _buildingData.RemoveUnitOfPopulationFromBuilding(_unitToRemove);
        GameManager.I.populationManager.GetPopulationDataByID(_unitToRemove).building = null;
        //GameManager.I.messagesManager.ShowBuildingMessage(GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID), BuildingMessageType.PeopleRemoved);
        GameManager.I.messagesManager.ShowiInformation(MessageLableType.RemovePopulation, GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).transform.position, true);

        //GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).SetPopulationBar();
        if (_buildingData.Population.Count < 1 && _buildingData.currentState != BuildingState.Ready)
        {
            GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).SetBuildingStatus(BuildingState.Built);
        }

    }

    #endregion

    #region Buildings

    /// <summary>
    /// Elenco dei BuildingView che sono stati istanziati nella scena di gioco
    /// </summary>
    public List<BuildingView> BuildingsInScene;

    /// <summary>
    /// Lista degli edifici che il player può usare.
    /// </summary>
    public List<BuildingData> BuildingsDataPrefabs = new List<BuildingData>();

    /// <summary>
    /// building da istanziare.
    /// </summary>
    BuildingView CurrentBuildView;

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

    /// <summary>
    /// Rimuove le macerie dell'edificio passatogli.
    /// </summary>
    /// <param name="_building"></param>
    public void RemoveBuildingDebris(BuildingData _building)
    {
        if (GameManager.I.buildingManager.GetBuildingView(_building.UniqueID))
            GameManager.I.buildingManager.GetBuildingView(_building.UniqueID).RemoveDebris();

    }

    /// <summary>
    /// se è vera, il player può rimuovere le macerie.
    /// </summary>
    /// <returns></returns>
    public bool CanRemoveDebris()
    {
        if (ID == "Esercito")
            return true;
        else
            return false;
    }

    #endregion

    #region Resource collection

    public static Settings GameSettings = null;
    /// <summary>
    /// Contiene tutti i prod modifiers del player.
    /// </summary>
    public ResProductionModifier ProdModifiers = new ResProductionModifier();

    void LoadResourcesProductionModifiers()
    {

        //GameSettings = new Settings() {
        //    ResProductionModifier = new List<ResProductionModifier>() {
        //        new ResProductionModifier(){ Casa = 1, Cava = 1, Chiesa = 1, Fattoria = 1, Foresta = 1, Muro = 1, Player = "asda", Torretta = 1 },
        //        new ResProductionModifier(){ Casa = 1, Cava = 1, Chiesa = 1, Fattoria = 1, Foresta = 1, Muro = 1, Player = "asda", Torretta = 1 },
        //    }
        //};

        //string resultJson = JsonUtility.ToJson(GameSettings);

        if (GameSettings == null)
        {
            TextAsset settingsFile = Resources.Load<TextAsset>("Settings/Settings");
            string jsonData = settingsFile.text;
            GameSettings = JsonUtility.FromJson<Settings>(jsonData);
            Debug.Log(GameSettings);
        }

        ProdModifiers = GameSettings.ResProductionModifier.Find(m => m.Player == ID);
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
                CellDoomstock cell = GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid];
                if (cell.building)
                {
                    if (cell.building.currentState == BuildingState.Ready)
                    {
                        foreach (var item in cell.building.BuildingResources)
                        {
                            GameManager.I.GetResourceDataByID(item.ID).Value += item.Limit;
                            item.Value = 0;

                            if (cell.building.Population.Count > 0)
                            {
                                GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Producing);

                            }
                            else
                            {
                                if (cell.building.ID != "Foresta")
                                {
                                    GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Built);
                                }
                                else if (cell.building.ID == "Foresta")
                                {
                                    GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Producing);
                                }

                            }
                        }
                    }
                    else if (cell.building.currentState == BuildingState.Producing)
                    {

                        AddResourceOnClick(cell.building);
                    }

                }
                else
                {
                    currentMenu = OpenMenuPlayerID();
                    if (currentMenu.PossibiliScelteAttuali.Count < 1)
                        currentMenu.Close();
                }
            }



            if (_inputStatus.X == ButtonState.Pressed) // ADD POPULATION 
            {
                if (GameManager.I.populationManager.GetAllFreePeople().Count > 0)
                {
                    if (GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building)
                    {
                        if (GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building.PopulationLimit > GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building.Population.Count)
                        {
                            List<PopulationData> freePeople = GameManager.I.populationManager.GetAllFreePeople();
                            int randomInd = Random.Range(0, freePeople.Count - 1);
                            AddPopulation(GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building, freePeople[randomInd].UniqueID);
                        }
                    }
                    else { return; }
                }
                else { return; }

            }
            if (_inputStatus.B == ButtonState.Pressed) // DESELECT
            {
                if (GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building != null)
                {
                    BuildingData _building = GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building;
                    if (_building.Population.Count > 0)
                        RemovePopulationFromBuilding(_building.Population[_building.Population.Count - 1].UniqueID, _building);
                }
                else { return; }
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
                if (currentMenu.PossibiliScelteAttuali.Count > 0)
                {
                    currentMenu.AddSelection(currentMenu.PossibiliScelteAttuali[currentMenu.IndiceDellaSelezioneEvidenziata]);
                }
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

    void Update()
    {
        // chiamata alla funzione che legge gli input
        CheckInputStatus(playerInput.GetPlayerInputStatus());
    }

    #endregion

    #region Events

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


    void AddResourceOnClick(BuildingData building)
    {

        switch (building.ID)
        {
            case "Cava":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Cava;
                }
                break;
            case "Foresta":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Foresta;
                }
                break;
            case "Fattoria":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Fattoria;
                }
                break;
            case "Chiesa":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Chiesa;
                }
                break;
            case "Torretta":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Torretta;
                }
                break;
            case "Muro":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Muro;
                }
                
                break;
            case "Casa":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Casa;
                }
                break;
            default:
                break;
        }
    }
}





