using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAudio : AudioBase
{
    private void Awake()
    {
        Bind(AudioEvent.PLAYER_BG_AUDIO,
            AudioEvent.SET_BG_VOLUME,
            AudioEvent.STOP_BG_AUDIO);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAYER_BG_AUDIO:
                PlayAudio();
                break;
            case AudioEvent.SET_BG_VOLUME:
                SetVolume((float)message);
                break;
            case AudioEvent.STOP_BG_AUDIO:
                StopAudio();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 播放的声音源
    /// </summary>
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }

    private void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    private void StopAudio()
    {
        audioSource.Pause();
    }
}
