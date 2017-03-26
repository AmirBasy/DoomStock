using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{

    public Text MainPeopleText;

    public int StandardNatality;
    public List<PopulationData> AllPopulation = new List<PopulationData>();
    /// <summary>
    /// scegliere nomi tra questi.
    /// </summary>
    public List<String> Names;
    /// <summary>
    /// Scegliere età massima di ciascun popolano tra MinLife e MaxLife.
    /// </summary>
    public int MinLife, MaxLife;
 

    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    private int mainPopulation;

    public int MainPopulation
    {
        get { return mainPopulation; }
        set
        {
            mainPopulation = value;

            if (MainPopulation > mainPopulation)
                MainPopulation = mainPopulation;
            if (MainPopulation <= 0)
                MainPopulation = 0;
            UpdateGraphic("Main People: " + MainPopulation);

        }
    }


    void Awake()
    {
       
        DontDestroyOnLoad(this.gameObject);

    }

    // Use this for initialization
    void Start()
    {
        mainPopulation = 0;
        UpdateGraphic("Main People: " + MainPopulation);

    }

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
                mainPopulation += building.Data.IncreaseMaxPopulation;
            }
        }
    }
   

    #region events


    public List<TimedEventData> TimedEvents;

    private void OnEnable()
    {
        TimeEventManager.OnEvent += OnEvent;
    }

    private void OnEvent(TimedEventData _eventData)
    {
        foreach (TimedEventData ev in TimedEvents)
        {
            if (ev.ID == "Birth")
            {
                MainPopulation++;
                PopulationData newUnit = CreatePopulation();
                AllFreePeople.Add(newUnit);
                AllPopulation.Add(newUnit);
            }
            if (ev.ID == "FineMese")
            {
                foreach (PopulationData p_data in AllPopulation)
                {
                    p_data.Month++;
                   
                    if(p_data.Month >= 12)
                    {
                        p_data.MaxAge--;
                        if (p_data.MaxAge <= 0) {
                            AllFreePeople.Remove(p_data);
                            AllPopulation.Remove(p_data);
                        }
                            

                        p_data.Month = 0;
                    }
                        
                }
            }
        }
    }
    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnEvent;
    }

    #endregion
    public List<PopulationData> AllFreePeople = new List<PopulationData>();

    /// <summary>
    /// genera un'unità di populationData con nome  e età massima random.
    /// </summary>
    /// <returns></returns>
    PopulationData CreatePopulation()
    {
        int randomIndex = UnityEngine.Random.Range(0, Names.Count);

        PopulationData unitToInstantiate = new PopulationData
        {
            MaxAge = UnityEngine.Random.Range(MinLife, MaxLife),
            Name = Names[randomIndex],
        };
        unitToInstantiate.Awake();
        return unitToInstantiate;
    }
}
