using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenu {
    /// <summary>
    /// Player che ha attivato il menù.
    /// </summary>
    Player CurrentPlayer { get; set; }
    /// <summary>
    /// Elenco degli elementi selezionabili nel menù al livello attuale.
    /// </summary>
    List<ISelectable> CurrentSelectables { get; set; }
    /// <summary>
    /// Elenco delle selezioni effettuate.
    /// </summary>
    List<ISelectable> Selections { get; set; }
    /// <summary>
    /// Carica i CurrentSelectables del livello attuale.
    /// </summary>
    void LoadSelections();
    /// <summary>
    /// Azione scopo del menù.
    /// </summary>
    void DoAction();
}

public static class IMenuExtension {
    /// <summary>
    /// Aggiunge la selezione appena effettuata alla lista di selzioni fatte.
    /// </summary>
    /// <param name="_this"></param>
    /// <param name="selectionToAdd"></param>
    public static void AddSelection(this IMenu _this, ISelectable selectionToAdd) {
        _this.Selections.Add(selectionToAdd);
        _this.LoadSelections();
    }

    /// <summary>
    /// Rimuove l'ultima selezione fatta.
    /// </summary>
    /// <param name="_this"></param>
    public static void GoBack(this IMenu _this) {
        _this.Selections.RemoveAt(_this.Selections.Count);
        _this.LoadSelections();
    }
}

/// <summary>
/// Interfaccia per tutti gli oggetti selezionabili.
/// </summary>
public interface ISelectable {
    string UniqueID { get; set; }
}

#region Test
public class TestMenu : IMenu {
    public Player CurrentPlayer { get; set; }
    public List<ISelectable> CurrentSelectables { get; set; }
    public List<ISelectable> Selections {
        get;
        set;
    }

    public void LoadSelections() {
        CurrentSelectables.Clear();
        switch (Selections.Count) {
            case 0:
                foreach (var p in GameManager.I.populationManager.GetAllFreePeople()) {
                    CurrentSelectables.Add(p);
                }
                break;
            case 1:
                foreach (var bView in CurrentPlayer.BuildingsInScene) {
                    CurrentSelectables.Add(bView.Data);
                }
                break;
            case 2:
                DoAction();
                break;
        }

    }

    public void DoAction() {
        CurrentPlayer.AddPopulation(Selections[1] as BuildingData, Selections[0] as PopulationData);
    }
}

#endregion
