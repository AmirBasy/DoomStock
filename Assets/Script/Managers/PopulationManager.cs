using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour {

    public Text MainPeopleText;
    public PopulationView PV;


    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    private int maxPopulation = 100;

    public int MaxPopulation
    {
        get { return maxPopulation; }
        set
        {
            maxPopulation = value;

            if (MaxPopulation > 99)
                MaxPopulation = 100;
            if (MaxPopulation <= 0)
                MaxPopulation = 0;
            UpdateGraphic("Main People: " + MaxPopulation);
        }
    }

    /// <summary>
    /// Prende tutti La Population che stanno nella cartella Resources/Population per evitare che i dati vengano salvati.
    /// </summary>
    /// <returns></returns>
    public static List<PopulationData> GetAllPopulation()
    {   
        PopulationData[] allBuildingfromResources = Resources.LoadAll<PopulationData>("Population");
        List<PopulationData> newPopulationList = new List<PopulationData>();
        foreach (PopulationData populationPointer in newPopulationList)
        {
            newPopulationList.Add(new PopulationData
            {
                  Food = populationPointer.Food,
                  Happiness = populationPointer.Happiness,
                  HealthCare = populationPointer.HealthCare,
                  LifeDuration =populationPointer.LifeDuration,
                  Name = populationPointer.Name,
                  TypeOfWork = populationPointer.TypeOfWork
            }
            );
        }
        return newPopulationList;
        }
    #region Risorse
    private int resource1;

    public int Resource1
    {
        get { return resource1; }
        set { resource1 = value; }
    }
    private int resource2;

    public int Resource2
    {
        get { return resource2; }
        set { resource2 = value; }
    }


    #endregion Risorse

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Use this for initialization
    void Start () {
       
        UpdateGraphic("Main People: " + MaxPopulation);
    }

    private void UpdateGraphic(string _newText)
    {
        if (MainPeopleText)
            MainPeopleText.text = _newText;
    }


   
}
