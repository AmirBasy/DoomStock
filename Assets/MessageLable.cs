using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MessageLable : MonoBehaviour {
    public TMP_Text text;
    public SpriteRenderer icon;
    public SpriteRenderer background;

    public void Show(PopulationData unit, PopulationMessageType _type) {
        switch (_type) {
            case PopulationMessageType.Birth:
            text.text = unit.Name;
            transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                Destroy(this.gameObject);
            });
            break;
            case PopulationMessageType.Death:
            break;
            default:
            break;
        }

    }
}
