using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioEvent
{
    /// <summary>
    /// 播放音效文件
    /// </summary>
    public const int PLAYER_EFFECT_AUDIO = 100;
    /// <summary>
    /// 播放音乐文件
    /// </summary>
    public const int PLAYER_BACKGROUND_AUDIO = 101;

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public const int PLAYER_BG_AUDIO = 102;
    /// <summary>
    /// 设置背景音量
    /// </summary>
    public const int SET_BG_VOLUME = 103;
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public const int STOP_BG_AUDIO = 104;
}