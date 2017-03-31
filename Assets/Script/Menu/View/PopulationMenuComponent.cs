using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationMenuComponent : MenuBase {

    public override void LoadSelections() {
        PossibiliScelteAttuali.Clear();
        switch (ScelteFatte.Count) {
            case 0:
                foreach (var p in GameManager.I.populationManager.AllFreePeople) {
                    PossibiliScelteAttuali.Add(p);
                }
                break;
            case 1:
                    foreach (var bView in CurrentPlayer.BuildingsInScene)
                    {
                        PossibiliScelteAttuali.Add(bView.Data);
                    }
                 break;
            case 2:
                DoAction();           
                return;
        }
        RefreshItemList();
    }

    public override void DoAction() {
        CurrentPlayer.AddPopulation(ScelteFatte[1] as BuildingData, ScelteFatte[0] as PopulationData);
        GameManager.I.populationManager.AllFreePeople.Remove(ScelteFatte[0] as PopulationData);
        GameManager.I.populationManager.FreePeopleCounter--;
        ScelteFatte.Clear();
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
