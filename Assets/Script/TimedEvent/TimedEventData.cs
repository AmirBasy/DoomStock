using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimedEventData",
                 menuName = "TimedEvent/TimedEventData", order = 3)]

public class TimedEventData : ScriptableObject
{
    /// <summary>
    /// Identifica il tipo di evento.
    /// </summary>
    public string ID;

    /// <summary>
    /// Segnala quando deve partire un evento.
    /// </summary>
    public int TimeUnitsToInvoke;

    /// <summary>
    /// Contatore delle unità di tempo runtime per questo evento.
    /// </summary>
    [HideInInspector]
    public int CurrentTimeUnit;

    /// <summary>
    /// Se è true si ripete.
    /// </summary>
    public bool isRepeating;

    public TimedEventData() {
        CurrentTimeUnit = TimeUnitsToInvoke;
    }
}
