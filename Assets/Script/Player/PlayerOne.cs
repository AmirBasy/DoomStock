using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOne : PlayerBase {

    public override void UsePopulation()
    {
        base.UsePopulation();
        //Con Q aggiungo a me 1 di popolazione e lo tolgo al GameManager
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.I.Population -= 1;
            population += 1;
        }
        //Con E tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.E))
        {
            population -= 1;
        }
    }
    void Update () {
        UsePopulation();
	}
}
