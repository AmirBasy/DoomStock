using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        if (Input.GetKeyDown(KeyCode.E) && population > 0)
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

    public void MoveToGridPosition()
    { 

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1),
            0.7f).OnComplete(delegate
            {
                Debug.Log("Movimento W");
            }).SetEase(Ease.OutBounce);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.DOMove(new Vector3(transform.position.x-1, transform.position.y, transform.position.z),
                0.7f).OnComplete(delegate {
                    Debug.Log("Movimento A");
                }).SetEase(Ease.OutBounce);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                0.7f).OnComplete(delegate {
                    Debug.Log("Movimento S");
                }).SetEase(Ease.OutBounce);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.DOMove(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z),
                0.7f).OnComplete(delegate {
                    Debug.Log("Movimento D");
                }).SetEase(Ease.OutBounce);
        }
    }
    void Update()
    {
        UsePopulation();
        DeployBuilding();
        MoveToGridPosition();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GridController>() != null)
        {
            MoveToGridPosition();
           
        }
    }

}
