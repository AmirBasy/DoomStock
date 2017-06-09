using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour {

    public Enemy TankPrefab, CombattentiPrefab;

    public int TankToSpawn, CombattentiToSpawn;

    public CellDoomstock StartPosition;

    IEnumerator SpawnEnemy(float waitTime)
    {

        
        for (int i = 0; i < TankToSpawn; i++)
        {

            CellDoomstock pos = RandomSpawnPosition();
            Enemy tank = Instantiate(TankPrefab, GameManager.I.gridController.GetCellWorldPosition((int)pos.GetGridPosition().x, (int)pos.GetGridPosition().y), TankPrefab.transform.rotation);

            tank.Init(pos);

            yield return new WaitForSeconds(waitTime);
        }

        for (int i = 0; i < CombattentiToSpawn; i++)
        {

            CellDoomstock pos = RandomSpawnPosition();
            Enemy combattente = Instantiate(CombattentiPrefab, GameManager.I.gridController.GetCellWorldPosition((int)pos.GetGridPosition().x, (int)pos.GetGridPosition().y), TankPrefab.transform.rotation);

            combattente.Init(pos);
            yield return new WaitForSeconds(waitTime);
        }

    }

    void OnUnitEvent(TimedEventData _eventData)
    {

        if (_eventData.ID == "EnemySpawn")
        {

            StartCoroutine(SpawnEnemy(2));

        }
    }

    public CellDoomstock RandomSpawnPosition()
    {
        
      
        CellDoomstock randomPos = new CellDoomstock();
        List<CellDoomstock> nullCells = new List<CellDoomstock>();
        
        
        foreach (var cell in GameManager.I.gridController.Cells)
        {
            if (cell.Type == CellDoomstock.CellType.Nullo)
            {
                nullCells.Add(cell);
            }
            
        }
        int random = Random.Range(0, nullCells.Count);
        randomPos = nullCells[random];
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
