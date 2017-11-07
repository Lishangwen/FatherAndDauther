using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BGM
{
    START=0,
    BGM1_1,
    BGM1_2,
    BGM2,
    BGM5_1,
    BGM5_2,
    BGM4,
    BGM_NIGHT
}
public class BgmCtrl : MonoBehaviour 
{
    public AudioClip[] dayAudios;
    public AudioClip[] nightAudios;
    public AudioClip[] Bgm;
    static BGM curAudio;
	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
        EventManager.addEventListener(EventType.DayNightChange, OnUpdateDayNight);
        EventManager.addEventListener(EventType.PlayBGM, OnPlayBGM);
        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainSwitch);
        //PlayAudio(dayAudios[0]);
	}
	
    public static void PlayBGM(BGM bgm)
    {
       
        //OnPlayBGM(data);
        EventManager.dispatchEvent(EventType.PlayBGM, (int)bgm);
        curAudio = bgm;
    }
    void OnCurtainSwitch(EventData data)
    {
        int curtainIndex = (int)data.data;
        if (curtainIndex == 2 || curtainIndex == 3)
        {
            PlayBGM(BGM.BGM2);
        }
        else if(curtainIndex==4)
        {
            PlayBGM(BGM.BGM4);
        }
        else if(curtainIndex==5)
        {
            PlayBGM(BGM.BGM5_1);
        }
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
        if(state==SkyState.Night)
        {
            PlayBGM(BGM.BGM_NIGHT);
        }
        
    }
	// Update is called once per frame
	void Update () 
    {
        if (curAudio == BGM.BGM1_2&&!GetComponent<AudioSource>().isPlaying)
        {
            PlayBGM(BGM.BGM1_1);
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
        if(curAudio==BGM.BGM1_2)
        {
            GetComponent<AudioSource>().loop = false;
        }
        else
        {
            GetComponent<AudioSource>().loop = true;
        }
        
    }
}
