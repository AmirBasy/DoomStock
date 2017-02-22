using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour {

    public BuildingType ActualPlayer;
    public List<GameObject> MyBuilding;
    public Text PeopleText;
    public int population = 0;

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
