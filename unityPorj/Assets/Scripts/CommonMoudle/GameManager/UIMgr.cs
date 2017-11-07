using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIMgr : MonoBehaviour {
    public GameObject tipText;
    public Button testButton;
    bool isClicked = false;
    public Text tipTextUI;
    bool isShow=false;
    float timeCounter=0;
	// Use this for initialization
	void Start () {
        
        testButton.onClick.AddListener(OnBtnClick);
        //tipTextUI = tipText.GetComponent<Text>();
	}
	void OnBtnClick()
    {
        if(SceneMgr.curtainIndex!=1)
        {
            isShow = true;
            timeCounter = 0;
            
            if (tipText.gameObject.activeSelf == false)
            {
                EventManager.dispatchEvent(EventType.ClickMailBox, null);
            }
            /*
            isClicked = !isClicked;
            tipText.gameObject.SetActive(isClicked);
            **/
        }
    }
	// Update is called once per frame
	void Update () 
    {
		if(SceneMgr.curtainIndex==2)
        {
            tipTextUI.text = "邮箱空空如也";
        }
        else if (SceneMgr.curtainIndex == 3)
        {
            tipTextUI.text = "邮箱里堆满了信，全是陌生人的名字";
        }
        else if(SceneMgr.curtainIndex==4)
        {
            tipTextUI.text = "邮箱落了厚厚一层灰";
        }
        else if (SceneMgr.curtainIndex == 5)
        {
            tipTextUI.text = "邮箱落了厚厚一层灰";
        }
        if(isShow&&timeCounter<2)
        {
            timeCounter += Time.deltaTime;
            tipText.SetActive(true);
        }
        else
        {
            isShow = false;
            timeCounter = 0;
            tipText.SetActive(false);
        }
	}
}
