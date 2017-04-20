using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour {

    public MessageLable MessageLablePrefab;


    public void ShowMessage(PopulationData unit, PopulationMessageType _type) {
        switch (_type) {
            case PopulationMessageType.Birth:
            MessageLable message = Instantiate(MessageLablePrefab, GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, transform.rotation);
            message.Show(unit, _type);
            break;
            case PopulationMessageType.Death:
            break;
            default:
            break;
        }
    }
}
public enum PopulationMessageType{
    Birth,
    Death
}