using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;

public abstract class PlayerBase : MonoBehaviour {

    public string ID;
    public List<BuildingView> Building;
    public Text PeopleText;


    private int population;

    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    public PlayerInputData inputData;
    protected GridController grid;
    protected Vector2 currentGridPosition;


	public virtual void UpdateGraphic(string newText)
    {
        PeopleText.text = newText;
    }
    /// <summary>
    /// Istanzia un edificio
    /// </summary>
    public virtual void DeployBuilding() { }
    /// <summary>
    /// Aggiungie la popolazione all'edificio
    /// </summary>
    public virtual void AddPeopleOnBuilding() { }

    #region Move Ability
    public virtual void MoveTo(Vector3 _destination) {
        transform.position = _destination;
    }
    #endregion

}

/// <summary>
/// Conterrà i tasti di input assegnati ad ogni player.
/// </summary>
public class PlayerInputData {
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Right;
    public KeyCode Left;

    public KeyCode AddPopulation;
    public KeyCode RemovePopulation;
    public KeyCode AddBuilding;
}

