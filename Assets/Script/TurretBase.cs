using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour, IPositionable
{
    
    private void OnEnable()
    {
        DragAndDrop.OnDropTurret += TurretDrop;
    }

    private void TurretDrop(DragAndDrop t)
    {
        //transform.position = new Vector3(0, 0);
        //prima di togliere il drag and drop dovrebbe controllare di essere stato posizionato giusto
         Destroy(t);
    }



    /// <summary>
    /// Cambia la posizione dell'oggetto
    /// </summary>
    /// <param name="newPosition"></param>
    public void SetPosition(Transform newPosition)
    {
        transform.SetParent(newPosition);
        transform.position = newPosition.position;
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Ground>() != null)
        {
            SetPosition(other.transform);
            


        }
       
    }

}
