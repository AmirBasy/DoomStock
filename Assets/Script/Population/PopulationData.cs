using UnityEngine;
using System.Collections.Generic;
using System;

public class PopulationData : ISelectable
{
    public string UniqueID { get; set; }
    public string NameLable { get; set; }

    public Sprite IconToGet { get; set; }

    public BuildingData building;
    public string Name, Ambition;
   
    public int Age, MaxAge, FoodRequirements, EatingTime, Month;

    public void Awake() {
        UniqueID = Name + GameManager.I.populationManager.GetUniqueId() ;
        NameLable = Name + ": " + Ambition;
    }
    
}
