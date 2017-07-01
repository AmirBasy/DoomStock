using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableMenuItem : MonoBehaviour
{

    public Image IconData;
    public Text Lable;
    public ISelectable SelectionData;

    public void SetData(ISelectable data)
    {

        SelectionData = data;
        if (data.IconToGet == null)
            Lable.text = data.NameLable;
        else
        {
            Lable.text = "";
        }
        if (data.IconToGet)
        {
            IconData.sprite = data.IconToGet;

        }
    }

    public void Select(bool _isSelected)
    {
        if (IconData)
        {

            if (_isSelected == true)
                IconData.color = Color.red;
            else
                IconData.color = Color.white; 
        }
    }
}
