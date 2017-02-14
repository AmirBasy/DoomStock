using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;


    #region Variable

    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    private int population;
    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    /// <summary>
    /// Risorse generiche in quanto non sono ancora state definite.
    /// </summary>
    public int Resource1, Resource2, Resource3, Resource4;

    #endregion
    void Awake()
    {
        if (I == null)
        {
            I = this;
        }
    }
    /// <summary>
    /// Gestice la velocita dello sviluppo delle costruzioni.
    /// </summary>
    /// <param name="_population">The population.</param>
    public void ConstructionTime(int _population) {

    }
}
