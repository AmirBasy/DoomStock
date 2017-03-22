using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationMenuComponent : MonoBehaviour, IMenu {
    public Player CurrentPlayer { get; set; }
    private List<ISelectable> _currentSelectables = new List<ISelectable>();
    /// <summary>
    /// 
    /// </summary>
    public List<ISelectable> CurrentSelectables {
        get { return _currentSelectables; }
        set { _currentSelectables = value; }
    }

    private List<ISelectable> _selections = new List<ISelectable>();
    /// <summary>
    /// 
    /// </summary>
    public List<ISelectable> Selections {
        get { return _selections; }
        set { _selections = value; }
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

    // Use this for initialization
    void Start () {
        LoadSelections();
        RefreshItemList();
	}

    // Update is called once per frame
    void Update() {

    }

    #region View

    public GameObject ButtonPrefab;

    void RefreshItemList() {
        foreach (var item in CurrentSelectables) {
            GameObject newGO = Instantiate(ButtonPrefab, transform);
            SelectableButton newButton = newGO.GetComponent<SelectableButton>();
            newButton.SetData(item);
            newButton.onClick.AddListener(() => this.AddSelection(newButton.SelectionData));
        } 
    }


    #endregion
}
