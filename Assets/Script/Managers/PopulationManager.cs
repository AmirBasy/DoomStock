using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{

    public Text MainPeopleText;
    public PopulationView PV;
    public int StandardNatality;

    public List<PopulationData> PopulationDataPrefabs;

    ///// <summary>
    ///// Lista di dell'Oggetto PopulationView in Scene
    ///// </summary>
    ///// <returns></returns>
    //public List<PopulationView> PopulationListInScene()
    //{
    //    List<PopulationView> newPopList = new List<PopulationView>();
    //    foreach (PopulationData pd in GetAllPopulation())
    //    {
    //        newPopList.Add(pd.PopulationPrefab);
    //    }
    //    Debug.Log("PopulationList e' di " + newPopList.Count);
    //    return newPopList;
    //}

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
        GetAllPopulation();
        DontDestroyOnLoad(this.gameObject);

    }

    // Use this for initialization
    void Start()
    {
        mainPopulation = 100;
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

    /// <summary>
    /// Lista che carica tutti i PopulationData dalla cartella risorse
    /// </summary>
    public List<PopulationData> GetAllPopulation()
    {
        PopulationData[] allPopulationfromResources = Resources.LoadAll<PopulationData>("Population");
        List<PopulationData> newPopulationList = new List<PopulationData>();
        foreach (PopulationData populationPointer in allPopulationfromResources)
        {
            newPopulationList.Add(Instantiate(populationPointer));
            #region Costruttore
            //{

            //    Age = populationPointer.Age,
            //    Ambition = populationPointer.Ambition,
            //    FoodRequirements = populationPointer.FoodRequirements,
            //    IndividualHappiness = populationPointer.IndividualHappiness,
            //    Name = populationPointer.Name,
            //    StandardLifeExpectation = populationPointer.StandardLifeExpectation,
            //    PopulationPrefab = populationPointer.PopulationPrefab,
            //} 
            #endregion

        }
        return newPopulationList;
    }

    /// <summary>
    /// Crea una nuova istanza di un population data e ne restituisce la view
    /// </summary>
    /// <returns></returns>
    public void CreatePopulation(PopulationData _populationData)
    {
        PopulationData NewIstancePopulationData = Instantiate(_populationData);
        PopulationView NewIstanceView = Instantiate(NewIstancePopulationData.PopulationPrefab);
        NewIstanceView.Init(NewIstancePopulationData);
    }


    /// <summary>
    /// Sceglie un populationData a caso.
    /// </summary>
    /// <returns></returns>
    public PopulationData GetPopulationRandom()
    {
        for (int i = 0; i < mainPopulation; i++)
        {
            int RandomInd = UnityEngine.Random.Range(0, GetAllPopulation().Count);
            PopulationData newPopulation = GetAllPopulation()[RandomInd];
            return newPopulation;
        }
        return null;
    }

    public List<PopulationData> GetAllFreePeople() {
        List<PopulationData> returnList = new List<PopulationData>() {
            new PopulationData() { Name = "Gino", UniqueID = "Gino" },
            new PopulationData() { Name = "Pino", UniqueID = "Pino"  },
            new PopulationData() { Name = "Tino", UniqueID = "Tino"  },
        };
        return returnList;
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
                CreatePopulation(GetPopulationRandom());
            }
        }
    }
    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnEvent;
    }

    #endregion

}
