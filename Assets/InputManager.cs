using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputPLayerOne();
        InputPLayerTwo();
        InputPLayerThree();
        InputPLayerFour();

    }

    #region PlayerInput
    void InputPLayerOne()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Movimento W");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Movimento A");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Movimento S");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Movimento D");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Prendo la risorsa Popolo con Q");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Tolgo la risorsa Popolo con E");
        }
    }
    void InputPLayerTwo()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Movimento P2 I");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Movimento P2 j");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Movimento P2 k");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Movimento P2 L");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Prendo la risorsa Popolo con U");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Tolgo la risorsa Popolo con o");
        }
    }
    void InputPLayerThree()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Movimento P3 UpArrow");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Movimento P3 LeftArrow");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Movimento P3 DownArrow");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Movimento P3 RightArrow");
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Debug.Log("Prendo la risorsa Popolo con PageUp");
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Debug.Log("Tolgo la risorsa Popolo con PageDown");
        }
    }
    void InputPLayerFour()
    {
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Debug.Log("Movimento P4 Keypad8");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("Movimento P4 Keypad4");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("Movimento P4 Keypad5");
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Debug.Log("Movimento P4 Keypad6");
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Debug.Log("Prendo la risorsa Popolo con KeypadPlus");
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Debug.Log("Tolgo la risorsa Popolo con KeypadMinus");

        }
    } 
    #endregion
}