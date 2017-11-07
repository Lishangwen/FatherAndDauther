using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAudioPlayer : MonoBehaviour
{
    [HideInInspector]
    public AudioPlayerType PlayerType;
    [HideInInspector]
    public EventType startEvent;
    [HideInInspector]
    public EventType endEvent;
    [HideInInspector]
    public bool compEventData;
    [HideInInspector]
    public string eventData;

    public AudioClip audioClip;

    public abstract void PlayAudio(EventData data);
    public abstract void StopAudio(EventData data);
    public abstract void OnAwake();

    private void OnDestroy()
    {
        if (startEvent != EventType.None) EventManager.removeEventListener(startEvent, PlayAudio);
        if (endEvent != EventType.None) EventManager.removeEventListener(endEvent, StopAudio);
       
    }

    private void Awake()
    {
        
        if (startEvent != EventType.None) EventManager.addEventListener(startEvent, PlayAudio);
        if (endEvent != EventType.None) EventManager.addEventListener(endEvent, StopAudio);
        OnAwake();
    }

   
}
