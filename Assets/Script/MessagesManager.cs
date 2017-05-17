using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MessagesManager : MonoBehaviour
{

    public UiInformation uiInformationPrefab;


    public void ShowiInformation(MessageLableType _message, Vector3 WorldPosition) {
           

        UiInformation info = Instantiate(uiInformationPrefab, WorldPosition,this.transform.rotation);
        info.ShowMessage(_message);
    }
    //public void ShowMessage(PopulationData unit, PopulationMessageType _type, BuildingView building = null)
    //{
    //    MessageLable message;
    //    switch (_type)
    //    {
    //        case PopulationMessageType.Birth:
    //            //message = Instantiate(uiInformationPrefab, GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, transform.rotation);
    //            message.Show(unit, _type);
    //            break;
    //        case PopulationMessageType.Death:
    //            message = Instantiate(uiInformationPrefab, GameManager.I.buildingManager.GetBuildingContainingUnit(unit),Quaternion.Euler(45, 30f, -3f));//transform.rotation.x + 45, transform.rotation.y+30, transform.rotation.z-3);
    //            message.Show(unit, _type);
    //            break;
    //        case PopulationMessageType.AddToBuilding:
    //            message = Instantiate(uiInformationPrefab, GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, transform.rotation);
    //            message.Show(unit, _type, building);
    //            break;
    //        case PopulationMessageType.BackToHole:
    //            message = Instantiate(uiInformationPrefab, GameManager.I.buildingManager.GetBuildingContainingUnit(unit), transform.rotation);
    //            message.Show(unit, _type);
    //            break;
    //        default:
    //            break;
    //    }
    //}
    //public void ShowBuildingMessage(BuildingView _building, BuildingMessageType _type, PopulationData _population = null)
    //{
    //    MessageLable message;
    //    message = Instantiate(uiInformationPrefab, _building.transform.position, transform.rotation);
    //    message.ShowBuilding(_building.Data, _type);
    //}
}

//public enum PopulationMessageType
//{
//    Birth,
//    Death,
//    AddToBuilding,
//    BackToHole
//}

//public enum BuildingMessageType
//{
//    Construction,
//    Builded,
//    Debris,
//    PeopleAdded,
//    PeopleRemoved,
//    Ready
//}