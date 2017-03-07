using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {
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

}
