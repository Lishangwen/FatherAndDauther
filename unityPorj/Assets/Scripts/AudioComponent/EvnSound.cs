using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvnSound : MonoBehaviour {

    public AudioClip clip;
    public int curtainId = -1;
    public int sceneId = -1;
    public SkyState sky;
    public bool compSky;
    public bool loop;

	// Use this for initialization
	void Start () {

        if (AudioManager.Instance.GetAudioName(AudioPlayerType.Sound) == clip.name) return;

        bool trueCurtain = curtainId == -1 ? true : curtainId == SceneMgr.curtainIndex;
        bool trueScene = sceneId == -1 ? true : sceneId == SceneMgr.curSceneIndex;
        bool trueSky = compSky ? true : SkyMgr.curState == sky;

        if (trueCurtain && trueScene && trueSky)
        {
            AudioManager.Instance.Play(AudioPlayerType.Sound, clip, loop);
        }
	}

}
