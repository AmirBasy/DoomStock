using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwo : PlayerBase {

    public override void UsePopulation()
    {
        base.UsePopulation();
        //Con U aggiungo a me 1 di popolazione e lo tolgo al GameManager
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.I.Population -= 1;
            population += 1;
        }
        //Con O tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.O))
        {
            population -= 1;
        }
    }
    void Update()
    {
        UsePopulation();
    }
}
