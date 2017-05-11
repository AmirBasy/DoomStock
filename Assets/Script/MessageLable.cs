using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MessageLable : MonoBehaviour {
    public TMP_Text text;
    public Sprite[] icon;
    public SpriteRenderer background;
    public Sprite[] IconAmbition;
    public TMP_Text AmbitionText;

//    public void Show(PopulationData unit, PopulationMessageType _type, BuildingView building = null) {
//        switch (_type) {
//            case PopulationMessageType.Birth:
//                text.text = unit.Name;
//                background.color = Color.cyan;
//                GetComponentInChildren<SpriteRenderer>().sprite = icon[0];
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case PopulationMessageType.Death:
//                text.text = unit.Name;
//                background.color = Color.black;
//                GetComponentInChildren<SpriteRenderer>().sprite = icon[2];
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case PopulationMessageType.AddToBuilding:
//                text.text = unit.Name;
//                background.color = Color.yellow;
//                transform.DOMove(building.transform.position, 5).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case PopulationMessageType.BackToHole:
//                text.text = unit.Name;
//                background.color = Color.green;
//                transform.DOMove(GameManager.I.gridController.GetCellPositionByStatus(CellDoomstock.CellStatus.Hole).WorldPosition, 5).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            default:
//                break;
//        }
//        AmbitionIcon(unit);
//    }

//    public void ShowBuilding(BuildingData _building, BuildingMessageType _type, PopulationData _population = null) {
//        switch (_type) {
//            case BuildingMessageType.Construction:
//                text.text = _building.currentState.ToString();
//                background.color = Color.grey;
//                GetComponentInChildren<SpriteRenderer>().sprite = icon[6];
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, _building.BuildingTime).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case BuildingMessageType.Builded:
//                text.text = _building.currentState.ToString();
//                background.color = Color.grey;
//                GetComponentInChildren<SpriteRenderer>().sprite = icon[12];
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case BuildingMessageType.Debris:
//                text.text = _building.currentState.ToString();
//                background.color = Color.grey;
//                GetComponentInChildren<SpriteRenderer>().sprite = icon[10];
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case BuildingMessageType.PeopleAdded:
//                text.text = "+1";
//                background.color = Color.grey;
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case BuildingMessageType.PeopleRemoved:
//                text.text = "-1";
//                background.color = Color.grey;
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });
//                break;
//            case BuildingMessageType.Ready:
//                text.text = "Pronto!";
//                background.color = Color.grey;
//                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
//                    Destroy(this.gameObject);
//                });

//                break;
//            default:
//                break;

//        }
//        AmbitionText.text = "";
//        GetComponentsInChildren<SpriteRenderer>()[1].sprite = IconAmbition[3];
//    }

//    public void AmbitionIcon(PopulationData unit) {
//        switch (unit.Ambition) {
//            case "Farmer":
//                AmbitionText.text = unit.Ambition;
//                GetComponentsInChildren<SpriteRenderer>()[1].sprite = IconAmbition[0];
//                break;
//            case "Cleric":
//                AmbitionText.text = unit.Ambition;
//                GetComponentsInChildren<SpriteRenderer>()[1].sprite = IconAmbition[1];
//                break;
//            case "Carpenter":
//                AmbitionText.text = unit.Ambition;
//                GetComponentsInChildren<SpriteRenderer>()[1].sprite = IconAmbition[2];
//                break;
//            default:
//                break;
//        }
//    }
}
