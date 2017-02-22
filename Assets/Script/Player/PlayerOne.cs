using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOne : PlayerBase {

    public override void UsePopulation()
    {
        base.UsePopulation();
        //Con Q aggiungo a me 1 di popolazione e lo tolgo al GameManager
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameManager.I.Population >0)
            {
                GameManager.I.Population -= 1;
                population += 1;
                UpdateGraphic("people: " + population + " press Q to add, E to remove"); 
            }
        }
        //Con E tolgo 1 dalla mia popolazione
        if (Input.GetKeyDown(KeyCode.E))
        {
            population -= 1;
            GameManager.I.Population += 1;
            if (population <= 0)
                population = 0;
            UpdateGraphic("people: " + population + " press Q to add, E to remove");
        }
    }
   

    public override void DeployBuilding()
    {
        base.DeployBuilding();
        if (Input.GetKeyDown(KeyCode.Z)&& ActualPlayer == BuildingType.Player1)
        {
            Instantiate(MyBuilding[0],transform.position, transform.rotation);
        }
    }

    public override void AddPeopleOnBuilding()
    {
        base.AddPeopleOnBuilding();
        if (Input.GetKeyDown(KeyCode.X) && population >0)
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
