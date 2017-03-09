using UnityEngine;
using System;


[CreateAssetMenu(fileName = "PopulationData",
                 menuName = "Population/PopulationData", order = 2)]
public class PopulationData : ScriptableObject
{

    public String Name, Ambition;

    public int Age,StandardLifeExpectation, FoodRequirements, IndividualHappiness ;

    
}
