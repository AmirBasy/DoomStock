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
            if (GameManager.I.Population > 0)
            {
                GameManager.I.Population -= 1;
                population += 1;
                UpdateGraphic("people: " + population + " press U to add, O to remove"); 
            }
        }
        //Con O tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.O))
        {
            population -= 1;
            GameManager.I.Population += 1;
            if (population <= 0)
                population = 0;
            UpdateGraphic("people: " + population + " press U to add, O to remove");
        }
    }
    public override void DeployBuilding()
    {
        base.DeployBuilding();
        if (Input.GetKeyDown(KeyCode.N) && ActualPlayer == BuildingType.Player2)
        {
            Instantiate(MyBuilding[0], transform.position, transform.rotation);
        }
    }

    public override void AddPeopleOnBuilding()
    {
        base.AddPeopleOnBuilding();
        if (Input.GetKeyDown(KeyCode.M) && population > 0)
        {
            //Aggiungo la popolazione all'edificio

        }
    }
        void Update()
    {
        UsePopulation();
        DeployBuilding();
    }
}
