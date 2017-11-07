using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum AudioPlayMode
{
    Loop,
    Once,
    OneShot,
    Repeat
}

public class AudioPlayer : BaseAudioPlayer
{
    [HideInInspector]
    public bool playOnAwake;
    [HideInInspector]
    public AudioPlayMode playMode = AudioPlayMode.Loop;
    [HideInInspector]
    public bool slowdown;
    [HideInInspector]
    public float slowSpeed = 1f;
    [HideInInspector]
    public float downMinVolume = 0.001f;
    [HideInInspector]
    public bool turnup;
    [HideInInspector]
    public float upSpeed = 1f;
    [HideInInspector] public float upMinVolume = 0.001f;
    [HideInInspector] public float repeatRate = 1;
    [HideInInspector] public int repeatTimes = 1;
    private int repeatCount = 0;
    [HideInInspector]
    public bool useEvent = false;

    public UnityEvent stopEvent;

    private void OnDisable()
    {
        Stop();
    }

    public void Play()
    {
        switch (playMode)
        {
            case AudioPlayMode.Loop:
                AudioManager.Instance.Play(PlayerType, audioClip, true);
                break;
            case AudioPlayMode.OneShot:
                AudioManager.Instance.PlayOneShot(audioClip);
                break;
            case AudioPlayMode.Repeat:
                repeatCount = 0;

                InvokeRepeating("RepeatPlay", 0, repeatRate);
                break;
        }
    }

    private void RepeatPlay()
    {
        AudioManager.Instance.PlayOneShot(audioClip);

        if (repeatTimes > 0)
        {
            repeatCount++;
            if (repeatCount >= repeatTimes)
            {
                CancelInvoke("RepeatPlay");
            }
        }
    }

    public void SlowDown()
    {
        AudioManager.Instance.SlowDown(PlayerType, slowSpeed, () => { stopEvent.Invoke(); }, downMinVolume);
    }

    public void PlayTurnUp()
    {
        AudioManager.Instance.PlayBgmTurnUp(PlayerType, audioClip, upSpeed, null, upMinVolume, AudioPlayMode.Loop);
    }

    public void Stop()
    {
        AudioManager.Instance.Stop(PlayerType);
        stopEvent.Invoke();
        if (playMode == AudioPlayMode.Repeat) CancelInvoke("RepeatPlay");
    }


    public override void PlayAudio(EventData data)
    {
        if (compEventData)
        {
            if (data.data.ToString() != eventData) return;
        }

        if (turnup)
        {
            PlayTurnUp();
        }
        else
        {
            Play();
        }
    }

    public override void StopAudio(EventData data)
    {
        if (compEventData)
        {
            if (data.data.ToString() != eventData) return;
        }

        if (slowdown)
        {
            SlowDown();
        }
        else
        {
            Stop();
        }

        
    }

    public override void OnAwake()
    {
        if (playOnAwake)
        {
            if (turnup)
            {
                PlayTurnUp();
            }
            else
            {
                Play();
            }
        }

    }
}
