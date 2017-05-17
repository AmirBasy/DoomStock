using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MenuBase
{

    /// <summary>
    /// carica le possibili scelte selezionabili.
    /// </summary>
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
                CellDoomstock cell = GameManager.I.gridController.Cells[CurrentPlayer.XpositionOnGrid, CurrentPlayer.YpositionOnGrid];
                switch (ScelteFatte[0].UniqueID)
                {
                    case " + Building":

                        foreach (BuildingData building in CurrentPlayer.BuildingsDataPrefabs)
                        {
                            BuildingData newBuildingInstance = Instantiate<BuildingData>(building);
                            PossibiliScelteAttuali.Add(newBuildingInstance);
                        }
                        break;
                    case " - Building":
                        DoAction();
                        break;
           
                    case " Info ":

                        break;
                    //case " - Debris":
                    //    DoAction();
                    //    break;
                    case " Prendi ":
                        DoAction();
                        break;
                    default:
                        break;
                }
                break;


            default:
                DoAction();
                return;
        }
        RefreshItemList();
        IndiceDellaSelezioneEvidenziata = 0;
    }

    /// <summary>
    /// esegue un'azione alla selezione.
    /// </summary>
    public override void DoAction()
    {
        CellDoomstock cell = GameManager.I.gridController.Cells[CurrentPlayer.XpositionOnGrid, CurrentPlayer.YpositionOnGrid];
        switch (ScelteFatte[0].UniqueID)
        {
            case " + Building":
                CurrentPlayer.DeployBuilding(ScelteFatte[1] as BuildingData);
                break;
            case " - Building":
                CurrentPlayer.DestroyBuilding(cell.building.UniqueID);
                break;
     
            case " Info ":
                break;
            //case " - Debris":
            //    CurrentPlayer.RemoveBuildingDebris(cell.building);
            //    break;

            default:
                break;
        }
        Close();
    }

    /// <summary>
    /// crea gli oggetti selezionabili.
    /// </summary>
    /// <param name="_item"></param>
    protected override void CreateMenuItem(ISelectable _item)
    {
        GameObject newGO = Instantiate(ButtonPrefab, MenuItemsContainer);
        SelectableMenuItem newItem = newGO.GetComponent<SelectableMenuItem>();
        newItem.SetData(_item);
    }

    #region event subscriptions
    private void OnEnable()
    {
        PopulationManager.OnFreePopulationChanged += RefreshList;
    }

    /// <summary>
    /// ricarica le scelte del menu, se ce ne sono di nuove.
    /// </summary>
    void RefreshList()
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
        PopulationManager.OnFreePopulationChanged -= RefreshList;
    }
    #endregion
}
