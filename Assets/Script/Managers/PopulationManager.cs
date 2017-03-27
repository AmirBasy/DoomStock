using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{

    #region Variables
    public Text MainPeopleText;

    public int HealthCare;

    /// <summary>
    /// scegliere nomi tra questi.
    /// </summary>
    public List<String> Names;

    /// <summary>
    /// Scegliere età massima di ciascun popolano tra MinLife e MaxLife.
    /// </summary>
    public int MinLife, MaxLife;

    /// <summary>
    /// Scegliere fabbisogno di ciascun popolano tra MinFoodRequirement e MaxFoodRequirement.
    /// </summary>
    public int MinFoodRequirement, MaxFoodRequirement;

    /// <summary>
    /// Scegliere ogni quanto mangiare di ciascun popolano tra MinEatingTime e MaxEatingTime.
    /// </summary>
    public int MinEatingTime, MaxEatingTime;

    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    //private int mainPopulation;
    //public int MainPopulation()
    //{

    //    UpdateGraphic("Main People: " + AllFreePeople.Count);

    //    return AllFreePeople.Count;
    //}
    #endregion

    #region Lists
    /// <summary>
    /// Lista di tutta la popolazione in scena.
    /// </summary>
    public List<PopulationData> AllPopulation = new List<PopulationData>();

    /// <summary>
    /// Lista della popolazione non assegnata.
    /// </summary>
    public List<PopulationData> AllFreePeople = new List<PopulationData>();
    #endregion

    #region Init
    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {

        UpdateGraphic("Main People: " + AllFreePeople.Count);


    }
    #endregion

    #region Functions

    /// <summary>
    /// aggiorna il testo MainPeople.
    /// </summary>
    /// <param name="_newText"></param>
    private void UpdateGraphic(string _newText)
    {
        if (MainPeopleText)
            MainPeopleText.text = _newText;
    }

    /// <summary>
    /// Aumenta la MaxPopulation per ogni edificio istanziato
    /// </summary>
    public void IncreaseMaxPopulation()
    {
        foreach (BuildingView building in GameManager.I.buildingManager.GetAllBuildingInScene())
        {
            if (building.Data.IncreaseMaxPopulation > 0)
            {
                // MainPopulation += building.Data.IncreaseMaxPopulation;
            }
        }
    }

    /// <summary>
    /// genera un'unità di populationData con nome, età massima e fabbisogno random 
    /// </summary>
    /// <returns></returns>
    PopulationData CreatePopulation()
    {
        int randomIndex = UnityEngine.Random.Range(0, Names.Count);

        PopulationData unitToInstantiate = new PopulationData
        {
            MaxAge = UnityEngine.Random.Range(MinLife, MaxLife),
            Name = Names[randomIndex],
            FoodRequirements = UnityEngine.Random.Range(MinFoodRequirement, MaxFoodRequirement),
            EatingTime = UnityEngine.Random.Range(MinEatingTime, MaxEatingTime),
        };
        unitToInstantiate.Awake();
        return unitToInstantiate;
    }
    #endregion

    #region Events


    public List<TimedEventData> TimedEvents;

    private void OnEnable()
    {
        TimeEventManager.OnEvent += OnEvent;
    }

    private void OnEvent(TimedEventData _eventData)
    {
        foreach (TimedEventData ev in TimedEvents)
        {
            #region Birth
            if (ev.ID == "Birth")
            {
                PopulationData newUnit = CreatePopulation();
                AllFreePeople.Add(newUnit);
                AllPopulation.Add(newUnit);
            }
            #endregion

            #region FineMese
            if (ev.ID == "FineMese")
            {
                foreach (PopulationData p_data in AllPopulation)
                {
                    p_data.Month++;

                    if (p_data.Month >= 12)
                    {
                        p_data.MaxAge--;
                        if (p_data.MaxAge <= 0)
                        {
                            AllFreePeople.Remove(p_data);
                            AllPopulation.Remove(p_data);
                        }


                        p_data.Month = 0;
                    }

                }
            }
            #endregion

            #region Food
            if (ev.ID == "Eat")
            {
                #region foreach
                //foreach (PopulationData _pdata in AllPopulation)
                //{
                //    _pdata.EatingTime--;
                //    if (_pdata.EatingTime <= 0)
                //    {
                //        Debug.Log("devo mangiare. " + _pdata.Name);
                //        GameManager.I.Food--;
                //        if (GameManager.I.Food <= 0)
                //        {
                //            GameManager.I.Food = 0;
                //            AllFreePeople.Remove(_pdata);           
                //            AllPopulation.Remove(_pdata);
                //        }
                //    }

                //} 
                #endregion

                for (int i = 0; i < AllPopulation.Count; i++)
                {
                    int eatingTime = AllPopulation[i].EatingTime;
                    AllPopulation[i].EatingTime--;
                    if (AllPopulation[i].EatingTime <= 0)
                    {
                        AllPopulation[i].EatingTime = 0;
                        Debug.Log("devo mangiare. " + AllPopulation[i].Name);
                        GameManager.I.Food--;
                        if (AllPopulation[i].EatingTime <= 0)
                        {
                            AllPopulation[i].EatingTime = eatingTime;
                        }

                        if (GameManager.I.Food <= 0)
                        {
                            GameManager.I.Food = 0;

                            AllFreePeople.Remove(AllPopulation[i]);
                            AllPopulation.Remove(AllPopulation[i]);
                        }
                    }
                }
                #endregion
            }
        }
    }

    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnEvent;
    }

    private void Update()
    {
        UpdateGraphic("Main People: " + AllFreePeople.Count);
    }
    #endregion
}
