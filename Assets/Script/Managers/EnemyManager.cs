using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour {

    public Enemy TankPrefab, CombattentiPrefab;

    public int TankToSpawn, CombattentiToSpawn;

    public CellDoomstock StartPosition;



    void OnUnitEvent(TimedEventData _eventData)
    {

        if (_eventData.ID == "EnemySpawn")
        {
          
            for (int i = 0; i < TankToSpawn; i++)
            {
                CellDoomstock pos = RandomSpawnPosition();
                Enemy tank = Instantiate(TankPrefab,GameManager.I.gridController.GetCellWorldPosition((int)pos.GetGridPosition().x, (int)pos.GetGridPosition().y), TankPrefab.transform.rotation);
                
                tank.Init(pos);
                
            }
            for (int i = 0; i < CombattentiToSpawn; i++)
            {
                CellDoomstock pos = RandomSpawnPosition();
                Enemy tank = Instantiate(TankPrefab, GameManager.I.gridController.GetCellWorldPosition((int)pos.GetGridPosition().x, (int)pos.GetGridPosition().y), TankPrefab.transform.rotation);

                tank.Init(pos);
            }

        }
    }

    public CellDoomstock RandomSpawnPosition()
    {
        int randomX = Random.Range(0, 5);
        int randomY = Random.Range(0, 5);
        CellDoomstock randomPos =  GameManager.I.gridController.Cells[randomX, randomY]; ;
      
        return randomPos;

    }
    private void OnEnable()
    {
        TimeEventManager.OnEvent += OnUnitEvent;
    }
    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnUnitEvent;
    }
}
