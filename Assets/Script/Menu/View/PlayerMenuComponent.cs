using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MenuBase {
    

    public override void LoadSelections() {
        PossibiliScelteAttuali.Clear();
        switch (ScelteFatte.Count) {
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
                switch (ScelteFatte[0].UniqueID)
                {
                    case " + Building":

                        foreach (ISelectable building in CurrentPlayer.BuildingsDataPrefabs)
                        {
                            PossibiliScelteAttuali.Add(building);
                        }
                        break;
                    case " - Building":
                        foreach (BuildingView building in CurrentPlayer.BuildingsInScene)
                        {
                            PossibiliScelteAttuali.Add(building.Data);
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
        IndiceDellaSelezioneEvidenziata = 0;
    }

    public override void DoAction() {
        switch (ScelteFatte[0].UniqueID)
        {
            case " + Building":
                CurrentPlayer.DeployBuilding(ScelteFatte[1] as BuildingData);
                break;
            case " - Building":
                CurrentPlayer.DestroyBuilding(ScelteFatte[1].UniqueID);
                break;
            case " -  People":
                //Chiamare funzione population
                break;
            default:
                break;
        }
        Close();
        
    }
    
    protected override void CreateMenuItem(ISelectable _item) {
        GameObject newGO = Instantiate(ButtonPrefab, MenuItemsContainer);
        SelectableMenuItem newItem = newGO.GetComponent<SelectableMenuItem>();
        newItem.SetData(_item);
    }

}
