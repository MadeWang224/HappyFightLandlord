using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.UPDATE_SHOW_DESK);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.UPDATE_SHOW_DESK:
                UpdateShowDesk(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 自身管理的卡牌列表
    /// </summary>
    private List<CardCtrl> cardCtrlList;

    /// <summary>
    /// 卡牌的父物体
    /// </summary>
    private Transform cardParent;

    private void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();
    }

    /// <summary>
    /// 更新显示桌面的牌
    /// </summary>
    /// <param name="cardList"></param>
    private void UpdateShowDesk(List<CardDto> cardList)
    {
        if(cardCtrlList.Count>cardList.Count)
        {
            //原来比现在多
            int index = 0;
            foreach (var cardCtrl in cardCtrlList)
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(cardList[index], index, true);
                index++;
                if(index==cardList.Count)
                {
                    break;
                }
            }
            for (int i = index; i < cardCtrlList.Count; i++)
            {
                cardCtrlList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            //原来比现在少
            int index = 0;
            foreach (var cardCtrl in cardCtrlList)
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(cardList[index], index, true);
                index++;
            }
            //创建新的卡牌
            GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
            for (int i = index; i < cardList.Count; i++)
            {
                CreateGo(cardList[i], i, cardPrefab);
            }
        }
    }

    /// <summary>
    /// 创建卡牌游戏物体
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void CreateGo(CardDto card, int index, GameObject cardPrefab)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2((0.25f * index), 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);

        //存储到本地
        this.cardCtrlList.Add(cardCtrl);
    }
}
