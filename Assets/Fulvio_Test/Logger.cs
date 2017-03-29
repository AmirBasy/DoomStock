using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour {

    Text LoggerText;
    //float time = 0.00f;
    private void Start()
    {
        LoggerText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        //time = Time.realtimeSinceStartup
    }

    #region API

    /// <summary>
    /// Funzione per scrivere all'interno del logger
    /// </summary>
    /// <param name="_stringToWrite">Cosa scrivere all'interno del logger</param>
    public void WriteInLogger(string _textToWrite)
    {
        LoggerText.text = DateTime.Now + ": " + _textToWrite + "\n ************ \n" + LoggerText.text;
    }

    #endregion
}
