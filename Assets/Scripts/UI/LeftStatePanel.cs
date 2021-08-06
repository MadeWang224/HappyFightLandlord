using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftStatePanel : StatePanel
{
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_LEFT_PLAYER_DATA);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_LEFT_PLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();

        GameModel gm = Models.GameModel;
        if (gm.GetLeftUserId() != -1)
        {
            this.userDto = gm.GetUserDto(gm.GetLeftUserId());
            if(gm.MatchRoomDto.ReadyUIdList.Contains(gm.GetLeftUserId()))
            {
                ReadyState();
            }
        }
        else
        {
            setPanelActive(false);
        }
    }
}
