using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopEffectAudioCtrl : MonoBehaviour 
{

    public AudioClip[] Bgm;
    static Effect curAudio;
    static bool isStop=true;
    //AudioSource curSource;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        EventManager.addEventListener(EventType.DayNightChange, OnUpdateDayNight);
        EventManager.addEventListener(EventType.PlayLoopEffect, OnPlayBGM);
        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainSwitch);
    }

    public static void PlayLoopEffect(Effect bgm)
    {

        //OnPlayBGM(data);
        EventManager.dispatchEvent(EventType.PlayLoopEffect, (int)bgm);
        curAudio = bgm;
        isStop = false;
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
        if(isStop)
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
        GetComponent<AudioSource>().loop = true;
    }
}
