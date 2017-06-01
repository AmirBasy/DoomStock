using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Interfaccia per tutte che sono in grado di calcolare un pathfinding.
/// </summary>
public interface IPathFinding {

    GridControllerDoomstock grid { get; }

    List<INode> GetNeighboursStar(INode node);
}

/// <summary>
/// Interfaccia per tutte le classi che sono in grado di eseguire un movimento lungo un path.
/// </summary>
public interface IPathFindingMover : IPathFinding {
    List<INode> CurrentPath { get; set; }
    int CurrentNodeIndex { get; set; }
    void DoMoveStep(INode _step);
}

public static class IPathFindingExtension {

    /// <summary>
    /// trova il percorso dallo startNode al lastNode
    /// </summary>
    /// <param name="_this"></param>
    /// <param name="startNode"></param>
    /// <param name="lastNode"></param>
    public static List<INode> Find_(this IPathFinding _this, INode startNode, INode lastNode, bool ignoreTraversable = false) {
        List<INode> Open = new List<INode>();
        List<INode> Closed = new List<INode>();
        INode current;
        List<INode> path = new List<INode>();
        Open.Add(startNode);

        do {
            //assegna a current l'INode con f cost più basso
            INode lower = null;

            foreach (var item in Open) {
                if (lower == null)
                    lower = item;
                else {
                    if (item.F_Cost < lower.F_Cost)
                        lower = item;
                }
            }
            // mette current nella lista dei già analizzati e lo rimuove dai disponibili
            current = lower;
            Open.Remove(current);
            Closed.Add(current);
            //controllo se il currentNode è uguale al lastNode
            if (current == lastNode) {
                path.Add(lastNode.parent);
                while (path[path.Count - 1] != startNode) {
                    path.Add(path[path.Count - 1].parent);
                }

                return path;
            }
            if (current == null) {
                Debug.Log("Path non trovato");
                return path;
            }
            List<INode> neighboursNodes = current.GetNeighbours();
            foreach (var neighbour in neighboursNodes) {
                // if ((!neighbour.isTraversable && !ignoreTraversable) || Closed.Contains(neighbour)) {
                //if (!neighbour.isTraversable || Closed.Contains(neighbour)) {
                //    continue;
                //}
                if (Closed.Contains(neighbour)) { continue; }
                if (!neighbour.isTraversable && !ignoreTraversable) { continue; }

                //neighbour.SetCost(startNode, lastNode);

                if (!Open.Contains(neighbour)) {
                    neighbour.parent = current;
                    Open.Add(neighbour);
                } else {
                    for (int i = 0; i < Open.Count; i++) {
                        if (Open[i].GetGridPosition() == neighbour.GetGridPosition()) {
                            int newCost = current.G_Cost + current.CalculateCost(current, neighbour);
                            if (newCost < Open[i].G_Cost) {
                                Open[i].parent = current;
                                Open[i].G_Cost = newCost;
                                Open[i].H_Cost = Open[i].CalculateCost(Open[i], lastNode);
                                //Open[i].G_Cost = neighbour.G_Cost;
                                //Open[i].H_Cost = neighbour.H_Cost;
                            }
                        }
                    }

                }

            }
        }
        while (current != lastNode);

        return path;
    }

    public static List<INode> FindPath(this IPathFinding _this, INode startNode, INode targetNode, PathFindingSettings _settings) {

        List<INode> openSet = new List<INode>();
        HashSet<INode> closedSet = new HashSet<INode>();
        openSet.Add(startNode);

        // Ciclo la collezione open fino a quando ha elementi e scelgo quello con il costo minore per avanzare con la ricerca
        while (openSet.Count > 0) {
            INode node = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].F_Cost < node.F_Cost || openSet[i].F_Cost == node.F_Cost) {
                    if (openSet[i].H_Cost < node.H_Cost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);
            // se il nodo attuale è uguale al target, ho raggiunto il target, ho terminato.
            if (node == targetNode) {
                return RetracePath(startNode, targetNode);
            }

            List<INode> nNodes = _this.GetNeighboursStar(node);
            // ---------------------------------
            // --------- Settings Eval ---------
            //if (!_settings.CanJump) {
            //    nNodes = nNodes.Where(n => n.ContainsTag("walkable")).ToList<INode>();
            //}
            // ---------------------------------

            foreach (INode neighbour in nNodes) {
                //if (!neighbour.isTraversable && !_settings.IgnoreObstacles || closedSet.Contains(neighbour)) {
                //    continue;
                //}
                if (closedSet.Contains(neighbour)) {
                    continue;
                }

                int newCostToNeighbour = node.G_Cost + GetDistance(node, neighbour, _settings);
                if (newCostToNeighbour < neighbour.G_Cost || !openSet.Contains(neighbour)) {
                    neighbour.G_Cost = newCostToNeighbour;
                    neighbour.H_Cost = GetDistance(neighbour, targetNode, _settings);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<INode>();
    }

    /// <summary>
    /// Restituisce un path per il percorso indicato da startNode e endNode in modo da renderlo percorribile seguendo l'ordine della lista di nodi risultante.
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    public static List<INode> RetracePath(INode startNode, INode endNode) {
        List<INode> path = new List<INode>();
        INode currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    /// <summary>
    /// Ritorna la distanza euristica dei 2 nodi in parametro.
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <param name="_pathFindingSettings"></param>
    /// <returns></returns>
    public static int GetDistance(INode nodeA, INode nodeB, PathFindingSettings _pathFindingSettings) {
        int dstX = Mathf.Abs((int)nodeA.GetGridPosition().y - (int)nodeB.GetGridPosition().y);
        int dstY = Mathf.Abs((int)nodeA.GetGridPosition().x - (int)nodeB.GetGridPosition().x);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public static void DoMoveToCurrentPathStep(this IPathFindingMover _this) {
        
        if (_this.CurrentPath.Count >= _this.CurrentNodeIndex)
            _this.DoMoveStep(_this.CurrentPath[_this.CurrentNodeIndex]);
    }

    
}

public struct PathFindingSettings {
    public bool IgnoreObstacles;
    /// <summary>
    /// Se true il moviemento è sul penultimo elemento del pathfinding, altrimenti sull'ultimo.
    /// </summary>
    public bool MoveToLastButOne;

    public static PathFindingSettings Tank = new PathFindingSettings() { IgnoreObstacles = false, MoveToLastButOne = true };
    public static PathFindingSettings Combattente = new PathFindingSettings() { IgnoreObstacles = true, MoveToLastButOne = true };
}
