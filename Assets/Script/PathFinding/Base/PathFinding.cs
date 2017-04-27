using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface PathFinding {
    List<INode> Open{ get; set; }
    List<INode> Closed { get; set; }
    INode current { get; set; }

}

public static class PathFindingExtension {
    /// <summary>
    /// trova il percorso dallo startNode al lastNode
    /// </summary>
    /// <param name="_this"></param>
    /// <param name="startNode"></param>
    /// <param name="lastNode"></param>
    public static void Find(this PathFinding _this, INode startNode, INode lastNode) {

        _this.Open.Add(startNode);

        for (int i = 0; i < _this.Open.Count; i++) {
            //assegna a current l'INode con f cost più basso
            INode lower = null;

            foreach (var item in _this.Open) {
                if (lower == null)
                    lower = item;
                else {
                    if (item.F_Cost < lower.F_Cost)
                        lower = item;
                }
            }
            // mette current nella lista dei già analizzati e lo rimuove dai disponibili
            _this.current = lower;
            _this.Open.Remove(_this.current);
            _this.Closed.Add(_this.current);
            //controllo se il currentNode è uguale al lastNode
            if (_this.current == lastNode) 
                return;
            foreach (var neighbour in _this.current.GetNeighbours()) {
                if (!neighbour.isTraversable || _this.Closed.Contains(neighbour)) {
                    continue;
                }
                neighbour.SetCost(startNode, lastNode);
                if (!_this.Open.Contains(neighbour)) {
                    _this.Open.Add(neighbour);
                } else {
                    INode n = _this.Open.Find(node => node.GetGridPosition() == neighbour.GetGridPosition());
                    if (neighbour.F_Cost < n.F_Cost) {
                        n = neighbour;
                    }
                }
                // TODO : set parent
            }
        }


    }

}
