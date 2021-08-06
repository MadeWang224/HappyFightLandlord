using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 存储所有的UI事件码
/// </summary>
public class UIEvent
{
    /// <summary>
    /// 设置开始面板的显示
    /// </summary>
    public const int START_PANEL_ACTIVE = 0;
    /// <summary>
    /// 设置注册面板的显示
    /// </summary>
    public const int REGIST_PANEL_ACTIVE = 1;
    /// <summary>
    /// 刷新信息面板
    /// </summary>
    public const int REFRESH_INFO_PANEL = 2;
    /// <summary>
    /// 显示进入房间按钮
    /// </summary>
    public const int SHOW_ENTER_ROOM_BUTTON = 3;
    /// <summary>
    /// 设置创建面板的显示
    /// </summary>
    public const int CREATE_PANEL_ACTIVE = 4;
    /// <summary>
    /// 设置底牌
    /// </summary>
    public const int SET_TABLE_CARDS = 5;
    /// <summary>
    /// 设置左边的角色数据
    /// </summary>
    public const int SET_LEFT_PLAYER_DATA = 6;
    /// <summary>
    /// 设置右边的角色数据
    /// </summary>
    public const int SET_RIGHT_PLAYER_DATA = 13;
    /// <summary>
    /// 设置自身角色数据
    /// </summary>
    //public const int SET_MY_PLAYER_DATA = 16;


    /// <summary>
    /// 角色准备
    /// </summary>
    public const int PLAYER_READY = 7;
    /// <summary>
    /// 角色进入
    /// </summary>
    public const int PLAYER_ENTER = 8;
    /// <summary>
    /// 角色离开
    /// </summary>
    public const int PLAYER_LEAVE = 9;
    /// <summary>
    /// 角色聊天
    /// </summary>
    public const int PLAYER_CHAT = 10;
    /// <summary>
    /// 角色改变身份
    /// </summary>
    public const int PLAYER_CHANGE_IDENTITY = 11;
    /// <summary>
    /// 开始游戏,隐藏角色状态
    /// </summary>
    public const int PLAYER_HIDE_STATE = 12;
    /// <summary>
    /// 已准备,隐藏准备按钮
    /// </summary>
    public const int PLAYER_HIDE_READY_BUTTON = 17;

    /// <summary>
    /// 开始抢地主,显示按钮
    /// </summary>
    public const int SHOW_GRAB_BUTTON = 14;
    /// <summary>
    /// 开始出牌,显示按钮
    /// </summary>
    public const int SHOW_DEAL_BUTTON = 15;

    /// <summary>
    /// 改变倍数
    /// </summary>
    public const int CHANGE_MUTIPLE = 18;

    /// <summary>
    /// 显示结束面板
    /// </summary>
    public const int SHOW_OVER_PANEL = 19;

    /// <summary>
    /// 提示信息面板
    /// </summary>
    public const int PROMPT_MSG = int.MaxValue;
}
