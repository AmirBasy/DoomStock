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
    public Player player;
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
    public GridController GridController;
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
                    Up = KeyCode.W,
                    Left = KeyCode.A,
                    Down = KeyCode.S,
                    Right = KeyCode.D,
                    Confirm = KeyCode.Z,
                    PopulationMenu = KeyCode.X,
                    GoBack = KeyCode.E,
                    
                });
            GridController.playersInQueue.AddPlayer(Players[0]);
            Players[0].SetupGrid(0,0);
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
            GridController.playersInQueue.AddPlayer(Players[1]);
            Players[1].SetupGrid(0,GridController.gridSize[1]-1);
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
            GridController.playersInQueue.AddPlayer(Players[2]);
            Players[2].SetupGrid(GridController.gridSize[0]-1, GridController.gridSize[1]-1);
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
            GridController.playersInQueue.AddPlayer(Players[3]);
            Players[3].SetupGrid(GridController.gridSize[0]-1, 0);
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
        SetupPlayers();
        Food = 10;
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

}

