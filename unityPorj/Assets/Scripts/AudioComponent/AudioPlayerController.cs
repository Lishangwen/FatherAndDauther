using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioPlayerController : MonoBehaviour
{
    private AudioSource audioSource;

    public string AudioClipName
    {
        get { return audioSource.clip == null ? string.Empty : audioSource.clip.name; }
    }

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public float Volume
    {
        get { return audioSource.volume; }
        set { audioSource.volume = value; }
    }


    public void PlayOnShot(AudioClip clip)
    {
        audioSource.volume = 1;
        audioSource.PlayOneShot(clip);      
    }

    public void PlayBgmTurnUp(AudioClip clip, float speed, UnityAction onComplete, float min = 0.001f, AudioPlayMode playMode = AudioPlayMode.Loop)
    {
        StopAllCoroutines();

        switch (playMode)
        {
            case AudioPlayMode.Loop:
                audioSource.clip = clip;
                audioSource.loop = true;
                break;
            case AudioPlayMode.Once:
                audioSource.clip = clip;
                audioSource.loop = false;
                break;
            case AudioPlayMode.OneShot:
                PlayOnShot(clip);
                break;
        }
        StartCoroutine(TurnUpCoroutine(speed, onComplete, min));
    }

    public void Play(AudioClip clip, bool isLoop = true)
    {
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SlowDown(float speed, UnityAction onComplete, float min = 0.001f)
    {
        StopAllCoroutines();
        StartCoroutine(SlowDownCoroutine(speed, onComplete, min));
    }



    private IEnumerator TurnUpCoroutine(float speed, UnityAction onComplete, float min = 0.001f)
    {
        audioSource.volume = min;
        audioSource.Play();
        yield return new WaitUntil(() =>
        {
            audioSource.volume += Time.deltaTime * 0.01f * speed;
#if UNITY_EDITOR
            //Debug.Log("音量: " + audioSource.volume);
#endif
            return audioSource.volume > 0.9999f;
        });
        audioSource.volume = 1f;
        if (onComplete != null)
        {
            onComplete();
        }
    }

    private IEnumerator SlowDownCoroutine(float speed, UnityAction onComplete, float min = 0.001f)
    {

        yield return new WaitUntil(() =>
        {
            audioSource.volume -= Time.deltaTime * 0.01f * speed;
#if UNITY_EDITOR
            //Debug.Log("音量: " + audioSource.volume);
#endif
            return audioSource.volume < min;
        });
        audioSource.Stop();
        if (onComplete != null)
        {
            onComplete();
        }
    }
}
