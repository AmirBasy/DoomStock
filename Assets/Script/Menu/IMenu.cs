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
    List<ISelectable> PossibiliScelteAttuali { get; set; }
    /// <summary>
    /// Indica la posizione del cursore nell'elenco dei CurrentSelectables in cui mi trovo.
    /// </summary>
    int IndiceDellaSelezioneEvidenziata { get; set; }

    List<ISelectable> firstLevelSelections { get; set; }
    /// <summary>
    /// Elenco delle selezioni effettuate.
    /// </summary>
    List<ISelectable> ScelteFatte { get; set; }
    /// <summary>
    /// Inizializza il menù.
    /// </summary>
    /// <param name="_player"></param>
    void Init(Player _player, List<ISelectable> _firstLevelSelections);
    /// <summary>
    /// Chiude il menù.
    /// </summary>
    void Close();
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
        _this.ScelteFatte.Add(selectionToAdd);
        _this.LoadSelections();
    }

    /// <summary>
    /// Rimuove l'ultima selezione fatta.
    /// </summary>
    /// <param name="_this"></param>
    public static void GoBack(this IMenu _this) {
        if (_this.ScelteFatte.Count < 1) {
            _this.Close();
            return;
        }
        _this.ScelteFatte.RemoveAt(_this.ScelteFatte.Count -1);
        //_this.ScelteFatte = _this.firstLevelSelections;
        _this.LoadSelections(); 
    }

    /// <summary>
    /// Sposta la selezione tra gli items in avanti.
    /// </summary>
    /// <param name="_this"></param>
    public static void MoveToNextItem(this IMenu _this) {
        _this.IndiceDellaSelezioneEvidenziata++;
        if(_this.IndiceDellaSelezioneEvidenziata >= _this.PossibiliScelteAttuali.Count)
            _this.IndiceDellaSelezioneEvidenziata = 0;
    }

    /// <summary>
    /// Sposta la selezione tra gli items indrè.
    /// </summary>
    /// <param name="_this"></param>
    public static void MoveToPrevItem(this IMenu _this) {
        _this.IndiceDellaSelezioneEvidenziata--;
        if (_this.IndiceDellaSelezioneEvidenziata < 0)
            _this.IndiceDellaSelezioneEvidenziata = _this.PossibiliScelteAttuali.Count - 1;
    }
}

/// <summary>
/// Interfaccia per tutti gli oggetti selezionabili.
/// </summary>
public interface ISelectable {
    string UniqueID { get; set; }
}

public enum MenuTypes {
    PopulationMenu,
    Player,

}
