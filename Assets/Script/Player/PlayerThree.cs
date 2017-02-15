using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThree : PlayerBase {

    public override void UsePopulation()
    {
        base.UsePopulation();
        //Con Freccia su aggiungo a me 1 di popolazione e lo tolgo al GameManager
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            GameManager.I.Population -= 1;
            population += 1;
        }
        //Con Freccia giù tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            population -= 1;
        }
    }
    void Update()
    {
        UsePopulation();
    }
}
