using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr:ManagerBase
{
    public static SceneMgr Instance = null;
    private void Awake()
    {
        Instance = this;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        Add(SceneEvent.LOAD_SCENE, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg msg = message as LoadSceneMsg;
                LoadScene(msg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 临时变量
    /// </summary>
    private Action OnSceneLoaded = null;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneBuildIndex"></param>
    private void LoadScene(LoadSceneMsg msg)
    {
        if (msg.SceneBuildIndex != -1)
            SceneManager.LoadScene(msg.SceneBuildIndex);
        if (msg.SceneBuildName != null)
            SceneManager.LoadScene(msg.SceneBuildName);

        if (msg.OnSceneLoaded != null)
            OnSceneLoaded = msg.OnSceneLoaded;
    }

    /// <summary>
    /// 当场景加载完成时候调用
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (OnSceneLoaded != null)
        {
            OnSceneLoaded();
            OnSceneLoaded = null;
        }
    }
}
