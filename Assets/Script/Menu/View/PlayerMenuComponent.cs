using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MenuBase {
    

    public override void LoadSelections() {
        CurrentSelectables.Clear();
        switch (Selections.Count) {
           
            case 0:
                if (firstLevelSelections != null)
                {
                    foreach (ISelectable selectable in firstLevelSelections)
                    {
                        CurrentSelectables.Add(selectable);
                    }
                    foreach (ISelectable item in firstLevelSelections)
                    {
                        FirstSaveList.Add(item);
                    }
                }
                break;
            case 1:
                switch (Selections[0].UniqueID)
                {
                    case " + Building":

                        foreach (ISelectable building in CurrentPlayer.BuildingsDataPrefabs)
                        {
                            CurrentSelectables.Add(building);
                        }
                        break;
                    case " - Building":
                        foreach (BuildingView building in CurrentPlayer.BuildingsInScene)
                        {
                            CurrentSelectables.Add(building.Data);
                        }
                        break;
                    case " -  People":
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
        
    }

    public override void DoAction() {
        switch (Selections[0].UniqueID)
        {
            case " + Building":
                CurrentPlayer.DeployBuilding(Selections[1] as BuildingData);
                break;
            case " - Building":
                CurrentPlayer.DestroyBuilding(Selections[1].UniqueID);
                break;
            case " -  People":
                //Chiamare funzione population
                break;
            default:
                break;
        }
        Show(false);
        Selections.Clear();
        FirstSaveList.Clear();
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
