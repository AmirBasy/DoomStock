
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEventManager : MonoBehaviour {

    public delegate void GameTimedEvent();

    public static GameTimedEvent OnEvent;
    public List<TimedEvent> Events;

    private void Start() {
        Events = new List<TimedEvent>();
        Events.Add(new TimedEvent() {
            ID = "Apocalisse",
            isRepeating = false,
            TimeToInvoke = 10000.0f});
        Events.Add(new TimedEvent() {
            ID = "FineAnno",
            isRepeating = true,
            TimeToInvoke = 365.0f
        });
    }

    ///appena viene aggiunto deve succedere qualcosa.
}
