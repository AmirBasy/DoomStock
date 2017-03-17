using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuilding : MonoBehaviour {

    public Text NameBuilding;
    public Text DescriptionBuilding;
    
    /// <summary>
    /// The RectTransform attached to this GameObject
    /// </summary>
    RectTransform rectTransform;
	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // La somma totale delle altezze delle caselle contenute all'interno del menu
            float TotalHeight = NameBuilding.rectTransform.sizeDelta.y + DescriptionBuilding.rectTransform.sizeDelta.y;
            
            /// Controllo se l'altezza del menu è minore della somma delle altezze delle caselle di testo contenute al suo interno
            
            if (rectTransform.sizeDelta.y < TotalHeight)
                /// ingrandisco il menu.
                Debug.Log("Ingrandisco il menu");
        }
	}
}
