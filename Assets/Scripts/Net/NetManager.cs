using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 网络模块
/// </summary>
public class NetManager:ManagerBase
{
    public static NetManager Instance = null;

    #region 处理客户端内部给服务器发消息的事件
    private void Awake()
    {
        Instance = this;

        Add(0, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
            default:
                break;
        }
    } 
    #endregion

    private void Start()
    {
        client.Connect();
    }

    private ClientPeer client = new ClientPeer("127.0.0.1", 6666);

    public void Connected(string ip,int port)
    {
        client = new ClientPeer(ip, port);
    }

    private void Update()
    {
        if (client == null)
            return;

        while(client.SocketMsgQueue.Count>0)
        {
            SocketMsg msg = client.SocketMsgQueue.Dequeue();
            //处理信息
            ProcessSocketMsg(msg);
        }
    }

    #region 处理接收的服务器发来的信息

    HandlerBase accountHandler = new AccountHandler();
    HandlerBase userHandler = new UserHandler();
    HandlerBase matchHandler = new MatchHandler();
    HandlerBase chatHandler = new ChatHandler();
    HandlerBase fightHandler = new FightHandler();

    /// <summary>
    /// 处理接收的网络信息
    /// </summary>
    private void ProcessSocketMsg(SocketMsg msg)
    {
        switch (msg.OpCode)
        {
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.USER:
                userHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.MATCH:
                matchHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.CHAT:
                chatHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.FIGHT:
                fightHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            default:
                break;
        }
    }

    #endregion
}
