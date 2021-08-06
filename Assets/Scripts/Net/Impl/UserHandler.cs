using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 角色网络消息处理类
/// </summary>
class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.GET_INFO_SRES:
                GetInfoResponse(value as UserDto);
                break;
            case UserCode.CREATE_SRES:
                CreateResponse((int)value);
                break;
            case UserCode.ONLINE_SRES:
                OnlineResponse((int)value);
                break;
            default:
                break;
        }
    }

    private SocketMsg socketMsg = new SocketMsg();

    /// <summary>
    /// 获取信息的回应
    /// </summary>
    private void GetInfoResponse(UserDto user)
    {
        if(user==null)
        {
            //没有角色
            //显示创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, true);
        }
        else
        {
            //有角色
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            ////角色上线
            //socketMsg.Change(OpCode.USER, UserCode.ONLINE_CREQ, null);
            //Dispatch(AreaCode.NET, 0, socketMsg);
            //保存服务器发来的角色数据
            Models.GameModel.UserDto = user;

            //更新显示
            Dispatch(AreaCode.UI, UIEvent.REFRESH_INFO_PANEL, user);
        }
    }

    /// <summary>
    /// 上线的响应
    /// </summary>
    /// <param name="reault"></param>
    private void OnlineResponse(int result)
    {
        if(result==0)
        {
            //上线成功
        }
        else if(result==-1)
        {
            //非法登录
            Debug.LogError("非法登录");
        }
        else if(result==-2)
        {
            //没有角色,不能上线
            Debug.LogError("没有角色");
        }
    }

    /// <summary>
    /// 创建角色的响应
    /// </summary>
    /// <param name="reault"></param>
    private void CreateResponse(int result)
    {
        if(result==-1)
        {
            Debug.LogError("非法登录");
        }
        else if(result==-2)
        {
            Debug.LogError("已有角色");
        }
        else if(result==0)
        {
            //创建成功
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            //获取角色信息
            socketMsg.Change(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
    }
}
