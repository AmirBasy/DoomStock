using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableMenuItem : MonoBehaviour {

    public Text Lable;
    public ISelectable SelectionData;

    public void SetData(ISelectable data) {
        Lable.text = "|| " + data.UniqueID + " ||"; 
    }

    public void Select(bool _isSelected) {
        if (_isSelected)
            Lable.color = Color.red;
        else
            Lable.color = Color.black;

    }
    
}
