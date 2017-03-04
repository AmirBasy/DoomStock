using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Grid;

public abstract class PlayerBase : MonoBehaviour {

    public string ID;
    public List<GameObject> Building;
    public Text PeopleText;
    public int population = 0;
    public PlayerInputData inputData;
    protected GridController grid;
    protected Vector2 currentGridPosition;
    


    /// <summary>
    /// Restituisce una lista di tutte le celle nella griglia
    /// </summary>
    //public List<Cell> GetAllCellsFromGrid(){

    //    List<Cell> ReturnList = new List<Cell>();
    //    foreach (Cell c in GameManager.I.GridController.Cells){
    //      //ReturnList.AddRange(GameManager.I.GridController.Cells);
    //      ReturnList.Add(new Cell() { GridPosition = new Vector2(0,0), WorldPosition = GameManager.I.GridController.TilePrefab.transform.position});
    //      //ReturnList.Add(new Cell() { GridPosition = new Vector2(0, 0), WorldPosition = new Vector3(-transform.position.x, 0, -transform.position.y) }); 
    //    }
    //    foreach (Cell c in ReturnList)
    //    {
    //        Debug.Log("esistooo");
    //    }
    //    return ReturnList;
    //}


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

