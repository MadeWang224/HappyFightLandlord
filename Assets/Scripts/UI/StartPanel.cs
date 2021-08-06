using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.START_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.START_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnLogin;
	private Button btnClose;
	private InputField inputAccount;
	private InputField inputPassword;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    private void Start()
    {
        btnLogin = transform.Find("btnLogin").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();

        btnLogin.onClick.AddListener(LoginClick);
        btnClose.onClick.AddListener(CloseClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();

        //面板需要默认隐藏
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }

    private void LoginClick()
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
        //和服务器交互
        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    private void CloseClick()
    {
        setPanelActive(false);
    }
}
