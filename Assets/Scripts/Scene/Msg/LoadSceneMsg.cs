using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LoadSceneMsg
{
    /// <summary>
    /// 索引
    /// </summary>
    public int SceneBuildIndex;
    /// <summary>
    /// 名字
    /// </summary>
    public string SceneBuildName;
    /// <summary>
    /// 委托
    /// </summary>
    public Action OnSceneLoaded;

    public LoadSceneMsg()
    {
        this.SceneBuildIndex = -1;
        this.SceneBuildName = null;
        this.OnSceneLoaded = null;
    }

    public LoadSceneMsg(string name,Action loaded)
    {
        this.SceneBuildIndex = -1;
        this.SceneBuildName = name;
        this.OnSceneLoaded = loaded;
    }

    public LoadSceneMsg(int index, Action loaded)
    {
        this.SceneBuildIndex = index;
        this.SceneBuildName = null;
        this.OnSceneLoaded = loaded;
    }
}
