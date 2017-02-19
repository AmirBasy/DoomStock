using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFour : PlayerBase {

    public override void UsePopulation()
    {
        base.UsePopulation();
        //Con keypad + aggiungo a me 1 di popolazione e lo tolgo al GameManager
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (GameManager.I.Population > 0)
            {
                GameManager.I.Population -= 1;
                population += 1;
                UpdateGraphic("people: " + population + " press + to add, - to remove"); 
            }
        }
        //Con keypad - tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            population -= 1;
            GameManager.I.Population += 1;
            if (population <= 0)
                population = 0;
            UpdateGraphic("people: " + population + " press + to add, - to remove");
        }
    }
    void Update()
    {
        UsePopulation();
    }
}
