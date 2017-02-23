using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEvent {
    /// <summary>
    /// Identifica il tipo di evento.
    /// </summary>
    public string ID;

    /// <summary>
    /// Segnala quando deve partire un evento.
    /// </summary>
    public float TimeToInvoke;

    /// <summary>
    /// Se è true si ripete.
    /// </summary>
    public bool isRepeating;
}
