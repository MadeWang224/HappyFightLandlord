using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtomPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.CHANGE_MUTIPLE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CHANGE_MUTIPLE:
                ChangeMutiple((int)message);
                break;
            default:
                break;
        }
    }

    private Text txtBeen;
    private Text txtMutiple;
    private Button btnChat;
    private Image imgChoose;
    private Button[] btns;

    private SocketMsg socketMsg;

    private void Start()
    {
        txtBeen = transform.Find("txtBeen").GetComponent<Text>();
        txtMutiple = transform.Find("txtMutiple").GetComponent<Text>();
        btnChat = transform.Find("btnChat").GetComponent<Button>();
        btns = new Button[7];
        imgChoose = transform.Find("imgChoose").GetComponent<Image>();
        for (int i = 0; i < 7; i++)
        {
            int j = i;
            btns[i] = imgChoose.transform.GetChild(i).GetComponent<Button>();
            btns[i].onClick.AddListener(() =>
            {
                ChatClick(j + 1);
            });
        }

        btnChat.onClick.AddListener(SetChooseActive);

        socketMsg = new SocketMsg();

        //默认
        imgChoose.gameObject.SetActive(false);

        RefreshPanel(Models.GameModel.UserDto.Been);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnChat.onClick.RemoveAllListeners();
        for (int i = 0; i < 7; i++)
        {
            btns[i].onClick.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 刷新自身面板信息
    /// </summary>
    private void RefreshPanel(int beenCount)
    {
        this.txtBeen.text = "× " + beenCount;
    }

    /// <summary>
    /// 改变倍数
    /// </summary>
    /// <param name="muti"></param>
    private void ChangeMutiple(int mutiple)
    {
        txtMutiple.text = "倍数 × " + mutiple;
    }

    /// <summary>
    /// 设置选择对话的面板显示
    /// </summary>
    private void SetChooseActive()
    {
        bool active = imgChoose.gameObject.activeInHierarchy;
        imgChoose.gameObject.SetActive(!active);
    }

    /// <summary>
    /// 点击某句聊天内容时调用
    /// </summary>
    /// <param name="chatType"></param>
    private void ChatClick(int chatType)
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, chatType);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}
