using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeFlow : MonoBehaviour
{
    public TMP_Text YearText;
    public Image[] MonthImage;

    private int yearCounter;
    private int monthCounter;

    public int MonthCounter
    {
        get { return monthCounter; }

        set { monthCounter = value;}
    }

    void Start()
    {
        YearText.text = yearCounter.ToString();
    }
    #region Events
    void Awake()
    {
        TimeEventManager.OnEvent += OnEvent;
    }

    private void OnEvent(TimedEventData _eventData)
    {
        #region FineMese
        if (_eventData.ID == "FineMese")
        {

 
            MonthCounter++;
            FillTheMounthImage();
        }
        #endregion

        #region Year
        if (_eventData.ID == "Anno")
        {
            monthCounter = 0;
            IncreaseYear();

        }
        #endregion
    }


    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnEvent;
    }
    #endregion
    #region API
    /// <summary>
    /// aumenta l'YearCounter ogni volta che viene chiamato
    /// </summary>
    public void IncreaseYear()
    {
        yearCounter++;
        YearText.text = yearCounter.ToString();
    }

    /// <summary>
    /// Colora di verde le immagini del mese
    /// </summary>
    public void FillTheMounthImage()
    {
        switch (MonthCounter)
        {
            case 1:
                MonthImage[0].color = Color.green;
                break;
            case 2:
                MonthImage[1].color = Color.green;
                break;
            case 3:
                MonthImage[2].color = Color.green;
                break;
            case 4:
                MonthImage[3].color = Color.green;
                break;
            case 5:
                MonthImage[4].color = Color.green;
                break;
            case 6:
                MonthImage[5].color = Color.green;
                break;
            case 7:
                MonthImage[6].color = Color.green;
                break;
            case 8:
                MonthImage[7].color = Color.green;
                break;
            case 9:
                MonthImage[8].color = Color.green;
                break;
            case 10:
                MonthImage[9].color = Color.green;
                break;
            case 11:
                MonthImage[10].color = Color.green;
                break;
            case 12:
                MonthImage[11].color = Color.green;
                for (int i = 0; i < GetComponentsInChildren<Image>().Length; i++)
                {
                    MonthImage[i].color = Color.white;
                }
                break;
            default:
                break;
        }
    }


    #endregion
}
