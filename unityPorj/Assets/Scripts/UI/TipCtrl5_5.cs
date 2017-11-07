using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCtrl5_5 : MonoBehaviour 
{
    public GameObject tip;
    bool showTip = false;
    float timeCounter=0;
	// Use this for initialization
	void Start () 
    {
        EventManager.addEventListener(EventType.ShowTip, OnShowTip);
	}
	void OnShowTip(EventData data)
    {
        showTip = true;
    }
	// Update is called once per frame
	void Update () 
    {
		if(showTip&&timeCounter<2)
        {
            timeCounter += Time.deltaTime;
            tip.SetActive(true);
        }
        else
        {
            showTip = false;
            timeCounter = 0;
            tip.SetActive(false);
        }
	}
}
