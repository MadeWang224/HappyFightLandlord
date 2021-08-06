using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudio : AudioBase
{
    private void Awake()
    {
        Bind(AudioEvent.PLAYER_EFFECT_AUDIO);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAYER_EFFECT_AUDIO:
                PlayEffectAudio(message.ToString());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 播放音乐的组件
    /// </summary>
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="assetName"></param>
    private void PlayEffectAudio(string assetName)
    {
        AudioClip ac = Resources.Load<AudioClip>("Sound/"+assetName);
        audioSource.clip = ac;
        audioSource.Play();
    }
}
