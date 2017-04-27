using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode {
    /// <summary>
    /// costo del  nodo rispetto al punto di partenza
    /// </summary>
    int G_Cost { get; set; }

    /// <summary>
    /// costo del nodo rispetto all'arrivo
    /// </summary>
    int H_Cost { get; set; }

    /// <summary>
    /// somma di H_cost e G_cost
    /// </summary>
    int F_Cost { get; }

    /// <summary>
    /// nodo parent da cui calcolare i valori
    /// </summary>
    INode parent { get; set; }

    /// <summary>
    /// se è true il valido per il pathfinding
    /// </summary>
    bool isTraversable { get; }

    /// <summary>
    /// Posizione nella griglia
    /// </summary>
    /// <returns></returns>
    Vector2 GetGridPosition();

    /// <summary>
    /// Posizione nel mondo
    /// </summary>
    /// <returns></returns>
    Vector3 GetWorldPosition();

    /// <summary>
    /// restituisce tutti i nodi vicini
    /// </summary>
    /// <returns></returns>
    List<INode> GetNeighbours();

    
}

public static class INodeExtension {
    /// <summary>
    /// setta il costo di G e H
    /// </summary>
    /// <param name="_this"></param>
    public static void SetCost(this INode _this, INode _startPath, INode _endPath) {
        _this.G_Cost = _this.CalculateCost(_this, _startPath);
        _this.H_Cost = _this.CalculateCost(_this, _endPath);
    }
    /// <summary>
    /// Calcola la distanza Euristica tra le assi X e Y dei Nodi
    /// </summary>
    /// <param name="_this"></param>
    /// <param name="_currentNode"></param>
    /// <param name="_arrivalNode"></param>
    /// <returns></returns>
    public static int CalculateCost(this INode _this,INode _currentNode,INode _arrivalNode ) {
        int resultDistance;
        int distanceX = Mathf.Abs((int)_currentNode.GetGridPosition().x - (int)_arrivalNode.GetGridPosition().x); 
        int distanceY = Mathf.Abs((int)_currentNode.GetGridPosition().y - (int)_arrivalNode.GetGridPosition().y);
        if (distanceX < distanceY) {
            // asse X corta
             resultDistance = distanceX * 14 + (distanceX - distanceY) * (10);
        } else {
            // asse Y corta
            resultDistance = distanceY * 14 + (distanceY - distanceX) * (10);
        }
        return resultDistance;
    }
}
