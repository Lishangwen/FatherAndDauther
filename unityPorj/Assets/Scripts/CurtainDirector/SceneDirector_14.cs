using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDirector_14 : MonoBehaviour 
{
    enum Progress
    {
        STOP,
        DownBike,
        Move2Boat,
        ReachBoat,
        BackHug,
        Back2Boat,
        FatherLeave,
        END
    }
    public GameObject father;
    public GameObject dog;
    public GameObject girl;
    public GameObject mailBox;
    public GameObject newBike;
    public GameObject oldBike;
    public GameObject fatherBoat;
    public Image mailTipBg;
    public Text mailTipTxt;

    public GameObject stagePoint;// 阶梯边
    public GameObject boatPoint; // 父亲上船点
    GirlCtrl girlCtrl;
    FatherOnBikeCtrl fatherCtrl;
    FatherBoat fatherBoatCtrl;
    DogCtrl dogCtrl;
    Progress curProgress;

    bool isHuged = false;
    bool isSkySet = false;
	// Use this for initialization
	void Start () 
    {
        girlCtrl = girl.GetComponent<GirlCtrl>();
        fatherCtrl = father.GetComponent<FatherOnBikeCtrl>();
        dogCtrl = dog.GetComponent<DogCtrl>();
        fatherBoatCtrl = fatherBoat.GetComponent<FatherBoat>();
        girlCtrl.ForbidContrl(true);
        fatherCtrl.ForbidContrl(true);
        dogCtrl.CancelFollow(true);
        EventManager.addEventListener(EventType.StartHugGirl, OnFatherClick);
        curProgress = Progress.STOP;
	}
    void OnFatherClick(EventData data)
    {
        if(curProgress==Progress.ReachBoat)
        {
            curProgress = Progress.BackHug;
            girlCtrl.Idle();
            BgmCtrl.PlayBGM(BGM.BGM1_2);
        }
    }
    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.StartHugGirl, OnFatherClick);
    }
    float GetDistance(GameObject a, GameObject b)
    {
        float distance = 0;
        Vector3 aPosition = a.transform.position;
        Vector3 bPosition = b.transform.position;
        distance = Vector3.Distance(aPosition, bPosition);
        return distance;
    }
    void UpdateUIColor()
    {
        float curClorValue = 1.0f - SkyMgr.Alphavalue;
        Color curColor = new Color(curClorValue, curClorValue, curClorValue, 1);
        mailTipBg.color = curColor;
        mailTipTxt.color = curColor;

    }
	// Update is called once per frame
	void Update () 
    {
        UpdateUIColor();
        // 狗到邮箱附近停下
		if(GetDistance(dog,mailBox)>2f)
        {
            dogCtrl.MoveRight();
        }
        else
        {
            dogCtrl.Idle();
        }

        switch(curProgress)
        {
            case Progress.STOP:
                {
                    if (GetDistance(father, mailBox) > 0.5f)
                    {
                        fatherCtrl.MoveRight();
                    }
                    else
                    {
                        fatherCtrl.Idle();
                        curProgress = Progress.DownBike;
                    }
                }
                break;
            case Progress.DownBike:
                {
                    AnimatorStateInfo animatorInfo;
                    animatorInfo = fatherCtrl.fatherAnimator.GetCurrentAnimatorStateInfo(0);
                    if (animatorInfo.IsTag("Idle1"))
                    {
                        oldBike.SetActive(false);
                        newBike.SetActive(true);
                        girl.SetActive(true);
                        girlCtrl.Idle();
                        curProgress = Progress.Move2Boat;
                    }
                    else
                    {
                        fatherCtrl.fatherAnimator.SetBool("isDownBike", true);
                    }
                }
                break;
            case Progress.Move2Boat:
                {
                    if (GetDistance(girl, stagePoint) > 0.5 && GetDistance(father, boatPoint) < 1.5f)
                    {
                        girlCtrl.MoveRight();
                    }
                    else
                    {
                        girlCtrl.Idle();
                    }
                    if (GetDistance(father, boatPoint) > 0.5f)
                    {
                        fatherCtrl.MoveRight();
                    }
                    else
                    {
                        fatherCtrl.Idle();
                        fatherCtrl.TurnLeft();
                        fatherCtrl.HeadTip.SetActive(true);
                        curProgress = Progress.ReachBoat;
                        girlCtrl.Idle();
                        //
                    }
                }
                break;
            case Progress.BackHug:
                {
                    if (GetDistance(father, girl) > 1.0f)
                    {
                        fatherCtrl.MoveLeft();
                    }
                    else
                    {
                        fatherCtrl.Idle();
                        AnimatorStateInfo animatorInfo;
                        animatorInfo = fatherCtrl.fatherAnimator.GetCurrentAnimatorStateInfo(0);
                        
                        if(!isHuged)
                        {
                            fatherCtrl.fatherAnimator.SetBool("startHug", true);
                            
                        }
                        //if (animatorInfo.IsTag("PutGirl"))
                        if (animatorInfo.IsTag("PutGirl") && !isHuged)
                        {
                            isHuged = true;
                            fatherCtrl.fatherAnimator.SetBool("startHug", false);
                        }
                        if(animatorInfo.IsTag("HugGirl"))
                        {
                            girl.SetActive(false);
                        }
                        else if(animatorInfo.IsTag("PutOk")&&isHuged)
                        {
                            girl.SetActive(true);
                            girlCtrl.Idle();
                        }
                        else if (animatorInfo.IsTag("Idle1") && isHuged)
                        {
                            curProgress = Progress.Back2Boat;
                        }
                        
                    }
                }
                break;
            case Progress.Back2Boat:
                {
                    if (GetDistance(father, boatPoint) > 0.5f)
                    {
                        fatherCtrl.MoveRight();
                    }
                    else
                    {
                        fatherCtrl.Idle();
                        fatherCtrl.TurnLeft();
                        curProgress = Progress.FatherLeave;
                    }
                }
                break;
            case Progress.FatherLeave:
                {
                    father.SetActive(false);
                    fatherBoatCtrl.Leave();
                    girlCtrl.ForbidContrl(false);
                    if(!isSkySet)
                    {
                        SkyMgr.IgnoreSkyState(false);
                        SkyMgr.BegainFade();    // 开始渐渐变黑
                        isSkySet = true;
                    }
                    
                    if(fatherBoat.transform.localPosition.y>2.7)
                    {
                        fatherBoat.SetActive(false);
                    }
                }
                break;
        }
        if(SkyMgr.curState==SkyState.Night)
        {
            SceneMgr.Switch2NextCurtain();
            SkyMgr.IgnoreSkyState(false);
            SkyMgr.ResetSkyTime(0.61f);
        }
        
	}
}
