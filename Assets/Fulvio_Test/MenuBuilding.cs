using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuilding : MonoBehaviour {

    public string MenuDelPlayer;
    public Text NameBuilding1;
    public Text NameBuilding2;

    public List<BuildingData> playerBuildingData = new List<BuildingData>();

    
   
    /// <summary>
    /// The RectTransform attached to this GameObject
    /// </summary>
    RectTransform rectTransform;


    // Use this for initialization
    void Start () {
        rectTransform = GetComponent<RectTransform>();
        playerBuildingData = buildingLst();

    }


	
	// Update is called once per frame
	void Update () {
        if (playerBuildingData != null)
        {
            NameBuilding1.text = playerBuildingData[0].ID;
            NameBuilding2.text = playerBuildingData[1].ID;
        }
		if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // La somma totale delle altezze delle caselle contenute all'interno del menu
            float TotalHeight = NameBuilding1.rectTransform.sizeDelta.y + NameBuilding2.rectTransform.sizeDelta.y;
            
            /// Controllo se l'altezza del menu è minore della somma delle altezze delle caselle di testo contenute al suo interno
            
            if (rectTransform.sizeDelta.y < TotalHeight)
                /// ingrandisco il menu.
                Debug.Log("Ingrandisco il menu");
        }
	}

    public List<BuildingData> buildingLst()
    {
        List<BuildingData> newlist = new List<BuildingData>();
        foreach (Player player in GameManager.I.Players)
        {
            if (player.ID == MenuDelPlayer)
            {
                foreach (BuildingData building in player.BuildingsDataPrefabs)
                {

                    newlist.Add(building);
                    //return newlist;
                }
            }
        }
        return newlist;
    }
}
