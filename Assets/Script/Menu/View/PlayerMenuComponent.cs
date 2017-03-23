using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MenuBase {

    public override void LoadSelections() {
        CurrentSelectables.Clear();
        switch (Selections.Count) {
            case 0:
                // If firstLevelSelections != null...
                break;
            case 1:

                break;
            default:
                DoAction();
                return;
        }
        RefreshItemList();
    }

    public override void DoAction() {
        
    }



}
