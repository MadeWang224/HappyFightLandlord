using Protocol.Dto;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePanel : UIBase
{
    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_ENTER);
        Bind(UIEvent.PLAYER_HIDE_STATE);
        Bind(UIEvent.PLAYER_READY);
        Bind(UIEvent.PLAYER_LEAVE);
        Bind(UIEvent.PLAYER_CHAT);
        Bind(UIEvent.PLAYER_CHANGE_IDENTITY);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PLAYER_READY:
                {
                    int userId = (int)message;
                    if (userDto == null)
                        break;
                    //如果是自身角色,就显示准备
                    if (userDto.Id == userId)
                        ReadyState();
                }
                break;
            case UIEvent.PLAYER_HIDE_STATE:
                txtReady.gameObject.SetActive(false);
                break;
            case UIEvent.PLAYER_LEAVE:
                {
                    int userId = (int)message;
                    if (userDto == null)
                        break;
                    if (userDto.Id == userId)
                        setPanelActive(false);
                }
                break;
            case UIEvent.PLAYER_ENTER:
                {
                    int userId = (int)message;
                    if (userDto == null)
                        break;
                    if (userDto.Id == userId)
                        setPanelActive(true);
                }
                break;
            case UIEvent.PLAYER_CHAT:
                {
                    if (userDto == null)
                        break;
                    ChatMsg msg = message as ChatMsg;
                    if (userDto.Id == msg.UserId)
                        ShowChat(msg.Text);
                }
                break;
            case UIEvent.PLAYER_CHANGE_IDENTITY:
                {
                    if (userDto == null)
                        break;
                    int userId = (int)message;
                    if (userDto.Id == userId)
                        SetIdentity(1);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 角色的数据
    /// </summary>
    protected UserDto userDto;

    protected Image imgIdentity;
    protected Text txtReady;
    protected Image imgChat;
    protected Text txtChat;

    protected virtual void Start()
    {
        imgIdentity = transform.Find("imgIdentity").GetComponent<Image>();
        txtReady = transform.Find("txtReady").GetComponent<Text>();
        imgChat = transform.Find("imgChat").GetComponent<Image>();
        txtChat = transform.Find("imgChat/Text").GetComponent<Text>();

        //默认状态
        txtReady.gameObject.SetActive(false);
        imgChat.gameObject.SetActive(false);
        SetIdentity(0);
    }

    
    protected virtual void ReadyState()
    {
        txtReady.gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置身份:0农民;1地主
    /// </summary>
    protected void SetIdentity(int identity)
    {
        string identityStr = identity == 0 ? "Farmer" : "Landlord";
        imgIdentity.sprite = Resources.Load<Sprite>("Identity/" + identityStr);
    }

    /// <summary>
    /// 显示时间
    /// </summary>
    protected int ShowTime = 3;
    /// <summary>
    /// 计时器
    /// </summary>
    protected float timer = 0f;
    /// <summary>
    /// 是否显示
    /// </summary>
    protected bool isShow = false;

    protected virtual void Update()
    {
        if(isShow==true)
        {
            timer += Time.deltaTime;
            if(timer>=ShowTime)
            {
                SetChatActive(false);
                timer = 0f;
                isShow = false;
            }    
        }
    }

    /// <summary>
    /// 快速聊天显示
    /// </summary>
    /// <param name="active"></param>
    protected void SetChatActive(bool active)
    {
        imgChat.gameObject.SetActive(active);
    }

    /// <summary>
    /// 显示聊天内容
    /// </summary>
    /// <param name="text">聊天内容</param>
    protected void ShowChat(string text)
    {
        txtChat.text = text;
        //显示
        SetChatActive(true);
        isShow = true;
    }
}
