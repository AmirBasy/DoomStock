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
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void GoToGridScene() {
        SceneManager.LoadScene("TestGridScene");
    }
    public void GoToResourcesScene() {
        SceneManager.LoadScene("TestPlayerScene");
    }
    private void Update()
    {
        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        FoodText.text = " Food = " + GameManager.I.Food.ToString();
        StoneText.text = " Stone = " + GameManager.I.Stone.ToString();
        WoodText.text = " Wood = " + GameManager.I.Wood.ToString();
        FaithText.text = " Faith = " + GameManager.I.Faith.ToString();
        SpiritText.text = " Spirit = " + GameManager.I.Spirit.ToString();
        HealthcareText.text = " Healthcare = " + GameManager.I.Healthcare.ToString();

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
                switch (_player.ID)
                {   
                    case "PlayerOne":
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " + Building" } as ISelectable
                            );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " - Building" } as ISelectable
                            );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " -  People" } as ISelectable
                            );
                        P1_Menu.Init(_player, FirstLevelSelectables);
                        return P1_Menu;
                    case "PlayerTwo":
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " + Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Remove Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Remove People" } as ISelectable
                        );
                        P2_Menu.Init(_player, FirstLevelSelectables);
                        return P2_Menu;
                    case "PlayerThree":
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " + Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " -Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Remove People" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Miracle" } as ISelectable
                        );
                        P3_Menu.Init(_player, FirstLevelSelectables);
                        return P3_Menu;
                    case "PlayerFour":
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = " + Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Remove Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Remove Building" } as ISelectable
                        );
                        FirstLevelSelectables.Add(
                            new mySelector() { UniqueID = "Qualcosa" } as ISelectable
                            );
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
    public void WriteInLogger(string _stringToWrite, logType _typeOfLog)
    {
        //controlla se c'è il collegamento al logger 
        if (logger != null)
            logger.WriteInLogger(_stringToWrite, _typeOfLog);
    }

    #endregion
}

public class mySelector : ISelectable
{
    public string UniqueID    {get;
              set;
    }
}