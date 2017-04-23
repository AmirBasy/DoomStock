using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{

    public MessageLable MessageLablePrefab;


    public void ShowMessage(PopulationData unit, PopulationMessageType _type, BuildingView building = null)
    {
        MessageLable message;
        switch (_type)
        {
            case PopulationMessageType.Birth:
                message = Instantiate(MessageLablePrefab, GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, transform.rotation);
                message.Show(unit, _type);
                break;
            case PopulationMessageType.Death:
                message = Instantiate(MessageLablePrefab, GameManager.I.buildingManager.GetBuildingContainingUnit(unit),Quaternion.Euler(45, 30f, -3f));//transform.rotation.x + 45, transform.rotation.y+30, transform.rotation.z-3);
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
    public void ShowBuildingMessage(BuildingView _building, BuildingMessageType _type, PopulationData _population = null)
    {
        MessageLable message;
        switch (_type)
        {
            case BuildingMessageType.Construction:
                message = Instantiate(MessageLablePrefab, _building.transform.position, transform.rotation);
                message.ShowBuilding(_building.Data, _type);
                break;
            case BuildingMessageType.Builded:
                message = Instantiate(MessageLablePrefab, _building.transform.position, transform.rotation);
                message.ShowBuilding(_building.Data, _type);
                break;
            case BuildingMessageType.Debris:
                message = Instantiate(MessageLablePrefab, _building.transform.position, transform.rotation);
                message.ShowBuilding(_building.Data, _type);
                break;
            case BuildingMessageType.PeopleAdded:
                message = Instantiate(MessageLablePrefab, _building.transform.position, transform.rotation);
                message.ShowBuilding(_building.Data, _type);
                break;
            case BuildingMessageType.PeopleRemoved:
                message = Instantiate(MessageLablePrefab, _building.transform.position, transform.rotation);
                message.ShowBuilding(_building.Data, _type);
                break;
            default:
                break;
        }
    }
}

public enum PopulationMessageType
{
    Birth,
    Death,
    AddToBuilding,
    BackToHole
}

public enum BuildingMessageType
{
    Construction,
    Builded,
    Debris,
    PeopleAdded,
    PeopleRemoved
}