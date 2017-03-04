using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Grid;

public class GameManager : MonoBehaviour {

    public static GameManager I;
    public Text MainPeopleText;
    public List<Player> Players;
    public GameObject PlayerPrefab;
   

    #region Managers
    public GridController GridController;
    public TimeEventManager timeEventManager;

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
                    AddPopulation = KeyCode.Q,
                    RemovePopulation = KeyCode.E,
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
                Up = KeyCode.I,
                Left = KeyCode.J,
                Down = KeyCode.K,
                Right = KeyCode.L,
                AddBuilding = KeyCode.F2,
                AddPopulation = KeyCode.U,
                RemovePopulation = KeyCode.O,
            });
            Players[2].SetupGrid(GridController, new Vector2(GridController.GridSize.x, GridController.GridSize.y));
        }
        if (Players[3] != null) {
            Players[3].SetupInput(
            new PlayerInputData() {
                Up = KeyCode.I,
                Left = KeyCode.J,
                Down = KeyCode.K,
                Right = KeyCode.L,
                AddBuilding = KeyCode.F1,
                AddPopulation = KeyCode.U,
                RemovePopulation = KeyCode.O,
            });
            Players[3].SetupGrid(GridController, new Vector2(GridController.GridSize.x, 0));
        }
    }

    #endregion

    #region Variables
    private void Start()
    {
        SetupPlayers();
        UpdateGraphic("Main People: " + population);
    }

    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    private int population = 100;

    public int Population
    {
        get { return population; }
        set {

            population = value;
           
            if (population > 99)
                population = 100;
            if (population <= 0)
                population = 0;
            UpdateGraphic("Main People: " + population);
        }
    }

    private void UpdateGraphic(string _newText)
    {
        if(MainPeopleText)
            MainPeopleText.text = _newText;
    }


    #region Risorse
    private int resource1;

    public int Resource1
    {
        get { return resource1; }
        set { resource1 = value; }
    }
    private int resource2;

    public int Resource2
    {
        get { return resource2; }
        set { resource2 = value; }
    }

 
    #endregion Risorse


    #endregion Variables


    void Awake()
    {
        if (I == null)
        {
            I = this;
        }
    }



   
}

