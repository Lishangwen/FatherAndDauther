using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerManager : MonoBehaviour {

    private string[] textArray = { "", "离\n\n别", "成\n\n长", "挫\n\n折", "成\n\n家", "迟\n\n暮" };

    private int currCurtainIndex;
    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField]
    private AudioClip[] soundClips;

    private AudioClip currCurtainBgm;

    private bool isInHome = false;

    private SkyState skyState = SkyState.Day;

    private float dumpSpeed = 30;

    public static AudioPlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundById(int id, bool isLoop)
    {
        AudioManager.Instance.Play(AudioPlayerType.Sound, soundClips[id], isLoop);
    }


    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(gameObject);

        AudioManager.Instance.Play(AudioPlayerType.Bgm, bgmClips[0], true);

        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainChanged);
        EventManager.addEventListener(EventType.StartHugGirl, OnClickFather);
        EventManager.addEventListener(EventType.DayNightChange, OnDayNightChanged);
        EventManager.addEventListener(EventType.EnterHome, OnEnterHome);
        EventManager.addEventListener(EventType.LeaveHome, OnLeaveHome);
        EventManager.addEventListener(EventType.TitleComplete, OnTitleCompelte);
        EventManager.addEventListener(EventType.SwitchBgm, OnSwitchBgm);
        EventManager.addEventListener(EventType.ClickMailBox, OnClickMailBox);
	}


    private void OnClickMailBox(EventData data)
    {
        if (SceneMgr.curtainIndex >= 2 && SceneMgr.curtainIndex <= 4)
        {
            AudioManager.Instance.SetVolume(AudioPlayerType.Sound, 0.7f);
            AudioManager.Instance.PlayOneShot(soundClips[3]);
        }
    }

    private void OnSwitchBgm(EventData e)
    {
        SwitchToBgm((int)e.data);
    }


    private void OnTitleCompelte(EventData e)
    {
        SwitchToBgm(1);
    }


    private void OnEnterHome(EventData e)
    {
        isInHome = true;

        Debug.Log("进入房间");
        CancelInvoke("PlayWolfSoundRepeat");

        if (skyState == SkyState.Night)
        {
            AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
            {
                Debug.Log("播放夜晚房屋音效" + bgmClips[8].name);
                AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, bgmClips[8], dumpSpeed * 2, null, 0.01f, AudioPlayMode.Loop);
            }, 0.01f);
        }
    }

    public void PlaySoundById(int id)
    {
        AudioManager.Instance.PlayOneShot(soundClips[id]);
    }

    private void OnLeaveHome(EventData e)
    {
        isInHome = false;

        Debug.Log("离开房间");
        if (skyState == SkyState.Night)
        {
            AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
            {
                PlayWolfSound();
                Debug.Log("播放夜晚音效");
                AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, bgmClips[7], dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
            }, 0.01f);
        }
        else
        {
            if (AudioManager.Instance.GetAudioName(AudioPlayerType.Bgm) == currCurtainBgm.name) return;

            AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
            {
                Debug.Log("播放原来的BGM" + currCurtainBgm.name);
                AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
            }, 0.01f);
        }
        
    }


    public void DelayInvoke(float t, Action action)
    {
        StartCoroutine(WaitForDo(t, action));
    }

    private IEnumerator WaitForDo(float t, Action ac)
    {
        yield return new WaitForSeconds(t);
        ac.Invoke();
    }


    private IEnumerator WaitForSlowDown(float t)
    {
        yield return new WaitForSeconds(t);
        AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, null, 0.01f);
    }


    private void PlayWolfSound()
    {
        CancelInvoke("PlayWolfSoundRepeat");
        InvokeRepeating("PlayWolfSoundRepeat", 0, 6);
    }

    private void PlayWolfSoundRepeat()
    {
        AudioManager.Instance.PlayOneShot(soundClips[14]);
    }

    private void OnDayNightChanged(EventData e)
    {
        skyState = (SkyState)e.data;
        if (skyState == SkyState.Night)
        {
            Debug.Log("夜晚");
            if (isInHome)
            {
                AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
                {
                    Debug.Log("播放夜晚房屋音效" + bgmClips[8].name);
                    AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, bgmClips[8], dumpSpeed * 2, null, 0.01f, AudioPlayMode.Loop);
                }, 0.01f);
            }
            else
            {
                AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
                {
                    PlayWolfSound();
                    Debug.Log("播放夜晚音效");
                    AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, bgmClips[7], dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
                }, 0.01f);

                
            }
        }
        else
        {
            Debug.Log("白天");

            CancelInvoke("PlayWolfSoundRepeat");

            AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
            {
                Debug.Log("播放原来的BGM" + currCurtainBgm.name);
                AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
            }, 0.01f);
        }
    }
	
    public void SwitchToBgm(int index)
    {
        AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
        {
            currCurtainBgm = bgmClips[index];
            AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
        }, 0.01f);
    }

    public void SwitchToBgmOnce(int index)
    {
        AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
        {
            currCurtainBgm = bgmClips[index];
            AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Once);
        }, 0.01f);
    }

    private void OnClickFather(EventData e)
    {
        if (SceneMgr.curtainIndex == 1 && SceneMgr.curSceneIndex == 2)
        {
            DelayInvoke(9.6f, () =>
            {
                AudioManager.Instance.Play(AudioPlayerType.Sound, soundClips[5], true);
                AudioManager.Instance.SetVolume(AudioPlayerType.Sound, 0.7f);
                DelayInvoke(5f, () =>
                {
                    AudioManager.Instance.SlowDown(AudioPlayerType.Sound, dumpSpeed, null, 0.01f);
                });
            });

            AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
            {
                AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, bgmClips[2], dumpSpeed, ()=> 
                {
                    StartCoroutine(WaitForSlowDown(8f));
                }, 0.01f, AudioPlayMode.Once);
            }, 0.01f);
        }
    }




    private void OnCurtainChanged(EventData e)
    {
        currCurtainIndex = (int)e.data;
        Debug.Log("当前幕 " + currCurtainIndex);

        CurtainTips.Instance.ShowTips(textArray[currCurtainIndex]);

        switch (currCurtainIndex)
        {
            case 2:
                currCurtainBgm = bgmClips[3];   //第三幕BGM
                //AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
                break;
            case 3:
                AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
                {
                    currCurtainBgm = bgmClips[9];   //第三幕BGM
                    AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);

                    AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Sound, soundClips[12], dumpSpeed, null, 0.01f, AudioPlayMode.Loop);

                }, 0.01f);

                break;

            case 4:
                AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
                {
                    currCurtainBgm = bgmClips[6];   //第四幕BGM
                    AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
                }, 0.01f);
                break;

            case 5:
                AudioManager.Instance.SlowDown(AudioPlayerType.Bgm, dumpSpeed, () =>
                {
                    currCurtainBgm = bgmClips[4];   //第五幕BGM 1
                    AudioManager.Instance.PlayBgmTurnUp(AudioPlayerType.Bgm, currCurtainBgm, dumpSpeed, null, 0.01f, AudioPlayMode.Loop);
                }, 0.01f);
                break;
        }

        if (currCurtainIndex != 3)
        {
            if (AudioManager.Instance.GetAudioName(AudioPlayerType.Sound) == soundClips[12].name)
            {
                AudioManager.Instance.Stop(AudioPlayerType.Sound);
            }
        }

    }
}
