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
    int F_Cost { get; set; }

    /// <summary>
    /// nodo parent da cui calcolare i valori
    /// </summary>
    INode parent { get; set; }

    /// <summary>
    /// se è true il valido per il pathfinding
    /// </summary>
    bool isTraversable { get; set; }

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
