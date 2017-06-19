using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuBase
{

    public override void LoadSelections()
    {
        PossibiliScelteAttuali.Clear();
        switch (ScelteFatte.Count)
        {
            case 0:
                if (firstLevelSelections != null)
                {
                    foreach (ISelectable selectable in firstLevelSelections)
                    {
                        PossibiliScelteAttuali.Add(selectable);
                    }
                }
                break;
            case 1:
                Time.timeScale = 0.00000000001f;
                switch (ScelteFatte[0].UniqueID)
                {
                    case "Restart":
                        break;
                    case "Back To Menu":
                        break;
                    case "Exit":
                        break;
                    case "Resume":
                        break;
                    default:
                        break;
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
        switch (ScelteFatte[0].UniqueID)
        {
            case "Restart":
                break;
            case "Back":
                break;
            case "Exit":
                break;
            case "Resume":
                break;
            default:
                break;
        }
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
