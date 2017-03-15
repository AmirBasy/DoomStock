using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceName",
                 menuName = "Resource/ResourceData", order = 1)]
public class BaseResourceData : ScriptableObject{

    /// <summary>
    /// identificativo del tipo di risorsa.
    /// </summary>
    public string ID;

    /// <summary>
    /// Valore della risorsa.
    /// </summary>
    public int Value;

    /// <summary>
    /// limite massimo della risorsa.
    /// </summary>
    public int Limit;

    /// <summary>
    /// se è vera, Value può andare sotto 0.
    /// </summary>
    public bool HasNegativeValue;

    /// <summary>
    /// Funzione che modifica il valore della risorsa.
    /// </summary>
    /// <param name="valueToModify"></param>
    public void ModifyResource(int valueToModify) {
        Value += valueToModify;
        if (!HasNegativeValue && Value < 0)
            Value = 0;
    }
}
