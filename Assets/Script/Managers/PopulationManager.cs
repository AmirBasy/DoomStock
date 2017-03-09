using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour {

    public Text MainPeopleText;
    public PopulationView PV;
    public int StandardNatality;
    /// <summary>
    /// Popolazione in comune tra i player
    /// </summary>
    private int maxPopulation;

    public int MaxPopulation
    {
        get { return maxPopulation; }
        set
        {
            maxPopulation = value;

            if (MaxPopulation > maxPopulation)
                MaxPopulation = maxPopulation;
            if (MaxPopulation <= 0)
                MaxPopulation = 0;
            UpdateGraphic("Main People: " + MaxPopulation);
            IncreaseMaxPopulation();
        }
    }
    /// <summary>
    /// Aspettativa di vita che viene modifacata in base all'HealthCare
    /// </summary>
    /// <returns></returns>
    //public int LifeExpectancy() {
    //    int PopulationLifeExpectancy;


    //    return PopulationLifeExpectancy;
    //}

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Use this for initialization
    void Start() {
        maxPopulation = 100;
        UpdateGraphic("Main People: " + MaxPopulation);
    }

    private void UpdateGraphic(string _newText)
    {
        if (MainPeopleText)
            MainPeopleText.text = _newText;
    }
    /// <summary>
    /// Aumenta la MaxPopulation
    /// </summary>
    public void IncreaseMaxPopulation(){
        foreach (BuildingView bulding in GameManager.I.buildingManager.GetAllBuildingInScene())
        {
          //  maxPopulation += 10;
        }
    }


}
