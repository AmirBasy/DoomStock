using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public float dimension =0;
    public float timeMultiplier = 0.01f;

    #region Labourers
    private int labourers = 1;

    public int Labourers
    {
        get
        {

            return labourers;
        }
        set
        {
            SetBuildingTime();

            labourers = value;
        }
    }

    #endregion

    /// <summary>
    /// Moltiplica i lavoratori per il multiplier del tempo
    /// </summary>
    void SetBuildingTime()
    {
       
        timeMultiplier *= Labourers;
    }


    private void Update()
    {

        if (Labourers >= 1)
        {
            increaseDimension();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Labourers += 1;
        }
    }

    /// <summary>
    /// fa crescere la dimensione nel tempo
    /// </summary>
    void increaseDimension() { dimension += Time.deltaTime * timeMultiplier; }


}
