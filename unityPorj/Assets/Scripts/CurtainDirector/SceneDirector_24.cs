using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDirector_24 : MonoBehaviour 
{
    public GameObject boat;
    public GameObject bike;
    public GameObject dog;
    public GameObject postMan;
    public GameObject mailBox;  // 邮箱点
    public GameObject boatPoint;
    public GameObject headTip;
    public GameObject pretendBike;
    public GameObject oldGoFar;   // 向远处走的老人
    public GameObject husband;
    public GameObject wife;

    public Image mailTipBg;
    public Text mailTipTxt;
    BoatCtrl boatCtrl;
    GirlCtrl girlCtrl;
    PostManCtrl postManCtrl;
    DogCtrl dogCtrl;
    State curState;
    State girlState;
    State girlState1;
    public GameObject bg1_3;
    public GameObject bg4;
    public GameObject bg5;
    public GameObject bg3;

    float timeCounter=0;

    enum State
    {
        None,
        Move2Wharf,  // 船向码头移动
        Move2MailBox,  // 移向邮箱
        StopAtMailBox,  // 停在邮箱处
        MoveBack2Boat,  // 回移
        ReachBoat,      // 到达船的位置
        DownBikeComplete,  // 女孩下车完成
        ClampBikeComplete,
        ReachEngage,       // 到达边界
        GoFar,
        ReachFarPoint
    }
	// Use this for initialization
	void Start () 
    {
        if (SceneMgr.curtainIndex != 2)
        {
            dog.SetActive(false);
        }

        boatCtrl = boat.GetComponent<BoatCtrl>();
        girlCtrl = bike.GetComponent<GirlCtrl>();
        dogCtrl = dog.GetComponent<DogCtrl>();
        postManCtrl = postMan.GetComponent<PostManCtrl>();
        dogCtrl.CancelFollow(true); //狗取消跟随
        headTip.SetActive(false);
        if(!GlobalState.isEnteredWharf)  // 第一次进入码头
        {
            boatCtrl.MoveLeft();
            postMan.SetActive(false);
            curState = State.Move2Wharf;
        }
        else
        {
            if(!GlobalState.isTalked)
            {
                
                postMan.transform.localPosition = new Vector3(-3.04f, -2.04f, 0);
                postMan.transform.eulerAngles = new Vector3(postMan.transform.eulerAngles.x, postMan.transform.eulerAngles.y, 0);
                postMan.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                //postManCtrl.CurDirect = Direction.RIGH
                postMan.SetActive(true);
                
                

                boat.transform.localPosition = new Vector3(4.29f, -3.47f, 0);
                //boat.transform.eulerAngles = new Vector3(0, 180, 0);
                boat.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                boatCtrl.Idle();
                boatCtrl.TurnLeft();
                curState = State.StopAtMailBox;
            }
            else
            {
                postMan.SetActive(false);
                boat.SetActive(false);
                curState = State.None;
            }
        }
        if(SceneMgr.curtainIndex==5)
        {
            boat.SetActive(false);
            postMan.SetActive(false);
        }
        GlobalState.isEnteredWharf = true;
        EventManager.addEventListener(EventType.PostManOnClick, OnPostManClick);
        girlState = State.None;
        girlState1 = State.None;
	}
	
    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.PostManOnClick, OnPostManClick);
    }
    void OnPostManClick(EventData data)
    {
        if(curState==State.StopAtMailBox)
        {
            GlobalState.startTalk = true;
        }
    }

    
    void UpdateUIColor()
    {
        float curClorValue = 1.0f - SkyMgr.Alphavalue;
        Color curColor = new Color(curClorValue, curClorValue, curClorValue, 1);
        mailTipBg.color = curColor;
        mailTipTxt.color = curColor;
        SpriteRenderer tipImage = headTip.GetComponent<SpriteRenderer>();
        tipImage.color = curColor;
        
    }

    void DownBike()
    {
        girlCtrl.ForbidContrl(true);
        girlCtrl.Idle();
        girlCtrl.CurAnimator.SetBool("isDownBike", true);
        AnimatorStateInfo animatorInfo;
        animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsTag("Idle1")||animatorInfo.IsTag("Walk1"))
        {
            pretendBike.SetActive(true);
            girlCtrl.ForbidContrl(false);
            girlState1 = State.DownBikeComplete;
            
            if (SceneMgr.curtainIndex == 4)
            {

                husband.SetActive(true);
                wife.SetActive(true);
                
            }
            if(SceneMgr.curtainIndex==5)
            {
                girlState = State.DownBikeComplete;
            }
            else
            {
                bike.gameObject.transform.localPosition += new Vector3(0.3f, 0, 0);
            }
        }
    }
    void ClampBike()
    {
        girlCtrl.ForbidContrl(true);
        girlCtrl.Idle();
        girlCtrl.CurAnimator.SetBool("isDownBike", false);
        pretendBike.SetActive(false);
        husband.SetActive(false);
        wife.SetActive(false);
        husband.SetActive(false);
        wife.SetActive(false);
        AnimatorStateInfo animatorInfo;
        animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsTag("Idle") || animatorInfo.IsTag("Idle2")||animatorInfo.IsTag("Walk2"))
        {
            girlCtrl.ForbidContrl(false);
            girlState1 = State.ClampBikeComplete;
            bike.gameObject.transform.localPosition -= new Vector3(0.3f, 0, 0);
            
        }
    }
	// Update is called once per frame
	void Update () 
    {
        if (SceneMgr.curtainIndex != 2)
        {
            dog.SetActive(false);
        }
        UpdateUIColor();
        if (SceneMgr.curtainIndex == 4)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(true);
            bg5.SetActive(false);
            bg3.SetActive(false);
        }
        else if (SceneMgr.curtainIndex == 5)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(false);
            bg5.SetActive(true);
            bg3.SetActive(false);
            postMan.SetActive(false);
            boat.SetActive(false);
        }
        else if (SceneMgr.curtainIndex == 3)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(false);
            bg5.SetActive(false);
            bg3.SetActive(true);
        }
        else
        {
            bg1_3.SetActive(true);
            bg4.SetActive(false);
            bg5.SetActive(false);
            bg3.SetActive(false);
        }
	}
    void FixedUpdate()
    {
        #region 邮差逻辑
        switch (curState)
        {
            case State.Move2Wharf:
                {
                    if(boatCtrl.isReachLeft)
                    {
                        curState = State.Move2MailBox;
                        boatCtrl.Idle();
                    }
                    else
                    {
                        boatCtrl.MoveLeft();
                    }
                }break;
            case State.Move2MailBox:
                {
                    postMan.SetActive(true);
                    postManCtrl.TurnLeft();

                    if(GlobalMethods.GetDistance(postMan,mailBox)>0.5f)
                    {
                        postManCtrl.MoveLeft();
                    }
                    else
                    {
                        postManCtrl.Idle();
                        curState = State.StopAtMailBox;
                    }
                } break;
            case State.StopAtMailBox:
                {
                    if(GlobalState.isTalked)
                    {
                        headTip.SetActive(false);
                        curState = State.MoveBack2Boat;
                    }
                    else
                    {
                        postManCtrl.TurnLeft();
                        headTip.SetActive(true);
                    }
                } break;
            case State.MoveBack2Boat:
                {
                    if (GlobalMethods.GetDistance(postMan, boatPoint) > 0.5f)
                    {
                        postManCtrl.MoveRight();
                    }
                    else
                    {
                        postManCtrl.Idle();
                        curState = State.ReachBoat;
                    }
                }break;
            case State.ReachBoat:
                {
                    postMan.SetActive(false);
                    boatCtrl.MoveRight();
                }break;
        }
        #endregion
        if (SceneMgr.curtainIndex != 5 && SceneMgr.curtainIndex != 1)
        {
            float a = GlobalMethods.GetDistance(bike, mailBox);
            if (a < 0.2f)
            {
                if (girlCtrl.CurDirect == Direction.RIGHT&&bike.gameObject.transform.localPosition.x<mailBox.gameObject.transform.localPosition.x)
                {
                    //lightling.SetActive(false);
                    //if(SceneMgr.curtainIndex!=4)
                    {
                        if (girlState1 != State.DownBikeComplete)
                        {
                            DownBike();
                        }
                    }

                   
                }
                else if (girlCtrl.CurDirect == Direction.LEFT && bike.gameObject.transform.localPosition.x > mailBox.gameObject.transform.localPosition.x)
                {
                    //if (SceneMgr.curtainIndex != 4)
                    {
                        if (girlState1 != State.ClampBikeComplete)
                        {
                            ClampBike();
                        }
                    }
                    
                }
            }
        }
        if(SceneMgr.curtainIndex==5)
        {
            switch (girlState)
            {
                case State.None:
                    {
                        if (GlobalMethods.GetDistance(bike, mailBox) < 0.5)
                        {
                            DownBike();
                        }
                    } break;
                case State.DownBikeComplete:
                    {
                        if (GlobalMethods.GetDistance(bike, boatPoint) > 0.5)
                        {
                            girlCtrl.ForbidContrl(true);
                            girlCtrl.MoveRight();
                        }
                        else
                        {
                            girlState = State.ReachEngage;
                        }
                    } break;
                case State.ReachEngage:
                    {
                        bike.SetActive(false);
                        oldGoFar.SetActive(true);
                        girlState = State.GoFar;
                        //
                    } break;
                case State.GoFar:
                    {
                        if(oldGoFar.transform.localScale.y>0.25)
                        {
                            oldGoFar.transform.localPosition += new Vector3(0, Time.deltaTime * 0.5f, 0);
                            oldGoFar.transform.localScale -= new Vector3(Time.deltaTime * 0.1f, Time.deltaTime * 0.1f, 0);
                        }
                        else
                        {
                            girlState = State.ReachFarPoint;
                        }
                    }break;
                case State.ReachFarPoint:
                    {
                        timeCounter += Time.deltaTime;
                        
                        if (timeCounter >= 3)
                        {
                            timeCounter =2.9f;
                            SceneMgr.Switch2NextScene();
                            //SkyMgr.BecomeDark(3,0);
                        }
                        SkyMgr.BecomeDark(3, timeCounter);
                    }break;
            }
        }
        
        
        
    }
}
