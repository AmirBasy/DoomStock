﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameMenu : MenuBase
{
    

    public override void LoadSelections()
    {
        if (MenuImageToLoad != null)
        {
            MenuImageToLoad.enabled = true;
        }
        foreach (TMPro.TMP_Text item in GetComponentsInChildren<TMPro.TMP_Text>())
        {
            item.enabled = true;
        }
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
                SceneManager.LoadScene("TestPlayerScene");
                Time.timeScale = 1;
                break;
            case "Back To Menu":
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
            case "Credits":
                SceneManager.LoadScene("Credits");
                break;
            case "LeaderBoard":
                break;
            default:
                break;
        }
        Time.timeScale = 1;
        ScelteFatte.Clear();
        Show(false);
        //  CurrentPlayer.currentMenu = null;
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
