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
            if (GameManager.I.Population > 0)
            {
                GameManager.I.Population -= 1;
                population += 1;
                UpdateGraphic("people: " + population + " press pageUP to add, pageDown to remove"); 
            }
        }
        //Con Freccia giù tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.PageDown) && population > 0)
        {
            population -= 1;
            GameManager.I.Population += 1;
            if (population <= 0)
                population = 0;
            UpdateGraphic("people: " + population + " press pageUP to add, pageDown to remove");
        }
    }
    void Update()
    {
        UsePopulation();
    }
}
