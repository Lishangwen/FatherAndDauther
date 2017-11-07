using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect
{
    START = 0,
    BGM1_1,
    BGM1_2,
    BGM2,
    BGM5_1,
    BGM5_2,
    BGM4,
    BGM_NIGHT
}
public class EffectAudioCtrl : MonoBehaviour 
{

    public AudioClip[] Bgm;
    static Effect curAudio;
    static bool isStop;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        EventManager.addEventListener(EventType.DayNightChange, OnUpdateDayNight);
        EventManager.addEventListener(EventType.PlayEffect, OnPlayBGM);
        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainSwitch);
    }

    public static void PlayEffect(Effect bgm)
    {

        //OnPlayBGM(data);
        EventManager.dispatchEvent(EventType.PlayEffect, (int)bgm);
        curAudio = bgm;
    }
    public static void Stop()
    {
        isStop = true;
    }
    void OnCurtainSwitch(EventData data)
    {
        int curtainIndex = (int)data.data;
        
    }
    void OnPlayBGM(EventData dat)
    {
        int bgmIndex = (int)dat.data;
        PlayAudio(Bgm[bgmIndex]);
    }
    void PlayAudio(AudioClip audioClip)
    {
        StartCoroutine(AsyncPlayAudio(audioClip));
    }

    void OnUpdateDayNight(EventData data)
    {

        SkyState state = (SkyState)data.data;
        if (state == SkyState.Night)
        {
            
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (isStop)
        {
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
        if (curAudio == Effect.BGM1_2 && !GetComponent<AudioSource>().isPlaying)
        {
            
        }
    }
    public IEnumerator AsyncPlayAudio(AudioClip audio)
    {
        yield return new WaitForSeconds(0);
        if (GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Stop();
        }

        GetComponent<AudioSource>().clip = audio;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = false;
    }
}
