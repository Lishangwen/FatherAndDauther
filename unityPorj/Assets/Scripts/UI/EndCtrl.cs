using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndCtrl : MonoBehaviour 
{
    public Button exitBtn;
    public Button restartBtn;
    public Animator endAnimator;
    public GameObject endObj;
    public GameObject btnBg1;
    public GameObject btnBg2;
    bool isPlay = false;
    
    enum State
    {
        PlayEnd,
        PlayComplete,
        Finally
    }
    State curState = State.PlayEnd;
	// Use this for initialization
	void Start ()
    {
        curState = State.PlayEnd;
	}
	

    void OnExit()
    {
        Application.Quit();
    }
    void OnRestart()
    {
        SceneMgr.RestartGame();
        GlobalState.ResetFlag();
    }
	// Update is called once per frame
	void Update () 
    {
        switch(curState)
        {
            case State.PlayEnd:
                {
                    AnimatorStateInfo animatorInfo;
                    animatorInfo = endAnimator.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.IsTag("EndPlay"))
                    {
                        isPlay = true;
                    }
                    else if (isPlay && animatorInfo.IsTag("EndIdle"))
                    {
                        curState = State.PlayComplete;
                        endObj.SetActive(false);
                        exitBtn.gameObject.SetActive(true);
                        restartBtn.gameObject.SetActive(true);
                        btnBg1.SetActive(true);
                        btnBg2.SetActive(true);
                    }
                }break;
            case State.PlayComplete:
                {
                    exitBtn.onClick.AddListener(OnExit);
                    restartBtn.onClick.AddListener(OnRestart);
                    curState = State.Finally;
                } break;

        }
		
	}
}
