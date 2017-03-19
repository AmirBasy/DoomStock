using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuilding : MonoBehaviour {

    /// <summary>
    /// Serve al menu per capire a quale player appartiene.
    /// </summary>
    public string MenuDelPlayer;

    /// <summary>
    /// Il prefab della casella di testo che conterrà il nome del Building che può costruire il player.
    /// </summary>
    public Text BuildingNameBox;

    /// <summary>
    /// La lista di Building che devono essere visualizzati nel menu.
    /// </summary>
    public List<BuildingData> playerBuildingData = new List<BuildingData>();

    
   
    /// <summary>
    /// The RectTransform attached to this GameObject
    /// </summary>
    RectTransform rectTransform;


    // Use this for initialization
    void Start () {
        rectTransform = GetComponent<RectTransform>();
        playerBuildingData = buildingLst();
        
        CreateBuildingNemeBox();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Prende la lista di player dal GameManager, il menu controlla a quale player appartiene e si salva la lista corrispondente.
    /// </summary>
    /// <returns>Ritorna una lista con i Building a seconda se il MenuBuilding si chiama PlayerOne, PlayerTwo, PlayerThree, PlayerFour</returns>
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

    /// <summary>
    /// Per Ogni elemento della lista playerBuildingData, crea una casella di testo e scrive al suo interno il nome dell'oggetto che ne fa parte.
    /// </summary>
    void CreateBuildingNemeBox()
    {
        ///Se la lista di buildingData che riceve dal player è piena
        if (playerBuildingData != null)
        {
            foreach (BuildingData item in playerBuildingData)
            {
                Text tempText;
                tempText = Instantiate(BuildingNameBox, transform, false);
                tempText.text = item.ID;
            }
        }
    }
}
