using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.GET_CARD_SRES:
                GetCards(value as List<CardDto>);
                break;
            case FightCode.TURN_GRAB_BRO:
                TurnGrabBro((int)value);
                break;
            case FightCode.GRAB_LANDLORD_BRO:
                GrabLandlordBro(value as GrabDto);
                break;
            case FightCode.TURN_DEAL_BRO:
                TurnDealBro((int)value);
                break;
            case FightCode.DEAL_BRO:
                DealBro(value as DealDto);
                break;
            case FightCode.DEAL_SRES:
                DealResponse((int)value);
                break;
            case FightCode.OVER_BRO:
                OverBro(value as OverDto);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 游戏结束的广播
    /// </summary>
    /// <param name="dto"></param>
    private void OverBro(OverDto dto)
    {
        //播放音效
        if(dto.WinUIdList.Contains(Models.GameModel.UserDto.Id))
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_EFFECT_AUDIO, "Fight/MusicEx_Win");
        }
        else
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_EFFECT_AUDIO, "Fight/MusicEx_Lose");
        }
        //显示面板 
        Dispatch(AreaCode.UI, UIEvent.SHOW_OVER_PANEL, dto);
    }

    /// <summary>
    /// 出牌响应
    /// </summary>
    /// <param name="result"></param>
    private void DealResponse(int result)
    {
        if(result==-1)
        {
            //出的牌管不上上一个玩家的牌
            PromptMsg promptMsg = new PromptMsg("管不上上一个玩家的牌", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            //重新显示出牌
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, true);
        }
    }

    /// <summary>
    /// 同步出牌
    /// </summary>
    private void DealBro(DealDto dto)
    {
        //移除出了的手牌
        int eventCode = -1;
        if (dto.UserId == Models.GameModel.MatchRoomDto.LeftId)
        {
            eventCode = CharacterEvent.REMOVE_LEFT_CARD;
        }
        else if (dto.UserId == Models.GameModel.MatchRoomDto.RightId)
        {
            eventCode = CharacterEvent.REMOVE_RIGHT_CARD;
        }
        else if (dto.UserId == Models.GameModel.UserDto.Id)
        {
            eventCode = CharacterEvent.REMOVE_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto.RemainCardList);
        //显示到桌面上
        Dispatch(AreaCode.CHARACTER, CharacterEvent.UPDATE_SHOW_DESK, dto.SelectCardList);
        //播放出牌音效
        PlayDealAudio(dto.Type, dto.Weight);

    }

    /// <summary>
    /// 播放出牌音效
    /// </summary>
    private void PlayDealAudio(int cardType,int weight)
    {
        string audioName = "Fight/";

        switch (cardType)
        {
            case CardType.SINGLE:
                audioName += "Woman_" + weight;
                break;
            case CardType.DOUBLE:
                audioName += "Woman_dui" + weight / 2;
                break;
            case CardType.STRAIGHT:
                audioName += "Woman_shunzi";
                break;
            case CardType.DOUBLE_STRAIGHT:
                audioName += "Woman_liandui";
                break;
            case CardType.TRIPLE_STRAIGHT:
                audioName += "Woman_feiji";
                break;
            case CardType.THREE:
                audioName += "Woman_tuple" + weight / 3;
                break;
            case CardType.THREE_ONE:
                audioName += "Woman_sandaiyi";
                break;
            case CardType.THREE_TWO:
                audioName += "Woman_sandaiyidui";
                break;
            case CardType.BOOM:
                audioName += "Woman_zhadan";
                break;
            case CardType.JOKER_BOOM:
                audioName += "Woman_wangzha";
                break;
            default:
                break;
        }

        Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_EFFECT_AUDIO, audioName);
    }

    /// <summary>
    /// 转换出牌的处理
    /// </summary>
    private void TurnDealBro(int userId)
    {
        if(Models.GameModel.UserDto.Id==userId)
        {
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, true);
        }    
    }

    /// <summary>
    /// 抢地主成功的处理
    /// </summary>
    /// <param name="dto"></param>
    private void GrabLandlordBro(GrabDto dto)
    {
        //跟该角色身份显示
        Dispatch(AreaCode.UI, UIEvent.PLAYER_CHANGE_IDENTITY, dto.UserId);
        //播放抢地主的声音
        Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_EFFECT_AUDIO, "Fight/Woman_Order");
        //显示3张底牌
        Dispatch(AreaCode.UI, UIEvent.SET_TABLE_CARDS, dto.TableGradList);
        //给地主添加手牌
        int eventCode = -1;
        if(dto.UserId==Models.GameModel.MatchRoomDto.LeftId)
        {
            eventCode = CharacterEvent.ADD_LEFT_CARD;
        }
        else if(dto.UserId == Models.GameModel.MatchRoomDto.RightId)
        {
            eventCode = CharacterEvent.ADD_RIGHT_CARD;
        }
        else if(dto.UserId == Models.GameModel.UserDto.Id)
        {
            eventCode = CharacterEvent.ADD_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto);
    }

    /// <summary>
    /// 是否是第一个玩家抢地主
    /// </summary>
    private bool isFirst = true;

    /// <summary>
    /// 抢地主
    /// </summary>
    /// <param name="userId"></param>
    private void TurnGrabBro(int userId)
    {
        if(isFirst==true)
        {
            isFirst = false;
        }
        else
        {
            //播放声音
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_EFFECT_AUDIO, "Fight/Woman_NoOrder");
        }
        //如果自身,显示两个按钮
        if(userId==Models.GameModel.UserDto.Id)
        {
            Dispatch(AreaCode.UI, UIEvent.SHOW_GRAB_BUTTON, true);
        }
    }

    /// <summary>
    /// 获取卡牌的处理
    /// </summary>
    /// <param name="cardList"></param>
    private void GetCards(List<CardDto> cardList)
    {
        //创建牌
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_MY_CARD, cardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_LEFT_CARD, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_RIGHT_CARD, null);

        //设置倍数为1
        Dispatch(AreaCode.UI, UIEvent.CHANGE_MUTIPLE, 1);
    }
}
