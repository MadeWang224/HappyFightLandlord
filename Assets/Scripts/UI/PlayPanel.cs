using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIBase
{
	private Button btnStart;

	private Button btnRegist;

    private void Start()
    {
        btnStart = transform.Find("btnStart").GetComponent<Button>();
        btnRegist = transform.Find("btnRegist").GetComponent<Button>();

        btnStart.onClick.AddListener(StartClick);
        btnRegist.onClick.AddListener(RegistClick);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnStart.onClick.RemoveAllListeners();
        btnRegist.onClick.RemoveAllListeners();
    }

    private void StartClick()
    {
        Dispatch(AreaCode.UI, UIEvent.START_PANEL_ACTIVE, true);
    }

    private void RegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, true);
    }
}
