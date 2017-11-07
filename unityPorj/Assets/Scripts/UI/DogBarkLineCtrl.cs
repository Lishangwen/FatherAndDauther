using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBarkLineCtrl : MonoBehaviour 
{
    public GameObject dogBarkLine;
    float barkLineShowTime=2.0f;
    bool EnableBarkLine=false;
	// Use this for initialization
	void Start () 
    {
        dogBarkLine.SetActive(false);
        EventManager.addEventListener(EventType.DogSpark, OnDogSpark);
	}
	
    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.DogSpark, OnDogSpark);
    }

    void OnDogSpark(EventData data)
    {
        if (SceneMgr.curtainIndex == 2&&SkyMgr.curState==SkyState.Night)
        {
            EnableBarkLine = true;
            barkLineShowTime = 2.0f;
        }
        else
        {
            dogBarkLine.SetActive(false);
        }
    }
	// Update is called once per frame
	void Update () 
    {
        if (SceneMgr.curtainIndex == 2 && SkyMgr.curState == SkyState.Night)
        {
            if (EnableBarkLine && barkLineShowTime > 0)
            {
                dogBarkLine.SetActive(true);
                barkLineShowTime -= Time.deltaTime;
            }
            else
            {
                dogBarkLine.SetActive(false);
                EnableBarkLine = false;
                barkLineShowTime = 2.0f;
            }
        }
        else
        {
            dogBarkLine.SetActive(false);
        }
		
	}
}
