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
    /// Apre il menu della popolazione libera
    /// </summary>
    IMenu OpenPauseMenu()
    {
        if (!GameManager.I.gridController.CanUseMenu(this))
            return null;
        return GameManager.I.uiManager.ShowMenu(MenuTypes.Pause, this);
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
    /// aper il Menu di EndGame
    /// </summary>
    /// <returns></returns>
    IMenu OpenEndGameMenu() {
        if (!GameManager.I.gridController.CanUseMenu(this))
            return null;
        return GameManager.I.uiManager.ShowMenu(MenuTypes.EndGame, this);
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
            GameManager.I.messagesManager.ShowiInformation(MessageLableType.AddPopulation, GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid], true);

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

        // GameManager.I.messagesManager.ShowiInformation(MessageLableType.RemovePopulation, GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).transform.position, true);

        //GameManager.I.buildingManager.GetBuildingView(_buildingData.UniqueID).SetPopulationBar();
        if (_buildingData.Population.Count < 1 && _buildingData.CurrentState != BuildingState.Ready)
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
            CurrentBuildView.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - (GameManager.I.CellSize / 2) - 0.5f, this.transform.position.z + 0.1f);
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
        if (GameManager.I.GetResourceDataByID("Wood").Value >= 0 && GameManager.I.GetResourceDataByID("Stone").Value >= 0)
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
        for (int i = 0; i < GameManager.I.buildingManager.GetAllBuildingInScene().Count; i++)
        {
            if (id == GameManager.I.buildingManager.GetAllBuildingInScene()[i].Data.UniqueID)
            {
                GameManager.I.buildingManager.GetAllBuildingInScene()[i].destroyMe();
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
        GameManager.I.soundManager.GetDebrisSound();
    }

    ///// <summary>
    ///// se è vera, il player può rimuovere le macerie.
    ///// </summary>
    ///// <returns></returns>
    //public bool CanRemoveDebris()
    //{
    //    if (ID == "Esercito")
    //        return true;
    //    else
    //        return false;
    //}

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
    [HideInInspector]public IMenu currentMenu = null;
    /// <summary>
    /// costo di fede per la demolizione
    /// </summary>
    public int DemolitionCost;
    /// <summary>
    /// costo di fede per l'abilità del clero.
    /// </summary>
    public int CleroAbilityCost;
    /// <summary>
    /// costo di fede per l'abilità di riparazione.
    /// </summary>
    public int RiparationCost;

    /// <summary>
    /// Controlla se vengono premuti degli input da parte del player.
    /// </summary>
    void CheckInputStatus(InputStatus _inputStatus)
    {
        if (currentMenu == null)

        {
            CellDoomstock cell = GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid];
            if (_inputStatus.X == ButtonState.Pressed)
            {
                if (cell.building && cell.building.CurrentState != BuildingState.Destroyed)
                    Ability(cell);
            }
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
            if (_inputStatus.A == ButtonState.Pressed) // SELECT
            {
                GameManager.I.soundManager.GetOpenMenuSound();
                if (cell.building)
                {
                    switch (cell.building.CurrentState)
                    {
                        case BuildingState.Construction:
                            break;
                        case BuildingState.Built:
                        case BuildingState.Producing:
                            if (CanClick())
                            {

                                foreach (var item in cell.building.BuildingResources)
                                {
                                    GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).LimitReached(item);
                                }
                                AddResourceOnClick(cell.building, cell);
                                foreach (var item in cell.building.BuildingResources)
                                {
                                    GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).LimitReached(item);
                                }
                            }
                            break;
                        case BuildingState.Ready:
                            if (CanClick())
                            {
                                GameManager.I.messagesManager.DesotryUiInformation(cell);
                                foreach (var item in cell.building.BuildingResources)
                                {
                                    GameManager.I.GetResourceDataByID(item.ID).Value += item.Limit;
                                    switch (item.ID)
                                    {
                                        case "Food":
                                            GameManager.I.soundManager.GetFoodSound();
                                            break;
                                        case "Wood":
                                            GameManager.I.soundManager.GetWoodSound();
                                            break;
                                        case "Faith":
                                            
                                            break;
                                        case "Stone":
                                            GameManager.I.soundManager.GetStoneSound();
                                            break;
                                        default:
                                            break;
                                    }
                                    item.Value = 0;

                                    if (cell.building.Population.Count > 0)
                                    {
                                        GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Waiting);

                                    }
                                    else
                                    {
                                        //if (cell.building.ID != "Foresta")
                                        //{
                                        GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Waiting);

                                        //}
                                        //else if (cell.building.ID == "Foresta")
                                        //{
                                        //    GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Waiting);


                                        //}

                                    }
                                }
                            }
                            break;
                        case BuildingState.Destroyed:
                            GameManager.I.messagesManager.DesotryUiInformation(cell);
                            RemoveBuildingDebris(cell.building);
                            break;
                        case BuildingState.Waiting:
                            break;
                        default:
                            break;
                    }


                }
                else
                {   
                    currentMenu = OpenMenuPlayerID();
                    if (currentMenu.PossibiliScelteAttuali.Count < 1)
                        currentMenu.Close();
                }
            }
            if (_inputStatus.Start == ButtonState.Pressed)
            {
                GameManager.I.soundManager.GetOpenMenuSound();
                currentMenu = OpenPauseMenu();
                Time.timeScale = 0.00000000001f;
                if (currentMenu.PossibiliScelteAttuali.Count < 1)
                    currentMenu.Close();
            }

            if (_inputStatus.RightShoulder == ButtonState.Pressed) // ADD POPULATION 
            {
                if (GameManager.I.populationManager.GetAllFreePeople().Count > 0)
                {
                    BuildingData _building = GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building;
                    if (_building && _building.ID != "Casa" && _building.ID != "Muro")
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
            if (_inputStatus.LeftShoulder == ButtonState.Pressed) // Remove Population
            {
                if (GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building != null)
                {
                    BuildingData _building = GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid].building;
                    if (_building.Population.Count > 0)
                    {
                        RemovePopulationFromBuilding(_building.Population[_building.Population.Count - 1].UniqueID, _building);
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.RemovePopulation, cell, true, "-1");
                    }

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
                GameManager.I.soundManager.GetOpenMenuSound();
                if (currentMenu.PossibiliScelteAttuali.Count > 0)
                {
                    currentMenu.AddSelection(currentMenu.PossibiliScelteAttuali[currentMenu.IndiceDellaSelezioneEvidenziata]);
                }
            }

            if (_inputStatus.B == ButtonState.Pressed)// DESELECT
            {
                GameManager.I.soundManager.BackMenuSound();
                currentMenu.GoBack();
                
            }
        }


    }

    void Update()
    {
        // chiamata alla funzione che legge gli input
        CheckInputStatus(playerInput.GetPlayerInputStatus());
        time += Time.deltaTime;

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

    /// <summary>
    /// aggiunge risorsa all'edificio cliccato
    /// </summary>
    /// <param name="building"></param>
    /// <param name="cell"></param>
    void AddResourceOnClick(BuildingData building, CellDoomstock cell)
    {

        switch (building.ID)
        {
            case "Cava":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Cava;

                    GameManager.I.messagesManager.ShowiInformation(MessageLableType.StoneProduction, cell, true, ProdModifiers.Cava.ToString());
                }
                break;
            case "Foresta":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Foresta;
                    GameManager.I.messagesManager.ShowiInformation(MessageLableType.WoodProduction, cell, true, ProdModifiers.Foresta.ToString());
                    GameManager.I.soundManager.AddWoodOnClickSound();
                }
                break;
            case "Fattoria":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Fattoria;
                    GameManager.I.messagesManager.ShowiInformation(MessageLableType.FoodProduction, cell, true, ProdModifiers.Fattoria.ToString());

                }
                break;
            case "Chiesa":
                foreach (var res in building.BuildingResources)
                {
                    res.Value += ProdModifiers.Chiesa;
                    GameManager.I.messagesManager.ShowiInformation(MessageLableType.FaithProduction, cell, true, ProdModifiers.Chiesa.ToString());
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
                    //GameManager.I.buildingManager.GetBuildingView(building.UniqueID).BarrettaGrow += 0.5f;
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
        if (cell.building.Population.Count == 0)
        {
            GameManager.I.buildingManager.GetBuildingView(building.UniqueID).AnimationStart(building);

            GameManager.I.buildingManager.GetBuildingView(building.UniqueID).AnimationStop(building);
        }
    }

    /// <summary>
    /// abilità dei player
    /// </summary>
    /// <param name="cell"></param>
    void Ability(CellDoomstock cell)
    {
        switch (ID)
        {
            case "Sindaco":
                if (GameManager.I.GetResourceDataByID("Faith").Value >= RiparationCost && cell.building.ID != "Foresta")
                {
                    GameManager.I.GetResourceDataByID("Faith").Value -= RiparationCost;
                    cell.building.BuildingLife = cell.building.InitialLife;
                    //GameManager.I.GetResourceDataByID("Wood").Value += cell.building.GetActualWoodValue();
                    //GameManager.I.GetResourceDataByID("Stone").Value += cell.building.GetActualStoneValue();
                    GameManager.I.messagesManager.ShowiInformation(MessageLableType.Reparing, GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid], true);
                }
                break;
            case "Esercito":
                if (GameManager.I.GetResourceDataByID("Faith").Value >= DemolitionCost && cell.building.ID != "Foresta")
                {
                    DestroyBuilding(cell.building.UniqueID);
                    GameManager.I.GetResourceDataByID("Faith").Value -= DemolitionCost;
                    GameManager.I.messagesManager.ShowiInformation(MessageLableType.Destroing, GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid], true);
                    // GameManager.I.messagesManager.ShowiInformation(MessageLableType.GetMacerie, GameManager.I.gridController.Cells[XpositionOnGrid, YpositionOnGrid]);
                }
                break;
            case "Clero":
                if (GameManager.I.GetResourceDataByID("Faith").Value >= CleroAbilityCost)
                {
                    GameManager.I.GetResourceDataByID("Faith").Value -= CleroAbilityCost;
                    foreach (var item in cell.building.BuildingResources)
                    {
                        item.Value = item.Limit;
                        GameManager.I.buildingManager.GetBuildingView(cell.building.UniqueID).SetBuildingStatus(BuildingState.Ready);
                    }
                }
                break;
            default:
                break;
        }
    }

    #region cooldown
    public float Cooldown_Click;
    float time;
    bool CanClick()
    {
        if (time >= Cooldown_Click)
        {
            time = 0;
            return true;
        }
        return false;
    }
    #endregion

}





