using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;


    #region Variables

    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    private int population = 100;
    public int Population
    {
        get { return population; }
        set { population = value; }
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
