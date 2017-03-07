
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEventManager : MonoBehaviour {

    public delegate void GameTimedEvent();

    public static GameTimedEvent OnEvent;
    public List<TimedEventData> Events;

    private void Start() {
        Events = new List<TimedEventData>();
        Events.Add(new TimedEventData() {
            ID = "Apocalisse",
            isRepeating = false,
            TimeToInvoke = 10000.0f});
        Events.Add(new TimedEventData() {
            ID = "FineAnno",
            isRepeating = true,
            TimeToInvoke = 365.0f
        });
    }

    ///appena viene aggiunto deve succedere qualcosa.
}
