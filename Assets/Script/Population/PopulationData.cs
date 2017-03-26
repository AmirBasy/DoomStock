using UnityEngine;
using System.Collections.Generic;



public class PopulationData : ISelectable
{
    public string UniqueID { get; set; }

    public string Name, Ambition;

    public int Age, MaxAge, FoodRequirements, IndividualHappiness, Month;

    public void Awake() {
        UniqueID = Name;
    }
}
