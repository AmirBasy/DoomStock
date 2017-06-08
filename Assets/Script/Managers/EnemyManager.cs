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
            RandomSpawnPosition();
            for (int i = 0; i < TankToSpawn; i++)
            {
               Enemy tank = Instantiate(TankPrefab,GameManager.I.gridController.GetCellWorldPosition((int)StartPosition.GetGridPosition().x, (int)StartPosition.GetGridPosition().y), TankPrefab.transform.rotation);
                
                tank.Init(StartPosition);
                
            }
            //for (int i = 0; i < CombattentiToSpawn; i++)
            //{
            //    Instantiate(CombattentiPrefab, StartPosition, CombattentiPrefab.transform.rotation).Init(GameManager.I.gridController.Cells[(int)StartPosition.x, (int)StartPosition.y]);
            //}
            
        }
    }

    public void RandomSpawnPosition()
    {
        StartPosition = GameManager.I.gridController.Cells[3, 3];

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
