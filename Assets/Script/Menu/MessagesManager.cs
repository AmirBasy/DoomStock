using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MessagesManager : MonoBehaviour
{
    #region Colors
    public static string RemovePopulationColor = "#FF0000FF";
    public static string PopulationColor = "#0AAE19FF";
    public static string FoodColor = "#DB6FA3FF";
    public static string WoodColor = "#DF5C21FF";
    public static string StoneColor = "#8D8D8DFF";
    public static string FaithColor = "#C700FFFF";
    public static string SpiritColor = "#00FF80FF";
    public static string MajorColor = "#FF0000FF";
    public static string UndefinedColor = "#FF0079FF";
    public static string ReparingColor = "#FBFF00FF";
    public static string DestroingColor = "00FFEAFF";
    #endregion

    public UiInformation uiInformationPrefab;
    float MinRange = 0;
    float MaxRange = 1;
    public GameObject Target;


    public void ShowiInformation(MessageLableType _message, CellDoomstock _worldPosition, bool isImmediate = false, string iconToGet = null)
    {

        float delay = 0;
        if (!isImmediate)
        {
            delay = Random.Range(MinRange, MaxRange);
            ;
        }
        StartCoroutine(ShowMessage(delay, _message, _worldPosition,  iconToGet));

    }

    IEnumerator ShowMessage(float waitTime, MessageLableType _message, CellDoomstock _worldPosition, string iconToGet = null)
    {

        yield return new WaitForSeconds(waitTime);
        UiInformation info = Instantiate(uiInformationPrefab,
            new Vector3 (_worldPosition.GetWorldPosition().x,
            _worldPosition.GetWorldPosition().y,
            _worldPosition.GetWorldPosition().z + 0.5f), this.transform.rotation);
        info.cell = _worldPosition;
        //info.cell = GameManager.I.gridController.Cells[(int)_worldPosition.GetWorldPosition().x, (int)_worldPosition.GetWorldPosition().y];
        switch (_message)
        {
            case MessageLableType.FoodProduction:
            case MessageLableType.FaithProduction:
            case MessageLableType.WoodProduction:
            case MessageLableType.SpiritProduction:
            case MessageLableType.StoneProduction:
            case MessageLableType.Death:
            case MessageLableType.Birth:
            case MessageLableType.RemovePopulation:
            case MessageLableType.AddPopulation:
            case MessageLableType.Reparing:
            case MessageLableType.Destroing:
                info.ShowMessagePop_up(_message, iconToGet);
                info.MessageType = "PopUp";
                break;
            case MessageLableType.LimitFood:
            case MessageLableType.LimitFaith:
            case MessageLableType.LimitWood:
            case MessageLableType.LimitSpirit:
            case MessageLableType.LimitStone:
            case MessageLableType.LimitPopulation:
            case MessageLableType.GetMacerie:
                info.ShowMessageStuck(_message);
                info.MessageType = "Stuck";
                // info.CellWorldPosition = _worldPosition;
                break;
            default:
                break;
        }

    }

    public void DesotryUiInformation(CellDoomstock _cell)
    {
        UiInformation[] uiToReturn = FindObjectsOfType<UiInformation>();
        foreach (var item in uiToReturn)
        {
            if (item.cell == _cell && item.MessageType == "Stuck")
            {
                item.transform.DOMove(new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z), 1f).SetEase(Ease.InOutCirc, 1).OnComplete(() =>
                {
                    Destroy(item.gameObject);
                });
                item.transform.DOScale(0, 0.2f).SetDelay(0.8f);
            }
        }
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