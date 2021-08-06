using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MatchHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.ENTER_SRES:
                EnterResponse(value as MatchRoomDto);
                break;
            case MatchCode.ENTER_BRO:
                EnterBro(value as UserDto);
                break;
            case MatchCode.LEAVE_BRO:
                LeaveBro((int)value);
                break;
            case MatchCode.READY_BRO:
                ReadyBro((int)value);
                break;
            case MatchCode.START_BRO:
                StartBro();
                break;
            default:
                break;
        }
    }

    PromptMsg promptMsg = new PromptMsg();

    /// <summary>
    ///  自己进入房间的响应
    /// </summary>
    /// <param name="matchRoom"></param>
    private void EnterResponse(MatchRoomDto matchRoom)
    {
        ////储存本地
        //GameModel gModel = Models.GameModel;
        //gModel.MatchRoomDto = matchRoom;
        //int myUserId = gModel.UserDto.Id;
        ////重置玩家的位置信息
        //gModel.MatchRoomDto.ResetPosition(myUserId);

        ////显示现在房间内的玩家
        //if(matchRoom.LeftId!=-1)
        //{
        //    UserDto leftUserDto = matchRoom.UIdUserDict[matchRoom.LeftId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        //}
        //if(matchRoom.RightId!=-1)
        //{
        //    UserDto rightUserDto = matchRoom.UIdUserDict[matchRoom.RightId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightUserDto);
        //}

        Models.GameModel.MatchRoomDto = matchRoom;
        ResetPosition();

        ////更新自身数据
        //int myUserId = Models.GameModel.UserDto.Id;
        //UserDto myUserDto = matchRoom.UIdUserDict[myUserId];
        ////给自己绑定数据
        //Dispatch(AreaCode.UI, UIEvent.SET_MY_PLAYER_DATA, myUserDto);

        //显示进入房间的按钮
        Dispatch(AreaCode.UI, UIEvent.SHOW_ENTER_ROOM_BUTTON, null);
    }

    /// <summary>
    /// 别人进入房间的广播
    /// </summary>
    /// <param name="user"></param>
    private void EnterBro(UserDto user)
    {
        //更新房间数据
        Models.GameModel.MatchRoomDto.Add(user);

        ResetPosition();

        //发送现在房间的玩家信息
        if (Models.GameModel.MatchRoomDto.LeftId != -1)
        {
            UserDto leftUserDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.MatchRoomDto.LeftId];
            Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        }
        if (Models.GameModel.MatchRoomDto.RightId != -1)
        {
            UserDto rightUserDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.MatchRoomDto.RightId];
            Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightUserDto);
        }

        //发消息,显示玩家状态面板所有物体
        Dispatch(AreaCode.UI, UIEvent.PLAYER_ENTER, user.Id);

        // 提示
        promptMsg.Change("玩家" + user.Name + "进入", UnityEngine.Color.green);
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
    }

    /// <summary>
    /// 重置位置,更新玩家显示
    /// </summary>
    private void ResetPosition()
    {
        GameModel gModel = Models.GameModel;
        MatchRoomDto matchRoom = gModel.MatchRoomDto;
        //重置一下玩家位置
        matchRoom.ResetPosition(gModel.UserDto.Id);

        ////再次发送现在房间的玩家信息
        //if (matchRoom.LeftId != -1)
        //{
        //    UserDto leftUserDto = matchRoom.UIdUserDict[matchRoom.LeftId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        //}
        //if (matchRoom.RightId != -1)
        //{
        //    UserDto rightUserDto = matchRoom.UIdUserDict[matchRoom.RightId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightUserDto);
        //}
    }

    /// <summary>
    /// 离开的广播
    /// </summary>
    private void LeaveBro(int leaveUserId)
    {
        //发消息,隐藏玩家状态面板所有物体
        Dispatch(AreaCode.UI, UIEvent.PLAYER_LEAVE, leaveUserId);

        ResetPosition();

        //移除数据
        Models.GameModel.MatchRoomDto.Leave(leaveUserId);
    }

    /// <summary>
    /// 准备的广播
    /// </summary>
    private void ReadyBro(int readyUserId)
    {
        //保存数据
        Models.GameModel.MatchRoomDto.Ready(readyUserId);
        //更新玩家准备状态
        Dispatch(AreaCode.UI, UIEvent.PLAYER_READY, readyUserId);

        if(readyUserId==Models.GameModel.UserDto.Id)
        {
            //发送消息,隐藏准备按钮
            Dispatch(AreaCode.UI, UIEvent.PLAYER_HIDE_READY_BUTTON, null);
        }
    }

    /// <summary>
    /// 开始游戏的广播
    /// </summary>
    private void StartBro()
    {
        promptMsg.Change("开始游戏", UnityEngine.Color.blue);
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        //开始游戏,隐藏状态面板的准备文字
        Dispatch(AreaCode.UI, UIEvent.PLAYER_HIDE_STATE, null);
    }
}
