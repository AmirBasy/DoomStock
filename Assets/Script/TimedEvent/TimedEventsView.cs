using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventsView : MonoBehaviour {


    public TimedEventData Data;

    
    private void Start()
    {
       

    }
    public void Init(TimedEventData _timedEventData)
    {
        Data = _timedEventData;
        UpdateGraphic();
    }

    public void UpdateGraphic()
    {
        
    }
}
