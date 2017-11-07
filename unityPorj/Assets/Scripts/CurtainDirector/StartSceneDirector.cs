using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneDirector : MonoBehaviour 
{
    public GameObject girl;
    public GameObject father;
    public GameObject title;
    public GameObject startBtnAnima;

    private Animator titleAnimator;
    private Animator girlAnimator;
    private Animator fatherAnimator;

    public float girlSpeed;
    public float fatherSpeed;
    public Button startGameBtn;
    float timeCounter = 0;
	// Use this for initialization

    enum State
    {
        None,
        MoveComplete,
        TitleComplete,
        StartGame
    }

    State curState;
	void Start () 
    {
        girlAnimator = girl.GetComponent<Animator>();
        fatherAnimator = father.GetComponent<Animator>();
        titleAnimator = title.GetComponent<Animator>();
        startGameBtn.onClick.AddListener(OnStartGameClick);
        curState = State.None;
        SkyMgr.IgnoreSkyState(true);
        BgmCtrl.PlayBGM(BGM.START);
	}
	
    void OnStartGameClick()
    {
        if (curState == State.TitleComplete)
        {
            curState = State.StartGame;
            EventManager.dispatchEvent(EventType.TitleComplete, null);
        }
    }
	// Update is called once per frame
	void Update () 
    {
        switch(curState)
        {
            case State.None:
                {
                    if (girl.transform.localScale.y < 0.8)
                    {
                        girlAnimator.SetBool("isStop", false);
                        girl.transform.localPosition -= new Vector3(0, Time.deltaTime * girlSpeed, 0);
                        girl.transform.localScale += new Vector3(Time.deltaTime * 0.1f, Time.deltaTime * 0.1f, 0);
                    }
                    else
                    {
                        girlAnimator.SetBool("isStop", true);
                    }
                    if (father.transform.localScale.y < 0.8)
                    {
                        fatherAnimator.SetBool("isStop", false);
                        father.transform.localPosition -= new Vector3(0, Time.deltaTime * fatherSpeed, 0);
                        father.transform.localScale += new Vector3(Time.deltaTime * 0.1f, Time.deltaTime * 0.1f, 0);
                    }
                    else
                    {
                        fatherAnimator.SetBool("isStop", true);
                        curState = State.MoveComplete;
                    }
                }break;

            case State.MoveComplete:
                {
                    title.SetActive(true);
                    AnimatorStateInfo animatorInfo;
                    animatorInfo = titleAnimator.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.IsTag("TitleOk"))
                    {
                        curState = State.TitleComplete;
                    }
                }break;
            case State.TitleComplete:
                {
                    startGameBtn.gameObject.SetActive(true);
                    startBtnAnima.SetActive(true);
                }break;
            case State.StartGame:
                {
                    timeCounter += Time.deltaTime;
                    if (timeCounter >= 3)
                    {
                        timeCounter = 2.9f;

                        SceneMgr.InitFirstCurtain();
                    }
                    SkyMgr.BecomeDark(3, timeCounter);
                    //SceneMgr.InitFirstCurtain();
                }break;
        }
        
	}
}
