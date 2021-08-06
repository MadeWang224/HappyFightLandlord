using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.SET_TABLE_CARDS);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_TABLE_CARDS:
                SetTableCards(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 底牌
    /// </summary>
    private Image[] imgCards = null;

    private void Start()
    {
        imgCards = new Image[3];
        imgCards[0] = transform.Find("imgCard1").GetComponent<Image>();
        imgCards[1] = transform.Find("imgCard2").GetComponent<Image>();
        imgCards[2] = transform.Find("imgCard3").GetComponent<Image>();

    }

    /// <summary>
    /// 设置底牌
    /// </summary>
    /// <param name="cards"></param>
    private void SetTableCards(List<CardDto> cards)
    {
        //卡牌数据类
        //TODO

        imgCards[0].sprite = Resources.Load<Sprite>("Poker/" + cards[0].Name);
        imgCards[1].sprite = Resources.Load<Sprite>("Poker/" + cards[1].Name);
        imgCards[2].sprite = Resources.Load<Sprite>("Poker/" + cards[2].Name);

    }
}
