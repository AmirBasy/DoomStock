using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiInformation : MonoBehaviour {

    public SpriteRenderer BackgroundColor;
    public SpriteRenderer Icon;
    
    public void ShowMessage(MessageLableType _message) {
        Color iconColor = new Color(0, 0, 0, 0);
        Color backgroundColor = new Color(0,0,0,0);
        switch (_message) {
            case MessageLableType.FoodProduction:
                ColorUtility.TryParseHtmlString("FFC516FF", out backgroundColor);
                break;
            case MessageLableType.Death:
                ColorUtility.TryParseHtmlString("242424FF", out backgroundColor);
                break;
            case MessageLableType.FaithProduction:
                ColorUtility.TryParseHtmlString("226BA7FF", out backgroundColor);
                break;
            case MessageLableType.Birth:
                ColorUtility.TryParseHtmlString("#0AAE19FF", out backgroundColor);
                transform.DOShakeScale(3);
                transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {
                    Destroy(this.gameObject);
                });
                break;
            case MessageLableType.WoodProduction:
                ColorUtility.TryParseHtmlString("DF5C21FF", out backgroundColor);
                break;
            case MessageLableType.StoneProduction:
                ColorUtility.TryParseHtmlString("8D8D8DFF", out backgroundColor);
                break;
            default:
                break;
        }
        BackgroundColor.color = backgroundColor;
        //Icon.color = iconColor;
    }

  
}
public enum MessageLableType {
    FoodProduction,
    Death,
    FaithProduction,
    Birth,
    WoodProduction,
    StoneProduction,
    RemovePopulation
}