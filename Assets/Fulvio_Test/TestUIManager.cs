using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour {
    public GameObject PlayerMenuPrefab;
    public List<BuildingData> UibuildingsList = new List<BuildingData>();
    public GameObject Player1UI;
    public GameObject Player2UI;
    public GameObject Player3UI;
    public GameObject Player4UI;

    
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
                
                if (Player1UI.active == true)
                {
                    Player1UI.SetActive(false);
                }
                else
                {
                    Player1UI.SetActive(true);
                }
                

                break;
            case "PlayerTwo":
                Debug.Log("Apri menu P2");
                break;
            case "PlayerThree":
                Debug.Log("Apri menu P3");
                break;
            case "PlayerFour":
                Debug.Log("Apri menu P4");
                break;
            default:
                Debug.Log("Nessun player corrispondente");
                break;
        }
    }
    
    public void SendBuildingDataToMenuBuilding(List<BuildingData> _buildingDataPrefabs, Player _player)
    {
        switch (_player.ID)
        {
            case "PlayerOne":
                //Player1UI.GetComponent<MenuBuilding>().playerBuildingData = _buildingDataPrefabs;
                break;
            case "PlayerTwo":
                //Player2UI.GetComponent<MenuBuilding>().playerBuildingData = _buildingDataPrefabs;
                break;
            case "PlayerThree":
                //Player3UI.GetComponent<MenuBuilding>().playerBuildingData = _buildingDataPrefabs;
                break;
            case "PlayerFour":
                //Player4UI.GetComponent<MenuBuilding>().playerBuildingData = _buildingDataPrefabs;
                break;
            default:
                break;
        }
    }

    #endregion
}
