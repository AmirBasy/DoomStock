
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeEventManager : MonoBehaviour {

    public Image[] month;
    public int MounthCounter;
    Text yearText;


    /// <summary>
    /// aumenta il contatore di uno ogni volta che passa un evento Mese
    /// </summary>
    public void IncreaseMounthCounter() {
        foreach (TimedEventData item in EventsPrefab)
        {
            if (item.ID == "FineMese")
            {
                MounthCounter++;     
            }
        }
        if (MounthCounter>=12)
        {
            MounthCounter = 0;
        }
    }

    public void FillTheMounthImage() {
        switch (MounthCounter)
        {
            case 1:
                GetComponentInChildren<Image>();
                month[0].color = Color.green;
                break;
            case 2:
                GetComponentInChildren<Image>();
                month[1].color = Color.green;
                break;
            case 3:
                GetComponentInChildren<Image>();
                month[2].color = Color.green;
                break;
            case 4:
                GetComponentInChildren<Image>();
                month[3].color = Color.green;
                break;
            case 5:
                GetComponentInChildren<Image>();
                month[4].color = Color.green;
                break;
            case 6:
                GetComponentInChildren<Image>();
                month[5].color = Color.green;
                break;
            case 7:
                GetComponentInChildren<Image>();
                month[6].color = Color.green;
                break;
            case 8:
                GetComponentInChildren<Image>();
                month[7].color = Color.green;
                break;
            case 9:
                GetComponentInChildren<Image>();
                month[8].color = Color.green;
                break;
            case 10:
                GetComponentInChildren<Image>();
                month[9].color = Color.green;
                break;
            case 11:
                GetComponentInChildren<Image>();
                month[10].color = Color.green;
                break;
            case 12:
                GetComponentInChildren<Image>();
                month[11].color = Color.green;
                break;
            default:
                break;
        }
    }

    #region Events
    public delegate void GameTimedEvent(TimedEventData _eventData);

    public static GameTimedEvent OnEvent;
    #endregion

    public List<TimedEventData> EventsPrefab = new List<TimedEventData>();
    [HideInInspector]
    protected List<TimedEventData> Events = new List<TimedEventData>();

    void Start()
    {
        foreach (TimedEventData ev in EventsPrefab) {
            Events.Add(Instantiate(ev));
        }
    }

    #region Time units
    /// <summary>
    /// Decide quanto dura una unità di tempo.
    /// </summary>
    public float unitDuration = 1f;
    /// <summary>
    /// Contatore delle unità di tempo.
    /// </summary>
    int unitsEnded = 0;

    float currentTime = 0;
    
    private void Update() {
        if (Events == null)
            return;
        currentTime += Time.deltaTime;
        if (currentTime > unitDuration) {
            currentTime = 0;
            unitsEnded++;
            UnitEnd();
        }

        IncreaseMounthCounter();
        FillTheMounthImage();
    }

    /// <summary>
    /// Chiamata quando scade una unità di tempo. Controllo se è necessario invocare un timed event.
    /// </summary>
    void UnitEnd() {
        foreach (var ev in Events) {
            if (ev.TimeUnitEnded()) {
                // Scateno l'evento
                if (OnEvent != null)
                    OnEvent(ev);
            }

        }

    }

    #endregion

    ///appena viene aggiunto deve succedere qualcosa.
}
