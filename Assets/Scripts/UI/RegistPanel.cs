using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnRegist;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;
    private InputField inputRepeat;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    private void Start()
    {
        btnRegist = transform.Find("btnRegist").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();
        inputRepeat = transform.Find("inputRepeat").GetComponent<InputField>();

        btnRegist.onClick.AddListener(RegistClick);
        btnClose.onClick.AddListener(CloseClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();

        //面板需要默认隐藏
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnRegist.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }

    private void RegistClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("用户名不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text)
            || inputPassword.text.Length < 4
            || inputPassword.text.Length > 16)
        {
            promptMsg.Change("密码不合法", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputRepeat.text)
            || inputRepeat.text != inputPassword.text)
        {
            promptMsg.Change("两次输入的密码不一致", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        //和服务器交互
        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    private void CloseClick()
    {
        setPanelActive(false);
    }
}
