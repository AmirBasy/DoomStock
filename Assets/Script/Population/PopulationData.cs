using UnityEngine;
using System;


public class PopulationData : ISelectable
{
    public string UniqueID { get; set; }

    public string Name, Ambition;

    public int Age, MaxAge, FoodRequirements, IndividualHappiness;

    public PopulationView PopulationPrefab;

    public void Awake() {
        UniqueID = Name;
    }
}
