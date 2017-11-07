using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMgr : MonoBehaviour {
    public Text tipText;
    public Button testButton;
    bool isClicked = false;
	// Use this for initialization
	void Start () {
        testButton.onClick.AddListener(OnBtnClick);
	}
	void OnBtnClick()
    {
        isClicked = !isClicked;
        tipText.gameObject.SetActive(isClicked);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
