using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {

    public Text FoodText, StoneText, WoodText, FaithText, SpiritText;
    public Button GridButton, ResourcesButton;
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

    }

    #region Functionalities

    #region Menu
    [Header("All Menu")]
    public MenuBase AddPopulationMenu;
    public PlayerMenuComponent P1_Menu;
    public PlayerMenuComponent P2_Menu;
    public PlayerMenuComponent P3_Menu;
    public PlayerMenuComponent P4_Menu;
    #endregion

    #endregion

    #region API

    public void ShowMenu(MenuTypes _type, Player _player) {
        switch (_type) {
            case MenuTypes.AddPopulation:
                AddPopulationMenu.Init(_player);
                break;
            case MenuTypes.AddBuilding:
                switch (_player.ID) {
                    case "PlayerOne":
                        P1_Menu.Init(_player);
                        break;
                    case "PlayerTwo":
                        P2_Menu.Init(_player);
                        break;
                    case "PlayerThree":
                        P3_Menu.Init(_player);
                        break;
                    case "PlayerFour":
                        P4_Menu.Init(_player);
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }
    }

    #endregion
}
