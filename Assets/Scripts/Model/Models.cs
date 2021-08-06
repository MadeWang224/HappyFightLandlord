using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Models
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    public static GameModel GameModel;
    static Models()
    {
        GameModel = new GameModel();
    }
}

