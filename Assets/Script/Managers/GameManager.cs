using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Grid;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// se è vera si vede la mappa
    /// </summary>
    public bool DebugMode = false;

    public static GameManager I;

    public int InitialFood, InitialWood, InitialStone;

    public List<BuildingView> forestInScene;
    
    #region Managers
    public GridControllerDoomstock gridController;
    public TimeEventManager timeEventManager;
    public PopulationManager populationManager;
    public BuildingManager buildingManager;
    public ResourcesManager resourcesManager;
    public UIManager uiManager;
    public MessagesManager messagesManager;
    #endregion

    #region Players

    /// <summary>
    /// Lista dei player.
    /// </summary>
    public List<Player> Players;

    /// <summary>
    /// cursore
    /// </summary>
    public GameObject PlayerPrefab;

    public void SetupPlayers() {
        CellDoomstock hole = gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole);
        if (Players[0] != null) {
            Players[0].SetupInput(
                new PlayerInputData()
                {
                    Up = KeyCode.W, //| Input.GetAxis() 
                    Left = KeyCode.A,// | KeyCode.Joystick1Button4,
                    Down = KeyCode.S,// | KeyCode.Joystick1Button5,
                    Right = KeyCode.D,// | KeyCode.Joystick1Button0,
                    Confirm = KeyCode.Z,// | KeyCode.Joystick1Button0,
                    AddPopulationUnit = KeyCode.X, // | KeyCode.Joystick1Button4,
                    GoBack = KeyCode.E,// | KeyCode.Joystick1Button1,
                    PlayerPower = KeyCode.Q

                });

            Players[0].SetUpPosition((int)hole.GridPosition.x - 1, (int)hole.GridPosition.y - 1, CellSize);
        }

        if (Players[1] != null) {
            Players[1].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.I,
                Left = KeyCode.J,
                Down = KeyCode.K,
                Right = KeyCode.L,
                Confirm = KeyCode.N,
                AddPopulationUnit = KeyCode.U,
                GoBack = KeyCode.O,
                PlayerPower = KeyCode.P,
                
            });
            
            Players[1].SetUpPosition((int)hole.GridPosition.x - 1, (int)hole.GridPosition.y + 1, CellSize);
        }

        if (Players[2] != null) {
            Players[2].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.UpArrow,
                Left = KeyCode.LeftArrow,
                Down = KeyCode.DownArrow,
                Right = KeyCode.RightArrow,
                Confirm = KeyCode.Home,
                AddPopulationUnit = KeyCode.PageUp,
                GoBack = KeyCode.PageDown,
                PlayerPower = KeyCode.Insert,
                
            });
            
            Players[2].SetUpPosition((int)hole.GridPosition.x + 1, (int)hole.GridPosition.y + 1, CellSize);
        }

        #region Quarto player commento
        //if (Players[3] != null) {
        //    Players[3].SetupInput(
        //    new PlayerInputData() {
        //        Up = KeyCode.Keypad8,
        //        Left = KeyCode.Keypad4,
        //        Down = KeyCode.Keypad5,
        //        Right = KeyCode.Keypad6,
        //        Confirm = KeyCode.KeypadMultiply,
        //        PopulationMenu = KeyCode.KeypadPlus,
        //        GoBack = KeyCode.KeypadMinus,

        //    });

        //    Players[3].SetUpPosition((int)hole.GridPosition.x + 1, (int)hole.GridPosition.y - 1, CellSize);
        //} 
        #endregion
    }

    #endregion

    #region SETUP
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (I == null)
        {
            I = this;
        }

    }
    public BuildingData forest;
    private void Start()
    {
     
        GridSetUp();
        foreach (var item in buildingManager.buildingsData)
        {
            if (item.ID == "Foresta")
                forest = item;
        }
        SetupForest(forest);
        SetupPlayers();
        SetupResources();

        
    } 
    #endregion

    #region GRID
    public int GridWidth, GridHeight;
    public float CellSize = 1;
    void GridSetUp()
    {
        gridController.CellSize = CellSize;
        gridController.CreateMap(GridWidth, GridHeight, DebugMode);
       

        if (OnGridCreated != null)
            OnGridCreated();
    } 
    #endregion

    #region Risorse

    /// <summary>
    /// Risorse generali del villaggio.
    /// </summary>
    public List<BaseResourceData> resources;



    /// <summary>
    /// Riempie la lista resources di nuove istanze di BaseResouceData.
    /// </summary>
    void SetupResources()
    {
        foreach (BaseResourceData item in Resources.LoadAll<BaseResourceData>("Risorse"))
        {
            resources.Add(Instantiate(item));
        }

        foreach (var item in resources)
        {
            if (item.ID == "Wood")
                item.Value = InitialWood;
            if (item.ID == "Stone")
                item.Value = InitialStone;
            if (item.ID == "Food")
                item.Value = InitialFood;
        }
    }

    #endregion

    void SetupForest(BuildingData forest)
    {       
       BuildingView CurrentBuildView;
        
        foreach (var item in gridController.Cells)
        {
            if (item.Type == CellDoomstock.CellType.Forest)
            {
                CurrentBuildView = buildingManager.CreateBuild(forest);
                CurrentBuildView.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                CurrentBuildView.transform.position = new Vector3(item.WorldPosition.x - (CellSize / 2) +0.5f, item.WorldPosition.y - (CellSize / 2) -0.30f , item.WorldPosition.z+0.5f );
                forestInScene.Add(CurrentBuildView);
                item.SetStatus(CellDoomstock.CellStatus.Filled, CurrentBuildView.Data);
            }
        }
        
    }

    #region API

    /// <summary>
    /// Restituisce la risorsa se gli si passa la stringa
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseResourceData GetResourceDataByID(string id)
    {
        foreach (BaseResourceData item in resources)
        {
            if (item.ID == id)
                return item;
        }
        return null;
    }

    /// <summary>
    /// Rimuove le risorse necessarie per costruire l'edificio
    /// </summary>
    public void RemoveResource(BuildingData data)
    {
        GetResourceDataByID("Wood").Value -= data.WoodToBuild;
        GetResourceDataByID("Stone").Value -= data.StoneToBuild;
    }

    /// <summary>
    /// Restituisce una nuova istanza di resourceData attraverso l'ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseResourceData GetNewInstanceOfResourceData(string id) {
        foreach (BaseResourceData item in Resources.LoadAll<BaseResourceData>("Risorse"))
        {
            if (item.ID == id)
                return Instantiate(item);
        }
        return null;
    }

    #endregion

    #region Eventi
    public delegate void GameEvent();

    public static GameEvent OnGridCreated;
    #endregion

}

