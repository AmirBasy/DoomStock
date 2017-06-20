using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    CellDoomstock data = null;
    //Renderer rend;
    //public Material[] Color;

    public void Init(CellDoomstock _data) {
        data = _data;
        data.OnDataChanged = null;
        data.OnDataChanged += OnDataChanged;
        //rend = GetComponentInChildren<Renderer>();
        //rend.enabled = true;
        //if (data.Status == CellDoomstock.CellStatus.Hole)
        //{
        //    rend.material = Color[0];
        //}
        ////else if (data.Cost > 5) {
        ////    rend.material = Color[1];
        ////}
        //else
        //{
        //    return;
        //}

    }

    void OnDataChanged(CellDoomstock _newData) {
        Init(_newData);
    }

    private void OnDisable()
    {
        if(data !=null)
        data.OnDataChanged -= OnDataChanged;
    }
}

