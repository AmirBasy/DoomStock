using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{

    #region Variables
    /// <summary>
    /// testo visibile per ogni risorsa.
    /// </summary>
    public Text FoodText, StoneText, WoodText, FaithText, SpiritText;
    #endregion

    #region Logger

    public Logger logger;

    #endregion

    #region LifeCylce

    private void Update()
    {
        UpdateGraphic();       
    }

    private void UpdateGraphic()
    {
        FoodText.text = " = " + GameManager.I.GetResourceDataByID("Food").Value.ToString();
        StoneText.text = " = " + GameManager.I.GetResourceDataByID("Stone").Value.ToString();
        WoodText.text = " = " + GameManager.I.GetResourceDataByID("Wood").Value.ToString();
        FaithText.text = " = " + GameManager.I.GetResourceDataByID("Faith").Value.ToString();
        SpiritText.text = " = " + GameManager.I.GetResourceDataByID("Spirit").Value.ToString();

    }

    #endregion

    #region Functionalities

    #region Menu

    [Header("All Menu")]
    public PlayerMenuComponent P1_Menu;
    public PlayerMenuComponent P2_Menu;
    public PlayerMenuComponent P3_Menu;
    public PauseMenu PauseMenu;
    public EndGameMenu EndGameMenu;

    #endregion

    #endregion

    #region API
    private void Start()
    {
        SetResourcesTextColor();
        SetFaithTextColor();
    }
    
    /// <summary>
    /// Modifica il colore del Text se la risorsa è bassa
    /// </summary>
    public void SetResourcesTextColor()
    {   
        #region Population
        if (GameManager.I.populationManager.IsFoodEnough() == false)
        {
            FoodText.color = Color.yellow;
        }
        else
        {
            FoodText.color = Color.green;
        }
        #endregion
        #endregion
        #region Stone
        if (GameManager.I.GetResourceDataByID("Stone").Value >= GameManager.I.InitialStone)
        {
            StoneText.color = Color.green;
        }
        if (GameManager.I.GetResourceDataByID("Stone").Value < GameManager.I.InitialStone / 2)
        {
            StoneText.color = Color.yellow;
        }
        if (GameManager.I.GetResourceDataByID("Stone").Value < GameManager.I.InitialStone / 3)
        {
            StoneText.color = Color.red;
        }
        #endregion
        #region Wood
        if (GameManager.I.GetResourceDataByID("Wood").Value >= GameManager.I.InitialWood)
        {
            WoodText.color = Color.green;
        }
        if (GameManager.I.GetResourceDataByID("Wood").Value < GameManager.I.InitialWood /2)
        {
            WoodText.color = Color.yellow;
        }
        if (GameManager.I.GetResourceDataByID("Wood").Value < GameManager.I.InitialWood/3)
        {
            WoodText.color = Color.red;
        }
        
        #endregion
    }

    public void SetFaithTextColor() {
        #region Faith
        if (GameManager.I.GetResourceDataByID("Faith").Value >= GameManager.I.InitialFaith)
        {
            FaithText.color = Color.green;
        }
        if (GameManager.I.GetResourceDataByID("Faith").Value < GameManager.I.InitialFaith / 2)
        {
            FaithText.color = Color.yellow;
        }
        if (GameManager.I.GetResourceDataByID("Faith").Value < GameManager.I.InitialFaith / 3)
        {
            FaithText.color = Color.red;
        }
    }

    public List<ISelectable> FirstLevelSelectables = new List<ISelectable>();

    public IMenu ShowMenu(MenuTypes _type, Player _player)
    {
        FirstLevelSelectables.Clear();
        switch (_type)
        {
            case MenuTypes.Pause:
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Resume", NameLable = "Resume" } as ISelectable);
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Restart", NameLable = "Restart" } as ISelectable);
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Back To Menu", NameLable = "Back To Menu" } as ISelectable);
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Exit", NameLable = "Exit" } as ISelectable);
                FirstLevelSelectables.Add(
                                     new Selector() { UniqueID = "Credits", NameLable = "Credits" } as ISelectable);
                PauseMenu.Init(_player, FirstLevelSelectables);
                return PauseMenu;
            case MenuTypes.EndGame:
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Restart", NameLable = "Restart" } as ISelectable);
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Back To Menu", NameLable = "Back To Menu" } as ISelectable);
                FirstLevelSelectables.Add(
                                      new Selector() { UniqueID = "Exit", NameLable = "Exit" } as ISelectable);
                FirstLevelSelectables.Add(
                                     new Selector() { UniqueID = "Credits", NameLable = "Credits" } as ISelectable);
                EndGameMenu.Init(_player, FirstLevelSelectables);
                return EndGameMenu;
            case MenuTypes.Player:
                CellDoomstock cell = GameManager.I.gridController.Cells[_player.XpositionOnGrid, _player.YpositionOnGrid];
                switch (cell.Status)
                {
                    case CellDoomstock.CellStatus.Empty:

                        if (cell.Type != CellDoomstock.CellType.Forest)
                        {
                            FirstLevelSelectables.Add(
                                       new Selector() { UniqueID = " + Building", NameLable = "Add Building" } as ISelectable);
                        }
                        break;
                    case CellDoomstock.CellStatus.Hole:

                        if (cell.Type != CellDoomstock.CellType.Forest)
                        {
                            FirstLevelSelectables.Add(
                                       new Selector() { UniqueID = " + Building", NameLable = "Add Building" } as ISelectable);
                        }
                        break;
                    case CellDoomstock.CellStatus.Filled:
                        if (cell.building.PlayerOwner == _player)
                        {
                            FirstLevelSelectables.Add(new Selector() { UniqueID = " - Building", NameLable = "Rem Building" } as ISelectable);

                            if (cell.building.CurrentState == BuildingState.Ready)
                            {
                                FirstLevelSelectables.Add(new Selector() { UniqueID = " Prendi ", NameLable = " Prendi " } as ISelectable);
                            }
                        }
                        else
                        {
                            FirstLevelSelectables.Add(new Selector() { UniqueID = " Info ", NameLable = "Info" } as ISelectable);
                        }

                        break;
                    default:
                        break;
                }
                switch (_player.ID)
                {
                    case "Sindaco":
                        P1_Menu.Init(_player, FirstLevelSelectables);
                        return P1_Menu;
                    case "Esercito":
                        P2_Menu.Init(_player, FirstLevelSelectables);
                        return P2_Menu;
                    case "Clero":
                        P3_Menu.Init(_player, FirstLevelSelectables);
                        return P3_Menu;

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


public class Selector : ISelectable
{
    public string UniqueID { get; set; }
    public string NameLable { get; set; }
    public Sprite IconToGet { get; set; }
}