using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector_55 : MonoBehaviour 
{
    public GameObject girl;
    GirlCtrl girlCtrl;
    public GameObject boatPoint;  // 船的地点
    public GameObject father;
    fatherCtrl_1 fatherCtrl;
    public GameObject fatherPoint;
    float timeCounter = 3;
    bool isSetLieDown = false;
    bool isSetGetUp = false;
    enum State
    {
        None,
        ReachBoat,
        BegainLieDown,
        LieDownCompelte,
        BegainBright,
        BegainGetUp,
        Back2Younth,
        Back2YounthComplete,
        Move2Father,
        StartHug,
        END
    }

    State curState;
	// Use this for initialization
	void Start () 
    {
        curState = State.None;
        girlCtrl = girl.GetComponent<GirlCtrl>();
        fatherCtrl = father.GetComponent<fatherCtrl_1>();
        EventManager.addEventListener(EventType.GrandmaLieDown, OnGrandmaLieDown);
        EventManager.addEventListener(EventType.FatherLastClick, OnFatherLastClick);
        //BgmCtrl.PlayBGM(BGM.BGM5_2);
	}
	
    void OnFatherLastClick(EventData data)
    {
        if(curState==State.Back2YounthComplete)
        {
            curState = State.Move2Father;
        }
    }

    void OnGrandmaLieDown(EventData data)
    {
        if(curState==State.None)
        {
            curState = State.BegainLieDown;
        }
        
    }
	// Update is called once per frame
	void Update () 
    {
		
	}

    void FixedUpdate()
    {
        switch(curState)
        {
            case State.None:
                {
                    timeCounter -= Time.deltaTime;
                    if (timeCounter <= 0)
                    {
                        timeCounter = 0;
                        
                    }
                    SkyMgr.BecomeDark(3, timeCounter);
                    if(GlobalMethods.GetDistance(girl,boatPoint)<2f&&girlCtrl.CurDirect==Direction.RIGHT)
                    {
                        girlCtrl.ForbidMoveRight(true);
                    }
                    else
                    {
                        girlCtrl.ForbidMoveRight(false);
                    }
                }break;
            case State.BegainLieDown:
                {
                    girlCtrl.ForbidContrl(true);
                    if(GlobalMethods.GetDistance(girl,boatPoint)>0.5f)
                    {
                        girlCtrl.MoveRight();
                    }
                    else
                    {
                        girlCtrl.Idle();
                        if (!isSetLieDown)
                        {
                            girlCtrl.CurAnimator.SetBool("LieDown", true);
                            isSetLieDown = true;   // 已经设置过躺下动画
                        }
                        AnimatorStateInfo animatorInfo;
                        animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
                        if (animatorInfo.IsTag("IdleLiewDown"))
                        {
                            curState = State.LieDownCompelte;
                        }
                    }
                    
                }break;
            case State.LieDownCompelte:
                {
                    timeCounter += Time.deltaTime;
                    if (timeCounter >= 3)
                    {
                        timeCounter = 2.9f;
                        curState = State.BegainBright;
                    }
                    SkyMgr.BecomeDark(3, timeCounter);
                    
                }break;
            case State.BegainBright:
                {
                    timeCounter -= Time.deltaTime;
                    if (timeCounter <= 0)
                    {
                        timeCounter = 0;
                        curState = State.BegainGetUp;
                    }
                    SkyMgr.BecomeDark(3, timeCounter);
                    
                } break;
            case State.BegainGetUp:
                {
                    if (!isSetGetUp)
                    {
                        girlCtrl.CurAnimator.SetBool("LieDown", false);
                        isSetGetUp = true;   // 已经设置过躺下动画
                    }
                    AnimatorStateInfo animatorInfo;
                    animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.IsTag("Idlex"))
                    {
                        curState = State.Back2Younth;
                        //BgmCtrl.PlayBGM(BGM.BGM5_2);

                        AudioPlayerManager.Instance.SwitchToBgmOnce(5);

                        EventManager.dispatchEvent(EventType.ShowTip, null);
                        girlCtrl.ForbidContrl(false);
                        girlCtrl.ForbidMoveLeft(true);
                        girlCtrl.ForbidMoveRight(false);
                    }
                }break;
            case State.Back2Younth:
                {
                    
                    if(girlCtrl.CurSpeed<0.1)
                    {
                        girlCtrl.CurAnimator.speed = 0;
                    }
                    else
                    {
                        girlCtrl.CurAnimator.speed = 1;
                    }
                     AnimatorStateInfo animatorInfo;
                    animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.IsTag("Idle1"))
                    {
                        curState = State.Back2YounthComplete;
                        
                    }
                }break;
            case State.Back2YounthComplete:
                {
                    if (GlobalMethods.GetDistance(girl, fatherPoint) < 0.2)
                    {
                        girlCtrl.ForbidContrl(true);
                        //curState = State.ReachFather;
                    }
                }break;
            case State.Move2Father:
                {
                    if(GlobalMethods.GetDistance(girl,fatherPoint)>0.2)
                    {
                        girlCtrl.MoveRight();
                        girlCtrl.ForbidContrl(true);
                        
                        //curState = State.ReachFather;
                    }
                    else
                    {
                        curState = State.StartHug;
                        girlCtrl.Idle();
                    }
                }break;
            case State.StartHug:
                {
                    fatherCtrl.CurAnimator.SetBool("isHug", true);
                    girl.SetActive(false);
                    AnimatorStateInfo animatorInfo;
                    animatorInfo = fatherCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.IsTag("Idle1"))
                    {
                        curState = State.END;
                    }
                }break;
            case State.END:
                {
                    timeCounter += Time.deltaTime;
                    if (timeCounter >= 3)
                    {
                        timeCounter = 2.9f;
                        curState = State.BegainBright;
                        SceneMgr.Switch2NextScene();
                    }
                    SkyMgr.BecomeDark(3, timeCounter);

                }break;
                
        }
    }
}
