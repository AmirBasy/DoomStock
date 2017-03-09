using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BuildingDataInfo", 
                 menuName = "Building/BuildingData", order = 1)]

public class BuildingData : ScriptableObject {
    /// <summary>
    /// la risorsa population che ha è assegnata al
    /// </summary>
    [HideInInspector]
    public int Population;
    /// <summary>
    /// Variabile che indica quanta Popolazione massima posso possedere
    /// </summary>
    public int PopulationLimit;
    /// <summary>
    /// Oggetto prefab dell edificio
    /// </summary> 
    public BuildingView BuildPrefab;
    [HideInInspector]
    public BaseResource Resource = new BaseResource();
    /// <summary>
    /// identifica il tipo di edificio
    /// </summary>
    public String ID;
    
}

