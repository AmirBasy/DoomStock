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


        //prima di togliere il drag and drop dovrebbe controllare di essere stato posizionato giusto
        //Destroy(t);
    }


    /// <summary>
    /// Cambia la posizione dell'oggetto
    /// </summary>
    /// <param name="newPosition"></param>
    public void SetPosition(Transform newPosition)
    {
        transform.SetParent(newPosition);
        //transform.position = newPosition.position;
        transform.position = new Vector3(newPosition.position.x, newPosition.position.y+1, newPosition.position.z+0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //se la torretta collide con uno slot di tipo terreno si posiziona nella sua pos e diventa suo figlio
        if (other.GetComponent<Ground>() != null)
        {
            SetPosition(other.transform);

        }
       
    }

}
