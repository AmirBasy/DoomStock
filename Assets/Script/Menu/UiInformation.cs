using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiInformation : MonoBehaviour {

    public Vector3 CellWorldPosition;
    public SpriteRenderer BackgroundColor;
    public SpriteRenderer Icon;
    public CellDoomstock cell;
    public string MessageType;
    
    public void ShowMessagePop_up(MessageLableType _message, string iconToGet) {
       

        Color iconColor = new Color(0, 0, 0, 0);
        Color backgroundColor = new Color(0,0,0,0);
        string IconString = "";
      
        switch (_message) {
            case MessageLableType.FoodProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.FoodColor, out backgroundColor);
                IconString = iconToGet;
                break;
            case MessageLableType.Death:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "Death";
                break;
            case MessageLableType.FaithProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.FaithColor, out backgroundColor);
                IconString = iconToGet;
                break;
            case MessageLableType.Birth:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "Birth";
                break;
            case MessageLableType.WoodProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.WoodColor, out backgroundColor);
                IconString = iconToGet;
                break;
            case MessageLableType.StoneProduction:
                ColorUtility.TryParseHtmlString(MessagesManager.StoneColor, out backgroundColor);
                IconString = iconToGet;
                break;
            case MessageLableType.AddPopulation:
                ColorUtility.TryParseHtmlString(MessagesManager.PopulationColor, out backgroundColor);
                IconString = "aggiunta_popolano";
                break;
            case MessageLableType.RemovePopulation:
                ColorUtility.TryParseHtmlString(MessagesManager.RemovePopulationColor, out backgroundColor);
                IconString = "rimozione_popolano";
                break;
            //case MessageLableType.SpiritProduction:
            //    ColorUtility.TryParseHtmlString(MessagesManager.SpiritColor, out backgroundColor);
            //    IconString = iconToGet;
            //    break;
            case MessageLableType.Reparing:
                ColorUtility.TryParseHtmlString(MessagesManager.ReparingColor, out backgroundColor);
                IconString = "accetta";
                break;
            case MessageLableType.Destroing:
                ColorUtility.TryParseHtmlString(MessagesManager.DestroingColor, out backgroundColor);
                IconString = "piccone";
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
        transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 0.5f, 1).OnComplete(() => {Destroy(this.gameObject);
        });
        BackgroundColor.color = backgroundColor;

    }

    public void ShowMessageStuck(MessageLableType _message) {
        Color iconColor = new Color(0, 0, 0, 0);
        Color backgroundColor = new Color(0, 0, 0, 0);
        string IconString = "";
        switch (_message) {
            case MessageLableType.LimitFaith:
                ColorUtility.TryParseHtmlString(MessagesManager.FaithColor, out backgroundColor);
                IconString = "fede_cappella";
                break;
            case MessageLableType.LimitFood:
                ColorUtility.TryParseHtmlString(MessagesManager.FoodColor, out backgroundColor);
                IconString = "Food";
                break;
            case MessageLableType.LimitSpirit:
                ColorUtility.TryParseHtmlString(MessagesManager.SpiritColor, out backgroundColor);
                IconString = "Spirit";
                break;
            case MessageLableType.LimitStone:
                ColorUtility.TryParseHtmlString(MessagesManager.StoneColor, out backgroundColor);
                IconString = "rock";
                break;
            case MessageLableType.LimitWood:
                ColorUtility.TryParseHtmlString(MessagesManager.WoodColor, out backgroundColor);
                IconString = "Wood";
                break;
            case MessageLableType.GetMacerie:
                ColorUtility.TryParseHtmlString(MessagesManager.UndefinedColor, out backgroundColor);
                IconString = "indicatore_quadro_edificabile";
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
        transform.DOMoveY(transform.position.y + GameManager.I.gridController.CellSize * 0.5f, 1).OnComplete(() => {

        });
        BackgroundColor.color = backgroundColor;    
    }
}
public enum MessageLableType {
    FoodProduction, FaithProduction, WoodProduction, SpiritProduction, StoneProduction,

    Death,Birth,RemovePopulation,AddPopulation,
    
    LimitFood,LimitFaith,LimitWood,LimitSpirit,LimitStone,LimitPopulation,

    Reparing,Destroing,Miracle,GetMacerie


}