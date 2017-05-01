using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "BuildingDataInfo",
                 menuName = "Building/BuildingData", order = 1)]

public class BuildingData : ScriptableObject, ISelectable
{
    #region Liste


    /// <summary>
    /// Lista degli eventi a cui questo edificio risponde.
    /// </summary>
    public List<TimedEventData> TimedEvents;

    /// <summary>
    /// Lista di Risorse che l'edifico puo creare.
    /// </summary>
    public List<BaseResourceData> BuildingResources;

    /// <summary>
    /// la risorsa population che ha è assegnata al
    /// </summary>
    public List<PopulationData> Population; 
    #endregion

    #region Proprietà

    /// <summary>
    /// player che possiede l'edificio
    /// </summary>
    private Player _playerOwner;
    public Player PlayerOwner
    {
        get { return _playerOwner; }
        set { _playerOwner = value; }
    }

    /// <summary>
    /// ID unico
    /// </summary>
    public string UniqueID { get; set; }

    /// <summary>
    /// nome dell'edicio
    /// </summary>
    public string NameLable { get; set; }

    /// <summary>
    /// l'ambizione che un popolano deve avere per lavorare in questo edificio producendo Happiness.
    /// </summary>
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

    #region API

    /// <summary>
    /// Restituisce la posizione sulla griglia.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetGridPosition()
    {

        return GameManager.I.gridController.GetBuildingPositionByUniqueID(UniqueID);
    }

    /// <summary>
    /// rimuove  un'unità di popolazione dall'edificio
    /// </summary>
    public void RemoveUnitOfPopulationFromBuilding(string _unitToRemoveID)
    {
        foreach (var item in Population.FindAll(p => p.UniqueID == _unitToRemoveID))
        {
            GameManager.I.populationManager.AddPopulation(item);
            GameManager.I.messagesManager.ShowMessage(item, PopulationMessageType.BackToHole);
        }

        Population.RemoveAll(p => p.UniqueID == _unitToRemoveID);

    }

    /// <summary>
    /// toglie tutta la popolazione dall'edificio e la rimette nella pozza
    /// </summary>
    public void RemoveAllPopulationFromBuilding()
    {
        for (int i = 0; i < Population.Count; i++)
        {
            RemoveUnitOfPopulationFromBuilding(Population[i].UniqueID);
        }
    }

    /// <summary>
    /// Ritorna True se l'edifico sta producendo
    /// </summary>
    /// <returns></returns>
    public bool IsBuildingProducing()
    {
        if (Population.Count > 0)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Stati edificio
    /// <summary>
    /// Stati dell'edificio.
    /// </summary>
    public enum BuildingState
    {
        Construction = 0,
        Built = 1,
        Debris = 2,
        Producing = 3,
        Ready = 4
    }

    public BuildingState currentState = BuildingState.Construction;
    #endregion

    #region Setup
    public void Awake()
    {
        if (!GameManager.I)
            return;
        UniqueID = ID + GameManager.I.buildingManager.GetUniqueId();
        NameLable = ID + " (" + UniqueID + ")";

       
    } 

    public void Init()
    {
        switch (ID)
        {
            case "Farm":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Food"));
                break;
            case "EstrattoreStone":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Stone"));
                break;
            case "EstrattoreWood":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Wood"));
                break;
            case "Chiesa":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Faith"));
                break;
            case "Torretta":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Spirit"));
                break;
            default:
                break;
        }
    }
    #endregion
}

