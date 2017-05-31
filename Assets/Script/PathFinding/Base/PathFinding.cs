using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public interface IPathFinding {


}
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
    public static List<INode> Find(this IPathFinding _this, INode startNode, INode lastNode, bool ignoreTraversable = false) {
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

    public static void DoMoveToCurrentPathStep(this IPathFindingMover _this) {
        
        if (_this.CurrentPath.Count >= _this.CurrentNodeIndex)
            _this.DoMoveStep(_this.CurrentPath[_this.CurrentNodeIndex]);
    }

    
}
