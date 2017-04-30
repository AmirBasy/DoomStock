using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationMenuComponent : MenuBase
{

    public override void LoadSelections()
    {
        PossibiliScelteAttuali.Clear();
        switch (ScelteFatte.Count)
        {
            case 0:
                foreach (var p in GameManager.I.populationManager.GetAllFreePeople())
                {
                    PossibiliScelteAttuali.Add(p);
                }
                break;
            case 1:
                foreach (var bView in CurrentPlayer.BuildingsInScene)
                {
                    if (bView.Data.currentState != BuildingData.BuildingState.Debris)
                    {
                        PossibiliScelteAttuali.Add(bView.Data);
                    }
                }
                    break;
            case 2:
                DoAction();
                return;
        }
        RefreshItemList();
        IndiceDellaSelezioneEvidenziata = 0;
    }

    public override void DoAction()
    {



        CurrentPlayer.AddPopulation(ScelteFatte[1] as BuildingData, ScelteFatte[0].UniqueID);
        ScelteFatte.Clear();
        Show(false);


    }

    #region event subscriptions
    private void OnEnable()
    {
        BuildingView.OnDestroy += RefreshList;
        PopulationManager.OnFreePopulationChanged += RefreshList2;
    }

    void RefreshList(BuildingView _buildingDestroyed)
    {
        StartCoroutine(RefreshActualList());
    }

    void RefreshList2()
    {
        StartCoroutine(RefreshActualList());
    }

    IEnumerator RefreshActualList()
    {
        yield return new WaitForSeconds(0.1f);
        LoadSelections();
        yield return null;
    }

    private void OnDisable()
    {
        BuildingView.OnDestroy -= RefreshList;
        PopulationManager.OnFreePopulationChanged -= RefreshList2;
    }
    #endregion
}
