using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterCtrl : MonoBehaviour 
{
    public GameObject letterPanel;
    public Button okBtn;
	// Use this for initialization
	void Start () 
    {
        letterPanel.SetActive(false);
        okBtn.onClick.AddListener(OnBtnClick);
        EventManager.addEventListener(EventType.ShowLetter, OnShowLetter);
	}

    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.ShowLetter, OnShowLetter);
    }
    void OnShowLetter(EventData data)
    {
        letterPanel.SetActive(true);
    }
	void OnBtnClick()
    {
        letterPanel.SetActive(false);
        GlobalState.startTalk = false;
        GlobalState.isTalked = true;
    }
	// Update is called once per frame
	void Update () 
    {
		
	}
}
