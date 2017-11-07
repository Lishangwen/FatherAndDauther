using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip;
    public bool playOnFirst;
    public bool loop;
    public bool compSky;
    public bool compCurtain;

    public int [] curtain;
    public SkyState state;
    public float delay = 0f;

    public float volume = 1;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        EventManager.addEventListener(EventType.SwitchCurtain, OnSwitchCurtain);
    }

    private void OnSwitchCurtain(EventData data)
    {
        if (!audioSource.isPlaying)
        {
            OnEnable();
        }
    }

    private void OnDestroy()
    {
        EventManager.removeEventListener(EventType.SwitchCurtain, OnSwitchCurtain);
    }




    private void OnEnable()
    {
        if (compSky && state != SkyMgr.curState)
        {
            audioSource.Stop();
            return;
        }
        if (compCurtain)
        {
            bool or = false;
            for (int i = 0; i < curtain.Length; i++)
            {
                if (curtain[i] == SceneMgr.curtainIndex)
                {
                    or = true;
                    break;
                }
            }
            if (!or)
            {
                audioSource.Stop();
                return;
            }
        }

        if (delay > 0.0001f)
        {
            StartCoroutine(WaitForPlay());
        }
        else
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (playOnFirst)
        {

            audioSource.volume = volume;
            if (loop)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }

        }
        playOnFirst = true;
    }


    private IEnumerator WaitForPlay()
    {
        yield return new WaitForSeconds(delay);
        PlaySound();
    }
}
