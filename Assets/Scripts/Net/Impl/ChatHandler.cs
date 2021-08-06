using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatHandler : HandlerBase
{
    private ChatMsg msg = new ChatMsg();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case ChatCode.SRES:
                {
                    ChatDto dto = value as ChatDto;
                    int userId = dto.UserId;
                    int chatType = dto.ChatType;
                    string text = Constant.GetChatText(chatType);

                    msg.UserId = userId;
                    msg.ChatType = chatType;
                    msg.Text = text;

                    //œ‘ æŒƒ◊÷
                    Dispatch(AreaCode.UI, UIEvent.PLAYER_CHAT, msg);
                    //≤•∑≈…˘“Ù
                    Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_EFFECT_AUDIO, "Chat/Chat_" + chatType);
                }
                break;
            default:
                break;
        }
    }
}
