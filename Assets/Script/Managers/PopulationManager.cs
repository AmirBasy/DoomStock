using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour {

    public Text MainPeopleText;
    public PopulationView PV;
    public int StandardNatality;
   
    public List<PopulationData> PopulationDataPrefabs;
    /// <summary>
    /// Lista di dell'Oggetto PopulationView in Scene
    /// </summary>
    /// <returns></returns>
    public List<PopulationView> PopulationListInScene() {
        List<PopulationView> newPopList = new List<PopulationView>();
        foreach (PopulationData pd in GetAllPopulation())
        {
            newPopList.Add(pd.PopulationPrefab);
        }
        Debug.Log("PopulationList e' di " + newPopList.Count);
        return newPopList;
    }
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
    void Start() {
        mainPopulation = 100;
        //PopulationListInScene();
        //CreatePopulation(PopulationDataPrefabs[0]);
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
    public void IncreaseMaxPopulation(){
        foreach (BuildingView building in GameManager.I.buildingManager.GetAllBuildingInScene())
        {
            if (building.Data.IncraseMaxPopulation>0)
            {
                mainPopulation += building.Data.IncraseMaxPopulation;
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
            newPopulationList.Add(ScriptableObject.CreateInstance<PopulationData>());
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
    /// Crea una PopulationData e restituisce una nuova istanza appena creata
    /// </summary>
    /// <returns></returns>
    public PopulationView CreatePopulation(PopulationData _populationData)
    {
        PopulationData NewIstancePopulationData;
        NewIstancePopulationData = Instantiate(_populationData);

        foreach (PopulationData populationData in GetAllPopulation())
        {
            PopulationView NewIstanceView = Instantiate(NewIstancePopulationData.PopulationPrefab);
            for (int i = 0; i < mainPopulation; i++)
            {
                //PopulationDataPrefabs.Add(populationData);
                Instantiate(NewIstancePopulationData.PopulationPrefab);
                NewIstanceView.Init(NewIstancePopulationData); 
            }
            return NewIstanceView; 
        }
        return null;
        
    }



    //public void GetPopulationRandom()
    //{
    //    for (int i = 0; i < maxPopulation; i++)
    //    {
    //        int RandomInd = Random.Range(0, GetAllPopulation().Count);
    //        PopulationData newPopulation = GetAllPopulation()[RandomInd];

    //    }
    //}
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
            }
        }
    }
    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnEvent;
    }

    #endregion

}
