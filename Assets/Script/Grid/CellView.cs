using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    CellDoomstock data;
    Renderer rend;
    public Material[] Color;
    CellDoomstock.CellStatus Status;


    public void init(CellDoomstock _cell) {
        data = _cell;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        foreach (CellDoomstock item in GameManager.I.gridController.Cells)
        {
            data = item;
        }
        if (data.Status == CellDoomstock.CellStatus.Hole)
        {
            rend.material = Color[0];
        }
        else
        {
            return;
        }

    }

    private void Start()
    {
        init(data);
    }
}
