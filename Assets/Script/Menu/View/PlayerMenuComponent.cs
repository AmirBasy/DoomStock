using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MenuBase {
    /// <summary>
    /// Lista momentanea che passa gli ISelectable scelti nella fase successiva del menu
    /// </summary>
    List<ISelectable> SaveList = new List<ISelectable>();

    public override void LoadSelections() {
        CurrentSelectables.Clear();
        switch (Selections.Count) {
            case 0:
                if (firstLevelSelections != null)
                    foreach (ISelectable selectable in firstLevelSelections){
                        CurrentSelectables.Add(selectable);
                        SaveList.Add(selectable);
                    }
                

                break;
            case 1:
                 foreach (ISelectable building in CurrentPlayer.BuildingsDataPrefabs)
                 {
                    CurrentSelectables.Add(building);
                 }              
                break;
            default:
                DoAction();
                return;
        }
        RefreshItemList();
        
    }

    public override void DoAction() {
        CurrentPlayer.DeployBuilding(Selections[1] as BuildingView);
        Show(false);
    }

    protected override void CreateMenuItem(ISelectable _item) {
        GameObject newGO = Instantiate(ButtonPrefab, MenuItemsContainer);
        SelectableMenuItem newItem = newGO.GetComponent<SelectableMenuItem>();
        newItem.SetData(_item);
        // TODO: Add selection logic
        newItem.gameObject.AddComponent<SelectableButton>().onClick.AddListener(() => this.AddSelection(newItem.SelectionData));
        
        //newItem.onClick.AddListener(() => this.AddSelection(newButton.SelectionData));
    }

}
