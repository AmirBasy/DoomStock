using UnityEngine;
using System;


[CreateAssetMenu(fileName = "PopulationData",
                 menuName = "Population/PopulationData", order = 2)]
public class PopulationData : ScriptableObject, ISelectable
{
    public string UniqueID { get; set; }

    public string Name, Ambition;

    public int Age,StandardLifeExpectation, FoodRequirements, IndividualHappiness ;

    public PopulationView PopulationPrefab;

    
}
