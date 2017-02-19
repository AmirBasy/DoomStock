using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour {

   public Text PeopleText;
   public int population = 0;

	public virtual void UsePopulation()
    {
        
        
    }
	public virtual void UpdateGraphic(string newText)
    {
        PeopleText.text = newText;
    }
}
