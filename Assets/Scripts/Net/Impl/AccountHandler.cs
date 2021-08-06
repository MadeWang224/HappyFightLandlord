using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AccountHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.LOGIN:
                LoginResponse((int)value);
                break;
            case AccountCode.REGIST_SRES:
                RegistResponse((int)value);
                break;
            default:
                break;
        }
    }

    private PromptMsg promptMsg = new PromptMsg();

    /// <summary>
    /// 登录响应
    /// </summary>
    private void LoginResponse(int result)
    {
        switch (result)
        {
            case 0:
                //跳转场景
                LoadSceneMsg msg = new LoadSceneMsg(1,()=> 
                {
                    //向服务器获取信息
                    SocketMsg socketMsg = new SocketMsg(OpCode.USER, UserCode.GET_INFO_CREQ, null);
                    Dispatch(AreaCode.NET, 0, socketMsg);
                });
                Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
                break;
            case -1:
                promptMsg.Change("账号不存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("账号在线", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("账号密码错误", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 注册响应
    /// </summary>
    /// <param name="result"></param>
    private void RegistResponse(int result)
    {
        switch (result)
        {
            case 0:
                promptMsg.Change("注册成功", Color.green);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -1:
                promptMsg.Change("账号已存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("账号不能为空", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("密码不合法", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }
    }
}
