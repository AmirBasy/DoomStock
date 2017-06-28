using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{

    #region Variables
    public Text MainPeopleText;


    ///// <summary>
    ///// scegliere nomi tra questi.
    ///// </summary>
    //public List<String> Names;

    //public List<String> Ambitions;
    /// <summary>
    /// Scegliere età massima di ciascun popolano tra MinLife e MaxLife.
    /// </summary>
   // public int MinLife, MaxLife;

    /// <summary>
    /// Scegliere fabbisogno di ciascun popolano tra MinFoodRequirement e MaxFoodRequirement.
    /// </summary>
    public int MinFoodRequirement, MaxFoodRequirement;

  

    /// <summary>
    /// PER DEBUG
    /// </summary>
    public int Startpop = 0;

    private int foodRequirement;

    public int FoodRequirement
    {
        get { return foodRequirement; }
        set
        {
            foodRequirement = value;
            GameManager.I.uiManager.SetFoodTextColor();
        }
    }

    #endregion

    #region Lists
    /// <summary>
    /// Lista di tutta la popolazione in scena.
    /// </summary>
    List<PopulationData> AllPopulation = new List<PopulationData>();

    /// <summary>
    /// Lista della popolazione non assegnata.
    /// </summary>
    // public List<PopulationData> AllFreePeople = new List<PopulationData>();
    List<PopulationData> AllFreePeople = new List<PopulationData>();


    #endregion

    #region Init
    void Awake()
    {

        //DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        UpdateGraphic(" = " + AllFreePeople.Count);
        for (int i = 0; i < Startpop; i++)
        {
            //TODO:queste tre vanno sempre insieme, mettere a posto
            PopulationData newUnit = CreatePopulation();
            AddPopulation(newUnit);
            AllPopulation.Add(newUnit);
        }
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
    /// genera un'unità di populationData
    /// </summary>
    /// <returns></returns>
    PopulationData CreatePopulation()
    {

        PopulationData unitToInstantiate = new PopulationData
        {
            // MaxAge = UnityEngine.Random.Range(MinLife, MaxLife),

            FoodRequirements = UnityEngine.Random.Range(MinFoodRequirement, MaxFoodRequirement),
            EatingTime = 1

        };
        unitToInstantiate.Awake();
        return unitToInstantiate;
    }

    /// <summary>
    /// morte di un popolano
    /// </summary>
    void UnitDeath(PopulationData unit)
    {
       // FoodRequirement -= unit.FoodRequirements;
        //GameManager.I.messagesManager.ShowMessage(unit, PopulationMessageType.Death);
       // GameManager.I.messagesManager.ShowiInformation(MessageLableType.Death, GameManager.I.buildingManager.GetBuildingContainingUnit(unit));
        AllFreePeople.Remove(unit);
        AllPopulation.Remove(unit);
        if (unit.building)
        {
            unit.building.Population.Remove(unit);
            unit.building = null;
        }
       


    }
    #endregion

    #region Events


    public delegate void PopulationEvent();

    public static PopulationEvent OnFreePopulationChanged;

    private void OnEnable()
    {
        TimeEventManager.OnEvent += OnEvent;
    }

    private void OnEvent(TimedEventData _eventData)
    {


        #region Birth
        if (_eventData.ID == "Birth")
        {


            if (GameManager.I.GetResourceDataByID("Food").Value > 0 && GameManager.I.buildingManager.IsThereAnySpace())
            {
                PopulationData newUnit = CreatePopulation();
                AddPopulation(newUnit);
                AllPopulation.Add(newUnit);
                BuildingView firstOpening = GameManager.I.buildingManager.GetFirstOpening();
                firstOpening.Data.Population.Add(newUnit);
               // FoodRequirement += newUnit.FoodRequirements;
                GameManager.I.messagesManager.ShowiInformation(MessageLableType.Birth, firstOpening.Data.Cell);
                //GameManager.I.GetResourceDataByID("Food").Value -= newUnit.FoodRequirements;
            }
        }
        #endregion

        #region FineMese
        if (_eventData.ID == "FineMese")
        {
            for (int i = 0; i < AllPopulation.Count; i++)
            {
                AllPopulation[i].Month++;

                if (AllPopulation[i].Month >= 12)
                {
                    AllPopulation[i].MaxAge--;
                    if (AllPopulation[i].MaxAge <= 0)
                    {
                        AllPopulation[i].Month = 0;
                        //UnitDeath(AllPopulation[i]);
                    }

                }
            }

        }
        #endregion

        #region Food
        if (_eventData.ID == "Eat")
        {

            for (int i = 0; i < AllPopulation.Count; i++)
            {
                int eatingTime = AllPopulation[i].EatingTime;
                AllPopulation[i].EatingTime--;
                if (AllPopulation[i].EatingTime <= 0)
                {
                    AllPopulation[i].EatingTime = 0;

                    GameManager.I.GetResourceDataByID("Food").Value -= AllPopulation[i].FoodRequirements;

                    if (AllPopulation[i].EatingTime <= 0)
                    {
                        AllPopulation[i].EatingTime = eatingTime;
                    }

                    #region morte di fame
                    if (GameManager.I.GetResourceDataByID("Food").Value <= 0)
                    {
                        GameManager.I.GetResourceDataByID("Food").Value = 0;
                        //FoodRequirement -= AllPopulation[i].FoodRequirements;
                        GameManager.I.uiManager.FoodText.color = Color.red;
                        UnitDeath(AllPopulation[i]);


                    }
                    #endregion
                }
            }

        }
    }
         #endregion
    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnEvent;
    }

    private void Update()
    {
        UpdateGraphic(" =   " + AllFreePeople.Count);
    }
    #endregion

    #region Unique ID
    int counter = 0;
    /// <summary>
    /// Genera un id univoco.
    /// </summary>
    /// <returns></returns>
    public int GetUniqueId()
    {
        counter++;
        return counter;
    }
    #endregion

    #region Api

    /// <summary>
    /// aggiunge un'unità di popolazione alla lista di popolani liberi.
    /// </summary>
    /// <param name="unitToAdd"></param>
    public void AddPopulation(PopulationData unitToAdd)
    {
        AllFreePeople.Add(unitToAdd);
        if (OnFreePopulationChanged != null)
            OnFreePopulationChanged();
    }

    /// <summary>
    /// toglie un'unità di popolazione dalla lista di popolani liberi.
    /// </summary>
    /// <param name="unitIDToRemove"></param>
    public PopulationData GetUnit(string unitIDToRemove)
    {
        PopulationData pdata = AllFreePeople.Find(p => p.UniqueID == unitIDToRemove);
        if (!AllFreePeople.Remove(pdata))
            return null;
        if (OnFreePopulationChanged != null)
            OnFreePopulationChanged();

        return pdata;
    }

    /// <summary>
    /// resituisce la lista di tutta la popolazione non assegnata.
    /// </summary>
    /// <returns></returns>
    public List<PopulationData> GetAllFreePeople()
    {
        return AllFreePeople;
    }

    /// <summary>
    /// Restituisce un populationData se gli si passa il suo ID unico.
    /// </summary>
    /// <param name="uniqueID"></param>
    /// <returns></returns>
    public PopulationData GetPopulationDataByID(string uniqueID)
    {
        foreach (PopulationData item in AllPopulation)
        {
            if (item.UniqueID == uniqueID)
                return item;
        }
        return null;
    }

    /// <summary>
    /// restituisce vero se Food è sufficiente per tutti i popolani in scena.
    /// </summary>
    /// <returns></returns>
    public bool IsFoodEnough()
    {
        if (FoodRequirement <= GameManager.I.GetResourceDataByID("Food").Value)
        {
            return true;
        }
        return false;
    }

    #endregion


}
