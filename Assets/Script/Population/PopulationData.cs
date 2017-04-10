using UnityEngine;
using System.Collections.Generic;



public class PopulationData : ISelectable
{
    public string UniqueID { get; set; }
    public string NameLable { get; set; }

    public string Name, Ambition;
    public bool IndividualHappiness;
    public int Age, MaxAge, FoodRequirements, EatingTime, Month;

    public void Awake() {
        UniqueID = Name + GameManager.I.populationManager.GetUniqueId() ;
        NameLable = Name;
    }
    
}
