using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public List<Enemy> enemyPrefab;
    public int TankToSpawn, CombattentiToSpawn;

    public CellDoomstock StartPosition;



    void OnUnitEvent(TimedEventData _eventData)
    {
        StartPosition = GameManager.I.gridController.Cells[3,3];

        if (_eventData.ID == "EnemySpawn")
        {
            //Instantiate(enemyPrefab,);
        }
    }
}
