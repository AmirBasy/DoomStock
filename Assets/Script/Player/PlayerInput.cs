using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInput
{
    // Variabili per il funzionamento dei controller
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public PlayerInput(PlayerIndex _playerIndex)
    {
        playerIndex = _playerIndex;
    }

    #region API
    /// <summary>
    /// Ritorna l'input del player nella forma di struttura
    /// </summary>
    /// <returns></returns>
    public InputStatus GetPlayerInputStatus()
    {
        InputStatus inputStatus = ControllerInput();
        if (!inputStatus.IsConnected)
            inputStatus = KeyboardInput();

        return inputStatus;
    }

    /// <summary>
    /// Funzione che permette di utilizzare la vibrazione del controller (da usare con un timer/corutine)
    /// </summary>
    /// <param name="_playerIndex"></param>
    /// <param name="_leftMotor"></param>
    /// <param name="_rightMotor"></param>
    public void SetControllerVibration(PlayerIndex _playerIndex, float _leftMotor, float _rightMotor)
    {
        GamePad.SetVibration(_playerIndex, _leftMotor, _rightMotor);
    }
    #endregion

    #region ControllerInput
    /// <summary>
    /// Controlla l'input da controller (usando il plugin XInputDotNetPure)
    /// </summary>
    InputStatus ControllerInput()
    {
        InputStatus inputStatus = new InputStatus();

        prevState = state;
        state = GamePad.GetState(playerIndex);

        // controlla se il controller è connesso
        if (!state.IsConnected)
        {
            inputStatus.IsConnected = state.IsConnected;
            return inputStatus;
        }
        else
            inputStatus.IsConnected = state.IsConnected;

        // left stick axis => equivalenti ai DPad nel vostro caso
        inputStatus.LeftThumbSticksAxisX = state.ThumbSticks.Left.X;
        inputStatus.LeftThumbSticksAxisY = state.ThumbSticks.Left.Y;

        if (prevState.Buttons.A == XInputDotNetPure.ButtonState.Released && state.Buttons.A == XInputDotNetPure.ButtonState.Pressed)
        {
            // confirm
            inputStatus.A = ButtonState.Pressed;
        }

        if (prevState.Buttons.B == XInputDotNetPure.ButtonState.Released && state.Buttons.B == XInputDotNetPure.ButtonState.Pressed)
        {
            // go back
            inputStatus.B = ButtonState.Pressed;
        }

        if (prevState.Buttons.X == XInputDotNetPure.ButtonState.Released && state.Buttons.X == XInputDotNetPure.ButtonState.Pressed)
        {
            // population menu
            inputStatus.X = ButtonState.Pressed;
        }

        if (prevState.DPad.Up == XInputDotNetPure.ButtonState.Released && state.DPad.Up == XInputDotNetPure.ButtonState.Pressed)
        {
            // up
            inputStatus.DPadUp = ButtonState.Pressed;
        }

        if (prevState.DPad.Left == XInputDotNetPure.ButtonState.Released && state.DPad.Left == XInputDotNetPure.ButtonState.Pressed)
        {
            // left
            inputStatus.DPadLeft = ButtonState.Pressed;
        }

        if (prevState.DPad.Down == XInputDotNetPure.ButtonState.Released && state.DPad.Down == XInputDotNetPure.ButtonState.Pressed)
        {
            // down
            inputStatus.DPadDown = ButtonState.Pressed;
        }

        if (prevState.DPad.Right == XInputDotNetPure.ButtonState.Released && state.DPad.Right == XInputDotNetPure.ButtonState.Pressed)
        {
            // right
            inputStatus.DPadRight = ButtonState.Pressed;
        }

        return inputStatus;
    }
    #endregion

    #region KeyboardInput
    /// <summary>
    /// Controlla l'input da tastiera (la tastiera non funziona se è collegato il controller dello stesso player Index)
    /// </summary>
    InputStatus KeyboardInput()
    {
        InputStatus inputStatus = new InputStatus();

        switch (playerIndex)
        {
            case PlayerIndex.One:
                #region Player One Input
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    // Confirm
                    inputStatus.A = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Go Back
                    inputStatus.B = ButtonState.Pressed;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    //  Add Population
                    inputStatus.X = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    // Up
                    inputStatus.DPadUp = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    // left
                    inputStatus.DPadLeft = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    // down
                    inputStatus.DPadDown = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    // right
                    inputStatus.DPadRight = ButtonState.Pressed;
                }
                #endregion
                break;
            case PlayerIndex.Two:
                #region Player Two Input
                if (Input.GetKeyDown(KeyCode.N))
                {
                    // Confirm
                    inputStatus.A = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    // Go Back
                    inputStatus.B = ButtonState.Pressed;
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    // Population Menu
                    inputStatus.X = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    // Up
                    inputStatus.DPadUp = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.J))
                {
                    // left
                    inputStatus.DPadLeft = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    // down
                    inputStatus.DPadDown = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    // right
                    inputStatus.DPadRight = ButtonState.Pressed;
                }
                #endregion
                break;
            case PlayerIndex.Three:
                #region Player Three Input
                if (Input.GetKeyDown(KeyCode.Home))
                {
                    // Confirm
                    inputStatus.A = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.PageDown))
                {
                    // Go Back
                    inputStatus.B = ButtonState.Pressed;
                }
                if (Input.GetKeyDown(KeyCode.PageUp))
                {
                    // Population Menu
                    inputStatus.X = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    // Up
                    inputStatus.DPadUp = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    // left
                    inputStatus.DPadLeft = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    // down
                    inputStatus.DPadDown = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    // right
                    inputStatus.DPadRight = ButtonState.Pressed;
                }
                #endregion
                break;
            case PlayerIndex.Four:
                #region Player Four Input
                if (Input.GetKeyDown(KeyCode.KeypadMultiply))
                {
                    // Confirm
                    inputStatus.A = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    // Go Back
                    inputStatus.B = ButtonState.Pressed;
                }
                if (Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    // Population Menu
                    inputStatus.X = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    // Up
                    inputStatus.DPadUp = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    // left
                    inputStatus.DPadLeft = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    // down
                    inputStatus.DPadDown = ButtonState.Pressed;
                }

                if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    // right
                    inputStatus.DPadRight = ButtonState.Pressed;
                }
                #endregion
                break;
        }

        return inputStatus;
    }
    #endregion
}

/// <summary>
/// Stato del bottone
/// </summary>
public enum ButtonState
{
    Released = 0,
    Pressed = 1,
    Held = 2
}

/// <summary>
/// Strutta che contine tutti i comandi del joystick
/// </summary>
public struct InputStatus
{
    public bool IsConnected;

    public float LeftTriggerAxis;
    public float RightTriggerAxis;

    public float LeftThumbSticksAxisX;
    public float LeftThumbSticksAxisY;

    public float RightThumbSticksAxisX;
    public float RightThumbSticksAxisY;

    public ButtonState A;
    public ButtonState B;
    public ButtonState X;
    public ButtonState Y;

    public ButtonState LeftShoulder;
    public ButtonState RightShoulder;

    public ButtonState LeftThumbSticks;
    public ButtonState RightThumbSticks;

    public ButtonState DPadUp;
    public ButtonState DPadLeft;
    public ButtonState DPadDown;
    public ButtonState DPadRight;

    public ButtonState Start;
    public ButtonState Select;
}