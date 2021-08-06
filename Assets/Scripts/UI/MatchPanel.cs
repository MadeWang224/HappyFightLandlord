using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : UIBase
{
    private void Awake()
    {
		Bind(UIEvent.SHOW_ENTER_ROOM_BUTTON);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
			case UIEvent.SHOW_ENTER_ROOM_BUTTON:
				btnEnter.gameObject.SetActive(true);
				break;
            default:
                break;
        }
    }

    private Button btnMatch;
	private Image imgBg;
	private Text txtDes;
	private Button btnCancel;
	private Button btnEnter;

	private SocketMsg socketMsg;

    private void Start()
    {
		btnMatch = transform.Find("btnMatch").GetComponent<Button>();
		imgBg = transform.Find("imgBg").GetComponent<Image>();
		txtDes = transform.Find("txtDes").GetComponent<Text>();
		btnCancel = transform.Find("btnCancel").GetComponent<Button>();
		btnEnter = transform.Find("btnEnter").GetComponent<Button>();

		btnMatch.onClick.AddListener(MatchClick);
		btnCancel.onClick.AddListener(CancelClick);
		btnEnter.onClick.AddListener(EnterClick);

		socketMsg = new SocketMsg();

		//默认状态
		SetObjectActive(false);
		btnEnter.gameObject.SetActive(false);
	}

    private void Update()
    {
		if (txtDes.gameObject.activeInHierarchy == false)
			return;
		timer += Time.deltaTime;
		if(timer>=intervalTime)
        {
			DoAnimation();
			timer = 0f;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

		btnMatch.onClick.RemoveAllListeners();
		btnCancel.onClick.RemoveAllListeners();
		btnEnter.onClick.RemoveAllListeners();
    }

	/// <summary>
	/// 进入房间按钮点击事件
	/// </summary>
	private void EnterClick()
    {
		Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, new LoadSceneMsg(2,null));
    }

	/// <summary>
	///  匹配按钮点击事件
	/// </summary>
    private void MatchClick()
    {
		//向服务器发起匹配的请求 
		socketMsg.Change(OpCode.MATCH, MatchCode.ENTER_CREQ, null);
		Dispatch(AreaCode.NET, 0, socketMsg);

		SetObjectActive(true);

		//按钮隐藏
		this.btnMatch.interactable = false;
    }

	/// <summary>
	///  取消按钮点击事件
	/// </summary>
	private void  CancelClick()
    {
		//向服务器发起离开匹配的请求 
		socketMsg.Change(OpCode.MATCH, MatchCode.LEAVE_CREQ, null);
		Dispatch(AreaCode.NET, 0, socketMsg);

		SetObjectActive(false);

		this.btnMatch.interactable = true;

	}

	/// <summary>
	/// 控制点击匹配按钮之后的显示的物体
	/// </summary>
	/// <param name="active"></param>
	private void SetObjectActive(bool active)
    {
		imgBg.gameObject.SetActive(active);
		txtDes.gameObject.SetActive(active);
		btnCancel.gameObject.SetActive(active);
    }

	private string defaultText = "正在寻找房间";
	private int dotCount = 0;
	/// <summary>
	///  动画间隔
	/// </summary>
	private float intervalTime = 1f;
	/// <summary>
	/// 计时器
	/// </summary>
	private float timer = 0f;

	/// <summary>
	/// 文字动画
	/// </summary>
	private void DoAnimation()
    {
		txtDes.text = defaultText;
		//点增加
		dotCount++;
		if (dotCount > 5)
			dotCount = 1;
        for (int i = 0; i < dotCount; i++)
        {
			txtDes.text += ".";
        }
    }
}
