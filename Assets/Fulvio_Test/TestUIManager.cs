using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour {
    public GameObject PlayerMenuPrefab;
    public List<BuildingData> UibuildingsList = new List<BuildingData>();
    public GameObject BuildingMenuPlayer1;
    public GameObject BuildingMenuPlayer2;
    public GameObject BuildingMenuPlayer3;
    public GameObject BuildingMenuPlayer4;

    
    // Use this for initialization
    void Start () {
        
        GameManager.I.UIPlayerManager = this;
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    #region API

    /// <summary>
    /// Confronta il nome dell'player che riceve e lo cicla per capire quale menu deve visualizzare.
    /// </summary>
    /// <param name="_player"></param>
    public void ActiveMenu(Player _player)
    {
        //GameObject tempMenu;
        switch (_player.ID)
        {
            /// Istanzia il menu corrispondente al player1. Necessario per determinare dove far apparire il menu.
            case "PlayerOne":
                //tempMenu = Instantiate(PlayerMenuPrefab, Player1UI.transform, false);
                //tempMenu.transform.position = new Vector3(Player1UI.transform.position.x + 95, Player1UI.transform.position.y - 45);
                
                if (BuildingMenuPlayer1.active == true)
                {
                    BuildingMenuPlayer1.SetActive(false);
                }
                else
                {
                    BuildingMenuPlayer1.SetActive(true);
                }
                

                break;
            case "PlayerTwo":
                if (BuildingMenuPlayer2.active == true)
                {
                    BuildingMenuPlayer2.SetActive(false);
                }
                else
                {
                    BuildingMenuPlayer2.SetActive(true);
                }
                break;
            case "PlayerThree":
                if (BuildingMenuPlayer3.active == true)
                {
                    BuildingMenuPlayer3.SetActive(false);
                }
                else
                {
                    BuildingMenuPlayer3.SetActive(true);
                }
                break;
            case "PlayerFour":
                if (BuildingMenuPlayer4.active == true)
                {
                    BuildingMenuPlayer4.SetActive(false);
                }
                else
                {
                    BuildingMenuPlayer4.SetActive(true);
                }
                break;
            default:
                Debug.Log("Nessun player corrispondente");
                break;
        }
    }
    
    #endregion
}
