using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager I;
    public Text MainPeopleText;

    #region Variables
    private void Start()
    {
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
/// <summary>
/// Enumeratore che differenzia la tipologia di Edificio edificabile per i Players.
/// </summary>
public enum BuildingType
{
    Player1,
    Player2,
    Player3,
    Player4
    
}
