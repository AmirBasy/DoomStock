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
}
