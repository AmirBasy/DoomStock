
using UnityEngine;
using System.Collections;
using System;

public class DragAndDrop : MonoBehaviour
{
    #region Events
    public delegate void TurretEvent(DragAndDrop t);
    public static TurretEvent OnDropTurret; 
    #endregion

    Vector3 dist;
    float posX;
    float posY;


    void OnMouseDown()
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
    }

    void OnMouseDrag()
    {
        Vector3 currentPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(currentPos);
        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        if (OnDropTurret != null)
        {
            OnDropTurret(this);
        }
    }
}
