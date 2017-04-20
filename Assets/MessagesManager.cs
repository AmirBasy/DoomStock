using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour {

    public MessageLable MessageLablePrefab;


    public void ShowMessage(PopulationData unit, PopulationMessageType _type, BuildingView building = null) {
        MessageLable message;
        switch (_type) {
            case PopulationMessageType.Birth:
            message = Instantiate(MessageLablePrefab, GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, transform.rotation);
            message.Show(unit, _type);
            break;
            case PopulationMessageType.Death:
            message = Instantiate(MessageLablePrefab,GameManager.I.buildingManager.GetBuildingContainingUnit(unit),transform.rotation);
            message.Show(unit, _type);
            break;
            case PopulationMessageType.AddToBuilding:
            message = Instantiate(MessageLablePrefab, GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, transform.rotation);
            message.Show(unit, _type, building);
            break;
            case PopulationMessageType.BackToHole:
            message = Instantiate(MessageLablePrefab, GameManager.I.buildingManager.GetBuildingContainingUnit(unit), transform.rotation);
            message.Show(unit, _type);
            break;
            default:
            break;
        }
    }
}
public enum PopulationMessageType{
    Birth,
    Death,
    AddToBuilding,
    BackToHole
}