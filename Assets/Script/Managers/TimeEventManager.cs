
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEventManager : MonoBehaviour {

    public delegate void GameTimedEvent(string _eventName);

    public static GameTimedEvent OnEvent;
    public List<TimedEventData> Events;

    private void Start() {
        Events = new List<TimedEventData>();
        Events.Add(new TimedEventData() {
            ID = "Apocalisse",
            isRepeating = false,
            TimeUnitsToInvoke = 10000});
        Events.Add(new TimedEventData() {
            ID = "FineAnno",
            isRepeating = true,
            TimeUnitsToInvoke = 365
        });
    }

    #region Time units

    public float unitDuration = 1;
    float currentTime = 0;
    int unitsEnded = 0;
    private void Update() {
        currentTime += Time.deltaTime;
        if (currentTime > unitDuration) {
            currentTime = 0;
            unitsEnded++;
            UnitEnd();
        }
    }

    void UnitEnd() {
        if (OnEvent != null)
            OnEvent("ssss");
    }

    #endregion
    ///appena viene aggiunto deve succedere qualcosa.
}
