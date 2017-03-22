using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableButton : Button {

    public ISelectable SelectionData;


    public void SetData(ISelectable data) {
        SelectionData = data;
        GetComponentInChildren<Text>().text = "- " + SelectionData.UniqueID;
        
    }
}
