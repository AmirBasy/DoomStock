using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MessageLable : MonoBehaviour {
    public TMP_Text text;
    public SpriteRenderer icon;
    public SpriteRenderer background;

    public void Show(PopulationData unit, PopulationMessageType _type, BuildingView building = null) {
        switch (_type) {
            case PopulationMessageType.Birth:
            text.text = unit.Name;
            background.color = Color.white;
            transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                Destroy(this.gameObject);
            });
            break;
            case PopulationMessageType.Death:
            text.text = unit.Name;
            background.color = Color.black;
            transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                Destroy(this.gameObject);
            });
            break;
            case PopulationMessageType.AddToBuilding:
            text.text = unit.Name;
            background.color = Color.yellow;
            transform.DOMove(building.transform.position, 5).OnComplete(() => {
                Destroy(this.gameObject);
            });
            break;
            case PopulationMessageType.BackToHole:
            text.text = unit.Name;
            background.color = Color.green;
            transform.DOMove(GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, 5).OnComplete(() => {
                Destroy(this.gameObject);
            });
            break;
            default:
            break;
        }

    }

    public void ShowBuilding(BuildingData _building, BuildingMessageType _type, PopulationData _population = null) {
        switch (_type)
        {
            case BuildingMessageType.Construction:
                text.text = _building.currentState.ToString();
                background.color = Color.grey;
                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, _building.BuildingTime).OnComplete(() => {
                    Destroy(this.gameObject);
                });
                break;
            case BuildingMessageType.Builded:
                text.text = _building.currentState.ToString();
                background.color = Color.grey;
                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                    Destroy(this.gameObject);
                });
                break;
            case BuildingMessageType.Debris:
                text.text = _building.currentState.ToString();
                background.color = Color.grey;
                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                    Destroy(this.gameObject);
                });
                break;
            case BuildingMessageType.PeopleAdded:
                text.text ="+1";
                background.color = Color.grey;
                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                    Destroy(this.gameObject);
                });
                break;
            case BuildingMessageType.PeopleRemoved:
                text.text = "-1";
                background.color = Color.grey;
                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                    Destroy(this.gameObject);
                });
                break;
            default:
                break;
        }
    }
}
