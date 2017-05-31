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
    /// Range di attacco della torretta
    /// </summary>
    public Vector2 AttackRange;

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
    /// Target Enemy
    /// </summary>
    public Enemy EnemyTarget;

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
    //public int Maintenance;

    /// <summary>
    /// Aumenta il LimiteMassimo della Popolazione
    /// </summary>
    public int IncreaseMaxPopulation;

    /// <summary>
    /// Vita dell' edificio
    /// </summary>
    public int BuildingLife;

    public int InitialLife;
    /// <summary>
    /// Variabile utilizzata per il Degrado
    /// </summary>
    public int DecreaseBuildingLife;


    public CellDoomstock Cell {
            get { return GameManager.I.gridController.GetCellFromBuilding(this); }
        
    }
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
            //GameManager.I.messagesManager.ShowMessage(item, PopulationMessageType.BackToHole);
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
    /// True se l'edificio è in una condizione attaccabile dai nemici.
    /// </summary>
    /// <returns></returns>
    public bool CanBeAttacked() {
        switch (currentState) {
            case BuildingState.Destroyed:
                return false;
            default:
                return true;
        }
    }

    #endregion

    public BuildingState currentState = BuildingState.Construction;
   

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
            case "Fattoria":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Food"));
                break;
            case "Cava":
                BuildingResources.Add(GameManager.I.GetNewInstanceOfResourceData("Stone"));
                break;
            case "Foresta":
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
#region Stati edificio
/// <summary>
/// Stati dell'edificio.
/// </summary>
public enum BuildingState
{
    Construction = 0,
    Built = 1,
    Producing = 2,
    Ready = 3, 
    Destroyed = 4,
}
#endregion