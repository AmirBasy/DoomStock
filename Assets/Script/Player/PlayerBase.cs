using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;

public abstract class PlayerBase : MonoBehaviour {

    public string ID;
    public Text PeopleText;

    private int population;

    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    public PlayerInputData inputData;
    protected GridController<CellDoomstock> grid;
    public int XpositionOnGrid;
    public int YpositionOnGrid;

    public virtual void UpdateGraphic(string newText)
    {
        PeopleText.text = newText;
    }
    /// <summary>
    /// Istanzia un edificio
    /// </summary>
    public virtual void DeployBuilding(BuildingData building) { }

    #region Move Ability
    public virtual void MoveTo(Vector3 _destination) {
        transform.position = _destination;
    }
    #endregion

    public Vector2 GetCurrentGridPosition()
    {
        return new Vector2(XpositionOnGrid,YpositionOnGrid);
    }
}

/// <summary>
/// Conterrà i tasti di input assegnati ad ogni player.
/// </summary>
public class PlayerInputData {
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Right;
    public KeyCode Left;

    public KeyCode PopulationMenu;
    public KeyCode GoBack;
    public KeyCode Confirm;

}

