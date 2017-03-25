using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationMenuComponent : MenuBase {

    List<ISelectable> SaveSelectable = new List<ISelectable>();  

    public override void LoadSelections() {
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
                return;
        }
        RefreshItemList();
    }

    public override void DoAction() {
        CurrentPlayer.AddPopulation(Selections[1] as BuildingData, Selections[0] as PopulationData);
        Show(false);
    }

    #region event subscriptions
    private void OnEnable() {
        BuildingView.OnDestroy += OnEventDecay;
    }

    void OnEventDecay(BuildingView _buildingDestroyed) {
        StartCoroutine(RefreshActualList());
    }

    IEnumerator RefreshActualList() {
        yield return new WaitForSeconds(0.1f);
        LoadSelections();
        yield return null;
    }

    private void OnDisable() {
        BuildingView.OnDestroy -= OnEventDecay;
    }
    #endregion
}
