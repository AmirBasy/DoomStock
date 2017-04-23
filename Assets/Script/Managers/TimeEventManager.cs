
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeEventManager : MonoBehaviour
{

    #region Events
    public delegate void GameTimedEvent(TimedEventData _eventData);

    public static GameTimedEvent OnEvent;
    #endregion

    public List<TimedEventData> EventsPrefab = new List<TimedEventData>();
    [HideInInspector]
    protected List<TimedEventData> Events = new List<TimedEventData>();

    void Start()
    {
        foreach (TimedEventData ev in EventsPrefab)
        {
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

    private void Update()
    {
        if (Events == null)
            return;
        currentTime += Time.deltaTime;
        if (currentTime > unitDuration)
        {
            currentTime = 0;
            unitsEnded++;
            UnitEnd();
        }
    }

    /// <summary>
    /// Chiamata quando scade una unità di tempo. Controllo se è necessario invocare un timed event.
    /// </summary>
    void UnitEnd()
    {
        foreach (var ev in Events)
        {
            if (ev.TimeUnitEnded())
            {
                // Scateno l'evento
                if (OnEvent != null)
                    OnEvent(ev);
            }

        }

    }

    #endregion

    ///appena viene aggiunto deve succedere qualcosa.
}
