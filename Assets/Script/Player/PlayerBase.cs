using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBase : MonoBehaviour {

    //public GameObject PlayerPrefab;
    public BuildingType ActualPlayer;
    public List<GameObject> MyBuilding;
    public Text PeopleText;
    public int population = 0;
    


    /// <summary>
    /// Restituisce una lista di tutte le celle nella griglia
    /// </summary>
    public List<Cell> GetAllCellsFromGrid(){

        List<Cell> ReturnList = new List<Cell>();
        foreach (Cell c in GameManager.I.GridController.Cells){
          //ReturnList.AddRange(GameManager.I.GridController.Cells);
          ReturnList.Add(new Cell() { GridPosition = new Vector2(0,0), WorldPosition = GameManager.I.GridController.TilePrefab.transform.position});
          //ReturnList.Add(new Cell() { GridPosition = new Vector2(0, 0), WorldPosition = new Vector3(-transform.position.x, 0, -transform.position.y) }); 
        }
        foreach (Cell c in ReturnList)
        {
            Debug.Log("esistooo");
        }
        return ReturnList;
    }


    void Start()
    {
       
    }


    public virtual void UsePopulation()
    {
        
        
    }
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



}

