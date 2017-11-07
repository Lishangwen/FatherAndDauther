using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AudioPlayerType
{
    Bgm,
    Sound,
}

public class AudioManager : MonoBehaviour
{
    private AudioPlayerType _defaulPlayerType = AudioPlayerType.Bgm;
    private static AudioManager instance;

    private AudioPlayerController [] audioCtrl = new AudioPlayerController[Enum.GetNames(typeof(AudioPlayerType)).Length];

    private void SetController(AudioPlayerType PlayerType, AudioPlayerController ctrl)
    {
        instance.audioCtrl[(int) PlayerType] = ctrl;
        ctrl.transform.parent = transform;
    }




    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public string GetAudioName(AudioPlayerType type)
    {
        return audioCtrl[(int)type].AudioClipName;
    }


    public void SetVolume(AudioPlayerType type, float volume)
    {
        audioCtrl[(int) type].Volume = volume;
    }

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("AudioManager").AddComponent<AudioManager>();

                instance.SetController(AudioPlayerType.Sound, new GameObject("SoundPlayer").AddComponent<AudioPlayerController>());
                instance.SetController(AudioPlayerType.Bgm, new GameObject("BgmPlayer").AddComponent<AudioPlayerController>());
            }
            return instance;
        }
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        audioCtrl[(int)AudioPlayerType.Sound].PlayOnShot(audioClip);
    }

    public void Play(AudioPlayerType PlayerType, AudioClip audioClip, bool loop)
    {
        audioCtrl[(int)PlayerType].Play(audioClip, loop);
    }

    public void PlayBgmTurnUp(AudioPlayerType PlayerType, AudioClip audioClip, float f, UnityAction action, float upMinVolume, AudioPlayMode mode)
    {
        audioCtrl[(int)PlayerType].PlayBgmTurnUp(audioClip, f, action, upMinVolume, mode);
    }

    public void SlowDown(AudioPlayerType PlayerType, float f, UnityAction action, float downMinVolume)
    {
        audioCtrl[(int)PlayerType].SlowDown(f, action, downMinVolume);
    }

    public void Stop(AudioPlayerType PlayerType)
    {
        audioCtrl[(int)PlayerType].Stop();
    }
}
