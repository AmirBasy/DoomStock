using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class MenuBase : MonoBehaviour, IMenu {

    #region Properties
    public Player CurrentPlayer { get; set; }

    private List<ISelectable> _possibiliScelteAttuali = new List<ISelectable>();
    /// <summary>
    /// Lista degli elementi selezionabili attualmente nel menù.
    /// </summary>
    public List<ISelectable> PossibiliScelteAttuali {
        get { return _possibiliScelteAttuali; }
        set { _possibiliScelteAttuali = value; }
    }

    
    private int _indiceDellaSelezioneEvidenziata;
    /// <summary>
    /// Indica la posizione del cursore nell'elenco dei CurrentSelectables in cui mi trovo.
    /// </summary>
    public int IndiceDellaSelezioneEvidenziata {
        get { return _indiceDellaSelezioneEvidenziata; }
        set {
            _indiceDellaSelezioneEvidenziata = value;
            SelectActiveItem(_indiceDellaSelezioneEvidenziata);
        }
    }


    private List<ISelectable> _scelteFatte = new List<ISelectable>();
    /// <summary>
    /// Lista delle selezioni effettuate dal pleyer per scendere nei sottolivelli del menù.
    /// </summary>
    public List<ISelectable> ScelteFatte {
        get { return _scelteFatte; }
        set { _scelteFatte = value;}   
    }

    /// <summary>
    /// Se necessario contiene il primo livello di selectables.
    /// </summary>
    public List<ISelectable> firstLevelSelections { get; set; }
    #endregion

    #region ViewProperties
    bool IsVisible = false;
    
    #endregion

    #region View

    private void Start() {
        Close();
    }

    public GameObject ButtonPrefab;
    public Transform MenuItemsContainer;

    /// <summary>
    /// Esegue il refresh della lista degli oggetti visibi del menù.
    /// </summary>
    public void RefreshItemList() {
        if (!IsVisible)
            return;
        foreach (SelectableMenuItem item in GetComponentsInChildren<SelectableMenuItem>()) {
            DestroyObject(item.gameObject);
        } 

        foreach (ISelectable item in PossibiliScelteAttuali) {
            
            CreateMenuItem(item);

        }

    }

    /// <summary>
    /// Seleziona l'elemento della lista degli items del menù all'index indicato come parametro, e disattiva tutti gli altri.
    /// </summary>
    /// <param name="selectedItemIndex"></param>
    public void SelectActiveItem(int selectedItemIndex) {
        for (int i = 0; i < MenuItemsContainer.GetComponentsInChildren<SelectableMenuItem>().Count(); i++) {
            if (selectedItemIndex == i)
            {
                MenuItemsContainer.GetComponentsInChildren<SelectableMenuItem>()[i].Select(true);
                return;
            }
            else
                MenuItemsContainer.GetComponentsInChildren<SelectableMenuItem>()[i].Select(false);
        }
    }

    /// <summary>
    /// Crea un nuovo MenuItem per i button
    /// </summary>
    /// <param name="_item"></param>
    protected virtual void CreateMenuItem(ISelectable _item) {
        GameObject newGO = Instantiate(ButtonPrefab, MenuItemsContainer);
        SelectableMenuItem newButton = newGO.GetComponent<SelectableMenuItem>();
        newButton.SetData(_item);
        //newButton.onClick.AddListener(() => this.AddSelection(newButton.SelectionData));
    }

    /// <summary>
    /// Chiude il menù.
    /// </summary>
    public void Close() {
        Show(false);
        ScelteFatte.Clear();
    }

    /// <summary>
    /// Abilita o disabilita la visualizzazione del menù.
    /// </summary>
    /// <param name="_show"></param>
    public void Show(bool _show) {
        IsVisible = _show;
        MenuItemsContainer.gameObject.SetActive(IsVisible);
        if (!_show && CurrentPlayer != null)
            CurrentPlayer.OnMenuClosed(this);
    }

    #endregion

    /// <summary>
    /// Inizializza il menù.
    /// </summary>
    /// <param name="_player"></param>
    public void Init(Player _player, List<ISelectable> _firstLevelSelections = null) {
        CurrentPlayer = _player;
        if (_firstLevelSelections != null)
            firstLevelSelections = _firstLevelSelections;
        Show(true);
        LoadSelections();
    }

    public abstract void DoAction();

    /// <summary>
    /// Carico la lista dei CurrentSelectables.
    /// </summary>
    public abstract void LoadSelections();
}
