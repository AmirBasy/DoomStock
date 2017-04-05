using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Grid;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager I;
    public List<Player> Players;
    public GameObject PlayerPrefab;
    public string[] BaseResource;
    public int Food, Wood, Stone, Faith, Spirit;

    
    int healthcare;

    public int Healthcare
    {
        get { return healthcare; }
        set {
            if (value <= 0)
                value = 0;
            healthcare = value;
            
            GameManager.I.populationManager.MaxLife += value;
        }
    }


    #region Managers
    public GridControllerDoomstock gridController;
    public TimeEventManager timeEventManager;
    public PopulationManager populationManager;
    public BuildingManager buildingManager;
    public ResourcesManager resourcesManager;
    public UIManager uiManager;
    //FulvioTestUI
    public TestUIManager UIPlayerManager;
    #endregion

    #region Players

    public void SetupPlayers() {
        if (Players[0] != null) {
            Players[0].SetupInput(
                new PlayerInputData() {
                    Up = KeyCode.W, //| Input.GetAxis() 
                    Left = KeyCode.A,// | KeyCode.Joystick1Button4,
                    Down = KeyCode.S,// | KeyCode.Joystick1Button5,
                    Right = KeyCode.D,// | KeyCode.Joystick1Button0,
                    Confirm = KeyCode.Z | KeyCode.Joystick1Button0,
                    PopulationMenu = KeyCode.X | KeyCode.Joystick1Button4,
                    GoBack = KeyCode.E | KeyCode.Joystick1Button1,

                });
            gridController.playersInQueue.AddPlayer(Players[0]);
            Players[0].SetUpPosition(0,0);
        }

        if (Players[1] != null) {
            Players[1].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.I,
                Left = KeyCode.J,
                Down = KeyCode.K,
                Right = KeyCode.L,
                Confirm = KeyCode.N,
                PopulationMenu = KeyCode.U,
                GoBack = KeyCode.O,
                
            });
            gridController.playersInQueue.AddPlayer(Players[1]);
            Players[1].SetUpPosition(0,(int)gridController.GridSize.y-1);
        }

        if (Players[2] != null) {
            Players[2].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.UpArrow,
                Left = KeyCode.LeftArrow,
                Down = KeyCode.DownArrow,
                Right = KeyCode.RightArrow,
                Confirm = KeyCode.Home,
                PopulationMenu = KeyCode.PageUp,
                GoBack = KeyCode.PageDown,
                
            });
            gridController.playersInQueue.AddPlayer(Players[2]);
            Players[2].SetUpPosition((int)gridController.GridSize.x -1, (int)gridController.GridSize.y-1);
        }
        if (Players[3] != null) {
            Players[3].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.Keypad8,
                Left = KeyCode.Keypad4,
                Down = KeyCode.Keypad5,
                Right = KeyCode.Keypad6,
                Confirm = KeyCode.KeypadMultiply,
                PopulationMenu = KeyCode.KeypadPlus,
                GoBack = KeyCode.KeypadMinus,
                
            });
            gridController.playersInQueue.AddPlayer(Players[3]);
            Players[3].SetUpPosition((int)gridController.GridSize.x-1, 0);
        }
    }

    #endregion

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (I == null)
        {
            I = this;
        }
       
    }

    private void Start()
    {
        //TODO: mettere nel SetUp del gioco
        GridSetUp();
        SetupPlayers(); 
        Food = 10000000;
        Wood = 100;
        Stone = 100;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Rimuove le risorse necessarie per costruire l'edificio
    /// </summary>
    public void RemoveResource(BuildingData data)
    {
        GameManager.I.Wood -= data.WoodToBuild;
        GameManager.I.Stone -= data.StoneToBuild;
    }
    public int X, Y;
    void GridSetUp() {
        gridController.CreateMap(X,Y);
        gridController.Cells[(int)(gridController.GridSize.x / 2), (int)(gridController.GridSize.y / 2)].SetStatus(CellDoomstock.CellStatus.Hole);
        Logger.I.WriteInLogger(string.Format("pozza creata in {0} {1}", (int)(gridController.GridSize.x / 2), (int)(gridController.GridSize.y / 2)), logType.LowPriority);
      

    }


}

