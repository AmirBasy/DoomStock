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
    /// Inizializza il menù.
    /// </summary>
    /// <param name="_player"></param>
    void Init(Player _player, List<ISelectable> _firstLevelSelections);
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

public enum MenuTypes {
    AddPopulation,
    AddBuilding,
    Player,

}
