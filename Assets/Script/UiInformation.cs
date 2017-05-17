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
        string IconString = "";
        switch (_message) {
            case MessageLableType.FoodProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.FoodColor, out backgroundColor);
                IconString = "1+";
                break;
            case MessageLableType.Death:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "Death";
                break;
            case MessageLableType.FaithProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.FaithColor, out backgroundColor);
                IconString = "1+";
                break;
            case MessageLableType.Birth:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "Birth";
                break;
            case MessageLableType.WoodProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.WoodColor, out backgroundColor);
                IconString = "1+";
                break;
            case MessageLableType.StoneProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.StoneColor, out backgroundColor);
                IconString = "1+";
                break;
            case MessageLableType.AddPopulation:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "1+";
                break;
            case MessageLableType.RemovePopulation:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "1-";
                break;
            case MessageLableType.SpiritProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.SpiritColor, out backgroundColor);
                IconString = "Spirit";
                break; 
            default:
                break;
        }
        if (IconString != null) {
            Icon.gameObject.SetActive(true);
            Icon.sprite = Resources.Load<Sprite>("Icons/vector/" + IconString); 
        } else {
            Icon.gameObject.SetActive(false);
        }
        transform.DOShakeScale(3);
        transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 2, 4).OnComplete(() => {Destroy(this.gameObject);
        });
        BackgroundColor.color = backgroundColor;

    }

  
}
public enum MessageLableType {
    FoodProduction,
    Death,
    FaithProduction,
    Birth,
    WoodProduction,
    StoneProduction,
    RemovePopulation,
    AddPopulation,
    SpiritProduction,
    Limit
}