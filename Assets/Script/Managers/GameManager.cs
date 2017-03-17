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
    public int Food;
    
   

    #region Managers
    public GridController GridController;
    public TimeEventManager timeEventManager;
    public PopulationManager populationManager;
    public BuildingManager buildingManager;
    //FulvioTestUI
    public TestUIPlayer UIPlayerManager;
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
                    AddBuilding = KeyCode.Z,
                    AddPopulation = KeyCode.X,
                    RemovePopulation = KeyCode.E,
                    //FulvioTestUI
                    OpenMenu = KeyCode.Space,
                });
            Players[0].SetupGrid(GridController, new Vector2(0, 0));
        }

        if (Players[1] != null) {
            Players[1].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.I,
                Left = KeyCode.J,
                Down = KeyCode.K,
                Right = KeyCode.L,
                AddBuilding = KeyCode.N,
                AddPopulation = KeyCode.U,
                RemovePopulation = KeyCode.O,
            });
            Players[1].SetupGrid(GridController, new Vector2(0, GridController.GridSize.y));
        }

        if (Players[2] != null) {
            Players[2].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.UpArrow,
                Left = KeyCode.LeftArrow,
                Down = KeyCode.DownArrow,
                Right = KeyCode.RightArrow,
                AddBuilding = KeyCode.F2,
                AddPopulation = KeyCode.PageUp,
                RemovePopulation = KeyCode.PageDown,
            });
            Players[2].SetupGrid(GridController, new Vector2(GridController.GridSize.x, GridController.GridSize.y));
        }
        if (Players[3] != null) {
            Players[3].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.Keypad8,
                Left = KeyCode.Keypad4,
                Down = KeyCode.Keypad5,
                Right = KeyCode.Keypad6,
                AddBuilding = KeyCode.F1,
                AddPopulation = KeyCode.KeypadPlus,
                RemovePopulation = KeyCode.KeypadMinus,
            });
            Players[3].SetupGrid(GridController, new Vector2(GridController.GridSize.x, 0));
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
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

   
}

