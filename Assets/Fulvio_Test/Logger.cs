using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour {

    Text LoggerText;
    List<string> PopulationStrings = new List<string>();
    List<string> BuildingStrings = new List<string>();
    List<string> EventStrings = new List<string>();
    List<string> LowPriorityStrings = new List<string>();

    public bool ShowLowPrioLog;
    public bool ShowPopulationLog;
    public bool ShowBuildingLog;
    public bool ShowEventLog;


    private void Start()
    {
        LoggerText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Controlla le booleane per verificare cosa può essere visualizzato e scrive tutta la lista selezionata nella casella di testo
    /// </summary>
    void ShowInLogger()
    {
        ClearLogger();

        if (ShowLowPrioLog)
        {
            for (int i = 0; i < LowPriorityStrings.Count; i++)
            {
                LoggerText.color = Color.white;
                LoggerText.text = DateTime.Now + ": " + LowPriorityStrings[i] + "\n ************ \n" + LoggerText.text;
            }
        }

        if (ShowPopulationLog)
        {
            for (int i = 0; i < PopulationStrings.Count; i++)
            {
                LoggerText.color = Color.yellow;
                LoggerText.text = DateTime.Now + ": " + PopulationStrings[i] + "\n ************ \n" + LoggerText.text;
            }
        }

        if (ShowBuildingLog)
        {
            for (int i = 0; i < BuildingStrings.Count; i++)
            {
                LoggerText.color = Color.green;
                LoggerText.text = DateTime.Now + ": " + BuildingStrings[i] + "\n ************ \n" + LoggerText.text;
            }
        }

        if (ShowEventLog)
        {
            for (int i = 0; i < EventStrings.Count; i++)
            {
                LoggerText.color = Color.blue;
                LoggerText.text = DateTime.Now + ": " + EventStrings[i] + "\n ************ \n" + LoggerText.text;
            }
        }
    }

    /// <summary>
    /// Pulisce la casella di testo del logger
    /// </summary>
    void ClearLogger()
    {
        LoggerText.text = "";
    }

    #region API

    /// <summary>
    /// Salva la stringa che deve scrivere in una lista, pulisce il logger e mostra le stringhe delle liste
    /// </summary>
    /// <param name="_stringToWrite">Cosa scrivere all'interno del logger</param>
    public void WriteInLogger(string _textToWrite, logType _typeOfLog)
    {
        switch (_typeOfLog)
        {
            case logType.Population:
                PopulationStrings.Add(_textToWrite);
                break;

            case logType.Building:
                BuildingStrings.Add(_textToWrite);
                break;

            case logType.Event:
                EventStrings.Add(_textToWrite);
                break;

            case logType.LowPriority:
                LowPriorityStrings.Add(_textToWrite);
                break;

            default:
                break;
        }

        
        ShowInLogger();
        //LoggerText.text = DateTime.Now + ": " + _textToWrite + "\n ************ \n" + LoggerText.text;
    }

    #endregion
}


public enum logType
{
    Population,
    Building,
    Event,
    LowPriority,
}
