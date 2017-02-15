using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform TilePrefab;
    public Vector2 MapSize;


	// Use this for initialization
	void Start () {
        GenerateMap();
	}
    /// <summary>
    /// Generates la griglia utilizzando il TilePrefab
    /// </summary>
    public void GenerateMap() {

        Transform mapHolder = new GameObject("Griglia").transform;
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-MapSize.x/2+0.5f+x, 0, -MapSize.y/2+0.5f+y);
                Transform newTile = Instantiate(TilePrefab, tilePosition, Quaternion.Euler(Vector3.right)) as Transform;
                newTile.parent = mapHolder;
            }

        }
    }

}
