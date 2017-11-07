using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSound : MonoBehaviour {

    public AudioPlayMode type;
    public AudioClip clip;

    public void Play()
    {
        switch (type)
        {
            case AudioPlayMode.Loop:
                
                break;
            case AudioPlayMode.OneShot:
                AudioManager.Instance.PlayOneShot(clip);
                break;
        }
    }

}
