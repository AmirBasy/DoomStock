using UnityEngine;
using System;


[CreateAssetMenu(fileName = "PopulationData",
                 menuName = "Population/PopulationData", order = 2)]
public class PopulationData : ScriptableObject
{

    public String Name, TypeOfWork;

    public int LifeDuration, HealthCare, Food, Happiness ;

    
}
