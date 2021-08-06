using Protocol.Code;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_MY_CARD,
            CharacterEvent.ADD_MY_CARD,
            CharacterEvent.DEAL_CARD,
            CharacterEvent.REMOVE_MY_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_MY_CARD:
                StartCoroutine(InitCardList(message as List<CardDto>));
                break;
            case CharacterEvent.ADD_MY_CARD:
                AddTableCard(message as GrabDto);
                break;
            case CharacterEvent.DEAL_CARD:
                DealSelectCard();
                break;
            case CharacterEvent.REMOVE_MY_CARD:
                RemoveCard(message as List<CardDto>);
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

    private PromptMsg promptMsg;

    private SocketMsg socketMsg;

    private void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
    }

    /// <summary>
    /// 出牌 出选中的牌
    /// </summary>
    private void DealSelectCard()
    {
        List<CardDto> selectCardList = GetSelectCard();
        DealDto dto = new DealDto(selectCardList, Models.GameModel.UserDto.Id);
        //是否合法
        if(dto.IsRegular==false)
        {
            promptMsg.Change("请选择合理的手牌!", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, true);
            return;
        }
        else
        {
            //出牌
            socketMsg.Change(OpCode.FIGHT, FightCode.DEAL_CREQ, dto);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
    }

    /// <summary>
    /// 移除卡牌的游戏物体
    /// </summary>
    /// <param name="remainCardList">剩下的</param>
    private void RemoveCard(List<CardDto> remainCardList)
    {
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            if (remainCardList.Count == 0)
                break;
            else
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(remainCardList[index], index, true);
                index++;
                //没有牌了
                if(index==remainCardList.Count)
                {
                    break;
                }
            }
        }
        //其余的隐藏
        for (int i = index; i < cardCtrlList.Count; i++)
        {
            cardCtrlList[i].Selected = false;
            cardCtrlList[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 获取选中的牌
    /// </summary>
    private List<CardDto> GetSelectCard()
    {
        List<CardDto> selectCardList = new List<CardDto>();
        foreach (var cardCtrl in cardCtrlList)
        {
            if(cardCtrl.Selected==true)
            {
                selectCardList.Add(cardCtrl.CardDto);
            }
        }
        return selectCardList;
    }

    /// <summary>
    /// 初始化显示卡牌
    /// </summary>
    /// <param name="cardList"></param>
    private IEnumerator InitCardList(List<CardDto> cardList)
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");

        for (int i = 0; i < cardList.Count; i++)
        {
            CreateGo(cardList[i], i,cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 创建卡牌游戏物体
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void CreateGo(CardDto card,int index,GameObject cardPrefab)
    {
        GameObject cardGo = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2((0.25f * index), 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);

        //存储到本地
        this.cardCtrlList.Add(cardCtrl);
    }

    /// <summary>
    /// 添加底牌
    /// </summary>
    /// <param name="cardList"></param>
    private void AddTableCard(GrabDto dto)
    {
        List<CardDto> tableCards = dto.TableGradList;
        List<CardDto> playerCards = dto.PlayerCardList;

        //复用之前的卡牌
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            cardCtrl.gameObject.SetActive(true);
            cardCtrl.Init(playerCards[index], index, true);
            index++;
        }
        //创建新的3张卡牌
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = index; i < playerCards.Count; i++)
        {
            CreateGo(playerCards[i], i, cardPrefab);
        }
    }
}
