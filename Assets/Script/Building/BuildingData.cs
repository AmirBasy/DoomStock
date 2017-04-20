using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "BuildingDataInfo",
                 menuName = "Building/BuildingData", order = 1)]

public class BuildingData : ScriptableObject, ISelectable {


    [HideInInspector]

    public BuildingState currentState = BuildingState.Construction;
    //public bool IsEnded;
    /// <summary>
    /// Lista degli eventi a cui questo edificio risponde.
    /// </summary>
    public List<TimedEventData> TimedEvents;
    /// <summary>
    /// Lista di Risorse che l'edifico puo creare.
    /// </summary>
    public List<BaseResourceData> BaseResources;
    /// <summary>
    /// la risorsa population che ha è assegnata al
    /// </summary>
    public List<PopulationData> Population;

    #region Proprietà
    private Player _playerOwner;

    public Player PlayerOwner {
        get { return _playerOwner; }
        set { _playerOwner = value; }
    }

    public string UniqueID { get; set; }
    public string NameLable { get; set; }


    public string Ambition;
    /// <summary>
    /// Tempo di costruzione per l'edificio
    /// </summary>
    public int BuildingTime;
    /// <summary>
    /// identifica il tipo di edificio
    /// </summary>
    public String ID;
    /// <summary>
    /// Variabile che indica quanta Popolazione massima posso possedere
    /// </summary>
    public int PopulationLimit;
    /// <summary>
    /// Elenco di risorse necessarie per costruire l'edificio
    /// </summary>
    public int WoodToBuild, StoneToBuild;
    ///// <summary>
    ///// Dichiara se un Building ha termianto il suo tempo di costruzione
    ///// </summary>
    //public bool isBuilt;
    /// <summary>
    /// Oggetto prefab dell edificio
    /// </summary> 
    public BuildingView BuildPrefab;
    /// <summary>
    /// Variabile che indica la potenza di "fuoco" dell'edificio
    /// </summary>
    public int Attack;
    /// <summary>
    /// Variabile che indica ogni quanto l'deificio può attaccare
    /// </summary>
    public float FireRateo;
    /// <summary>
    /// Variabile che indica la resistenza al danno dei nemici
    /// </summary>
    public int DamageResistance;
    /// <summary>
    /// Variabile che indica il costo di Manuntenzione
    /// </summary>
    public int Maintenance;
    /// <summary>
    /// Aumenta il LimiteMassimo della Popolazione
    /// </summary>
    public int IncreaseMaxPopulation;
    /// <summary>
    /// Vita dell' edificio
    /// </summary>
    public int BuildingLife;
    /// <summary>
    /// Variabile utilizzata per il Degrado
    /// </summary>
    public int DecreaseBuildingLife; 
    #endregion

    public void Awake() {
        if (!GameManager.I)
            return;
        UniqueID = ID + GameManager.I.buildingManager.GetUniqueId();
        NameLable = ID + " (" + UniqueID + ")" ;
    }

    #region API
    public Vector2 GetGridPosition() {

        return GameManager.I.gridController.GetBuildingPositionByUniqueID(UniqueID);
    }

    /// <summary>
    /// rimuove  un'unità di popolazione dall'edificio
    /// </summary>
    public void RemoveUnitOfPopulationFromBuilding(string _unitToRemoveID) {
        foreach (var item in Population.FindAll(p => p.UniqueID == _unitToRemoveID)) {
            GameManager.I.populationManager.AddPopulation(item);
            GameManager.I.messagesManager.ShowMessage(item, PopulationMessageType.BackToHole);
        }

        Population.RemoveAll(p => p.UniqueID == _unitToRemoveID);
       
    }
    /// <summary>
    /// toglie tutta la popolazione dall'edificio e la rimette nella pozza
    /// </summary>
    public void RemoveAllPopulationFromBuilding() {
        for (int i = 0; i < Population.Count; i++) {
            RemoveUnitOfPopulationFromBuilding(Population[i].UniqueID);
        }
    } 
    #endregion

    public enum BuildingState {
        Construction = 0,
        Built = 1,
        Debris = 2
    }
}

