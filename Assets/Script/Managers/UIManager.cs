using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour {

    #region Logger

    public Logger logger;

    #endregion

    public Text FoodText, StoneText, WoodText, FaithText, SpiritText, HealthcareText;
    // public Button GridButton, ResourcesButton;
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    public void GoToGridScene() {
        SceneManager.LoadScene("TestGridScene");
    }
    public void GoToResourcesScene() {
        SceneManager.LoadScene("TestPlayerScene");
    }
    private void Update() {
        UpdateGraphic();
    }

    private void UpdateGraphic() {
        FoodText.text = " Food = " + GameManager.I.GetResourceDataByID("Food").Value.ToString();
        StoneText.text = " Stone = " + GameManager.I.GetResourceDataByID("Stone").Value.ToString();
        WoodText.text = " Wood = " + GameManager.I.GetResourceDataByID("Wood").Value.ToString();
        FaithText.text = " Faith = " + GameManager.I.GetResourceDataByID("Faith").Value.ToString();
        SpiritText.text = " Spirit = " + GameManager.I.GetResourceDataByID("Spirit").Value.ToString();
        HealthcareText.text = " Healthcare = " + GameManager.I.GetResourceDataByID("Healthcare").Value.ToString();

    }

    #region Functionalities

    #region Menu
    [Header("All Menu")]
    public MenuBase _menuBase;
    public PlayerMenuComponent P1_Menu;
    public PlayerMenuComponent P2_Menu;
    public PlayerMenuComponent P3_Menu;
    public PlayerMenuComponent P4_Menu;
    #endregion

    #endregion

    #region API
    public List<ISelectable> FirstLevelSelectables = new List<ISelectable>();

    public IMenu ShowMenu(MenuTypes _type, Player _player) {
        FirstLevelSelectables.Clear();
        switch (_type) {
            case MenuTypes.PopulationMenu:
                _menuBase.Init(_player);
                return _menuBase;
            case MenuTypes.Player:
                CellDoomstock cell = GameManager.I.gridController.Cells[_player.XpositionOnGrid, _player.YpositionOnGrid];
                if (cell.Status == CellDoomstock.CellStatus.Empty) {
                    FirstLevelSelectables.Add(
                  new mySelector() { UniqueID = " + Building", NameLable = "Add Building" } as ISelectable);
                } else if (cell.Status == CellDoomstock.CellStatus.Filled) {
                    if (cell.building.PlayerOwner == _player) {
                        FirstLevelSelectables.Add(new mySelector() { UniqueID = " - Building", NameLable = "Rem Building" } as ISelectable);
                        if (cell.building.Population.Count >0)
                        {
                            FirstLevelSelectables.Add(new mySelector() { UniqueID = " -  People", NameLable = "Rem People" } as ISelectable); 
                        }
                        if (GameManager.I.populationManager.GetAllFreePeople().Count>0)
                        {
                            FirstLevelSelectables.Add(new mySelector() { UniqueID = " + People", NameLable = "Add People" } as ISelectable); 
                        }
                    } else {
                        FirstLevelSelectables.Add(new mySelector() { UniqueID = " Info ", NameLable = "Info" } as ISelectable);
                    }
                }
                else if (cell.Status == CellDoomstock.CellStatus.Hole) {
                    FirstLevelSelectables.Add(new mySelector() { UniqueID = " + People", NameLable = "Add People" } as ISelectable);
                }
                switch (_player.ID) {
                    case "PlayerOne":
                        P1_Menu.Init(_player, FirstLevelSelectables);
                        return P1_Menu;
                    case "PlayerTwo":
                        P2_Menu.Init(_player, FirstLevelSelectables);
                        return P2_Menu;
                    case "PlayerThree":
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Miracle" } as ISelectable
                        );
                        P3_Menu.Init(_player, FirstLevelSelectables);
                        return P3_Menu;
                    case "PlayerFour":
                        P4_Menu.Init(_player, FirstLevelSelectables);
                        return P4_Menu;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        return null; // Menù not found
    }

    /// <summary>
    /// Funzione per scrivere all'interno del logger
    /// </summary>
    /// <param name="_stringToWrite">Cosa scrivere all'interno del logger</param>
    public void WriteInLogger(string _stringToWrite, logType _typeOfLog) {
        //controlla se c'è il collegamento al logger 
        if (logger != null)
            logger.WriteInLogger(_stringToWrite, _typeOfLog);
    }

    #endregion
}

public class mySelector : ISelectable {
    public string UniqueID { get; set; }
    public string NameLable { get; set; }
}