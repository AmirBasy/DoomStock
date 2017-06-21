using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
public class UiMainMenu : MonoBehaviour
{

    public PlayerIndex InputPlayerIndex;
    PlayerInput playerInput;

    public PlayerIndex InputPlayerIndex1;
    PlayerInput playerInput1;

    public PlayerIndex InputPlayerIndex2;
    PlayerInput playerInput2;


    private void Start()
    {
        playerInput = new PlayerInput(InputPlayerIndex);
        playerInput1 = new PlayerInput(InputPlayerIndex1);
        playerInput2 = new PlayerInput(InputPlayerIndex2);

    }

    // Update is called once per frame
    void Update()
    {
        CheckInputStatus(playerInput.GetPlayerInputStatus());
        CheckInputStatus(playerInput1.GetPlayerInputStatus());
        CheckInputStatus(playerInput2.GetPlayerInputStatus());
    }

    void CheckInputStatus(InputStatus _inputStatus)
    {
        if (_inputStatus.A == ButtonState.Pressed)
        {
            SceneManager.LoadScene("TestPlayerScene");
        }
    }


}
