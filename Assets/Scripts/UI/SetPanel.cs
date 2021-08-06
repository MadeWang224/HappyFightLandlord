using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : UIBase
{
	private Button btnSet;
	private Image imgBg;
	private Button btnClose;
	private Text txtAudio;
	private Toggle togAudio;
	private Text txtVolume;
	private Slider sldVolume;
	private Button btnQuit;

    private void Start()
    {
		btnSet = transform.Find("btnSet").GetComponent<Button>();
		imgBg = transform.Find("imgBg").GetComponent<Image>();
		btnClose = transform.Find("btnClose").GetComponent<Button>();
		txtAudio = transform.Find("txtAudio").GetComponent<Text>();
		togAudio = transform.Find("togAudio").GetComponent<Toggle>();
		txtVolume = transform.Find("txtVolume").GetComponent<Text>();
		sldVolume = transform.Find("sldVolume").GetComponent<Slider>();
		btnQuit = transform.Find("btnQuit").GetComponent<Button>();

		btnSet.onClick.AddListener(SetClick);
		btnClose.onClick.AddListener(CloseClick);
		btnQuit.onClick.AddListener(QuitClick);
		togAudio.onValueChanged.AddListener(TogAudio_onValueChanged);
		sldVolume.onValueChanged.AddListener(SldVolume_onValueChanged);

		//默认状态
		SetObjectActive(false);
	}

    public override void OnDestroy()
    {
        base.OnDestroy();

		btnSet.onClick.RemoveAllListeners();
		btnClose.onClick.RemoveAllListeners();
		btnQuit.onClick.RemoveAllListeners();
		togAudio.onValueChanged.RemoveAllListeners();
		sldVolume.onValueChanged.RemoveAllListeners();
    }

    private void SetObjectActive(bool active)
    {
		imgBg.gameObject.SetActive(active);
		btnClose.gameObject.SetActive(active);
		txtAudio.gameObject.SetActive(active);
		togAudio.gameObject.SetActive(active);
		txtVolume.gameObject.SetActive(active);
		sldVolume.gameObject.SetActive(active);
		btnQuit.gameObject.SetActive(active);
	}

	/// <summary>
	/// 设置按钮点击事件
	/// </summary>
	private void SetClick()
    {
		SetObjectActive(true);
    }

	/// <summary>
	/// 关闭按钮点击事件
	/// </summary>
	private void CloseClick()
	{
		SetObjectActive(false);
	}

	/// <summary>
	/// 退出按钮的点击事件
	/// </summary>
	private void QuitClick()
    {
		Application.Quit();
    }

	/// <summary>
	/// 开关的点击事件
	/// </summary>
	/// <param name="result"></param>
	private void TogAudio_onValueChanged(bool value)
    {
		if(value==true)
        {
			Dispatch(AreaCode.AUDIO, AudioEvent.PLAYER_BG_AUDIO, null);
        }
		else
        {
			Dispatch(AreaCode.AUDIO, AudioEvent.STOP_BG_AUDIO, null);
		}
	}

	/// <summary>
	/// 滑动条滑动时触发的事件
	/// </summary>
	/// <param name="value"></param>
	private void SldVolume_onValueChanged(float value)
    {
		Dispatch(AreaCode.AUDIO, AudioEvent.SET_BG_VOLUME, value);
	}
}
