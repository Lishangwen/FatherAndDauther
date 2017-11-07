using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GirlCtrl : BaseActor 
{
    public Button leftBtn;
    public Button rightBtn;
    public GameObject lightling;  // 手电筒
    
    public float speed2;  // 第二幕速度
    public float speed3;
    public float speed4;
    public float speed5;
    public float darkSpeed;   // 黑夜速度

    public float dragForce2;   // 第二幕阻力
    public float dragForce3;
    public float dragForce4;
    public float dragForce5;
    public float darkDragForce;   // 黑夜阻力

    public bool isForbidMoveLeft=false;
    public bool isForbidMoveRight=false;
	// Use this for initialization

    enum OldState
    {
        Walk,
        RideBike,
        PushBike
    }

	void Start () 
    {
        SetOriginDirect(Direction.LEFT);
        Init();
        InitPostion();
        Idle();
        if(lightling!=null)
        {
            if (SkyMgr.curState == SkyState.Day)
            {
                lightling.SetActive(false);
            }
            else
            {
                lightling.SetActive(true);
            }
        }
        
        EventTriggerListener.GetListener(leftBtn.gameObject).onPointerDown +=OnLeftBtnDown;
        EventTriggerListener.GetListener(rightBtn.gameObject).onPointerDown += OnRightBtnDown;
        EventTriggerListener.GetListener(leftBtn.gameObject).onPointerUp += OnLeftBtnUp;
        EventTriggerListener.GetListener(rightBtn.gameObject).onPointerUp += OnRightBtnUp;
        EventManager.addEventListener(EventType.DayNightChange, OnUpdateLightView);
        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainSwitch);

        
        if(SceneMgr.curtainIndex==2)
        {
            CurAnimator.SetBool("isNoWind", true);
        }
        else
        {
            CurAnimator.SetBool("isNoWind", false);
        }
        if (SceneMgr.curtainIndex == 3)
        {
            CurAnimator.SetBool("isWind", true);
        }
        else
        {
            CurAnimator.SetBool("isWind", false);
        }

        if(SceneMgr.curtainIndex==4)
        {
            CurAnimator.SetBool("isFamly", true);
        }
        UpdateOldState();
        UpdateSpeed();
       
	}

    void SwitchByOldState(OldState oldState)
    {
        switch(oldState)
        {
            case OldState.RideBike:
                {
                    CurAnimator.SetBool("isOldRide", true);
                    CurAnimator.SetBool("isOldPush", false);
                    CurAnimator.SetBool("isOldWalk", false);
                }break;
            case OldState.Walk:
                {
                    CurAnimator.SetBool("isOldRide", false);
                    CurAnimator.SetBool("isOldPush", false);
                    CurAnimator.SetBool("isOldWalk", true);
                } break;
            case OldState.PushBike:
                {
                    CurAnimator.SetBool("isOldRide", false);
                    CurAnimator.SetBool("isOldPush", true);
                    CurAnimator.SetBool("isOldWalk", false);
                } break;
        }
    }

    void UpdateOldState()
    {
        if(SceneMgr.curtainIndex==5)
        {
            if(SceneMgr.curSceneIndex==2||SceneMgr.curSceneIndex==3)
            {
                SwitchByOldState(OldState.PushBike);
            }
            else if(SceneMgr.curSceneIndex==0||
                    SceneMgr.curSceneIndex==1 )
            {
                SwitchByOldState(OldState.RideBike);
            }
            else if(SceneMgr.curSceneIndex==4)
            {
                SwitchByOldState(OldState.Walk);
            }
        }
    }
    
    void UpdateSpeed()
    {
        if(SkyMgr.curState==SkyState.Day)
        {
            switch (SceneMgr.curtainIndex)
            {
                case 2:
                    {
                        CurSpeed = speed2;
                        CurDragForce = dragForce2;
                    } break;
                case 3:
                    {
                        CurSpeed = speed3;
                        CurDragForce = dragForce3;
                    } break;
                case 4:
                    {
                        CurSpeed = speed4;
                        CurDragForce = dragForce4;
                    } break;
                case 5:
                    {
                        CurSpeed = speed5;
                        CurDragForce = dragForce5;
                    } break;
            }
        }
        else if (SkyMgr.curState == SkyState.Night)
        {
            CurSpeed = darkSpeed;
            CurDragForce = darkDragForce;
        }
        
    }
    

    void OnCurtainSwitch(EventData data)
    {
        if (SceneMgr.curtainIndex == 2)
        {
            CurAnimator.SetBool("isNoWind", true);
        }
        else
        {
            CurAnimator.SetBool("isNoWind", false);
        }
        if (SceneMgr.curtainIndex == 3)
        {
            CurAnimator.SetBool("isWind", true);
        }
        else
        {
            CurAnimator.SetBool("isWind", false);
        }
        if((int)data.data==4)
        {
            Idle();
            CurAnimator.SetBool("isFamly", true);
        }
    }
    public void ForbidMoveLeft(bool isForbid)
    {
        isForbidMoveLeft = isForbid;
    }
    public void ForbidMoveRight(bool isForbid)
    {
        isForbidMoveRight = isForbid;
    }
    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.DayNightChange, OnUpdateLightView);
        EventManager.removeEventListener(EventType.SwitchCurtain, OnCurtainSwitch);
    }
    void OnUpdateLightView(EventData data)
    {
        if(lightling!=null)
        {
            SkyState state = (SkyState)data.data;
            if (state == SkyState.Day)
            {
                lightling.SetActive(false);

            }
            else
            {
                lightling.SetActive(true);

            }
        }
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (SceneMgr.curtainIndex == 2 || 
            SceneMgr.curtainIndex == 3 || 
            SceneMgr.curtainIndex == 4 || 
            SceneMgr.curtainIndex == 5)
        {
            if (col.gameObject.name == "Engage_right")
            {
                SceneMgr.Switch2NextScene();
            }
            else if (col.gameObject.name == "Engage_left")
            {
                SceneMgr.Switch2PreScene();
            }
        }
    }
	// Update is called once per frame
	void Update () 
    {
        UpdateOldState();
        UpdateSpeed();
	}
    void FixedUpdate()
    {
        if(SceneMgr.curtainIndex==2||SceneMgr.curtainIndex==3)
        {
            AddDragForce();
        }
        AddDownForce();
        if(!ForbidCtrl)
        {
            if (Input.GetKey(KeyCode.A) || LeftBtnDown) //左移动
            {
                if(!isForbidMoveLeft)
                {
                    MoveLeft();
                }
                
            }
            else if(Input.GetKey(KeyCode.D) || RightBtnDown)
            {
                if(!isForbidMoveRight)
                {
                    MoveRight();
                }
                
            }
            else
            {
                Idle();
            }
        
        }
    }

    public void MoveLeft()
    {
        base.MoveLeft();
        CurAnimator.SetBool("isStop", false);
    }
    public void MoveRight()
    {
        base.MoveRight();
        CurAnimator.SetBool("isStop", false);
    }

    public void Idle()
    {
        QuickStop();
        CurAnimator.SetBool("isStop", true);
        
    }
    void InitPostion()
    {
        if (SceneMgr.curtainIndex == 2 || SceneMgr.curtainIndex == 3 || SceneMgr.curtainIndex == 4 || SceneMgr.curtainIndex == 5)
        {
            if (SceneMgr.curSceneIndex == 0 && !SceneMgr.isLeft2Right)
            {
                TurnLeft();
                this.gameObject.transform.localPosition = new Vector3(8.91f, -2.92f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            if (SceneMgr.curSceneIndex == 1 && SceneMgr.isLeft2Right)
            {
                TurnRight();
                this.gameObject.transform.localPosition = new Vector3(-8.96f, -2.92f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            if (SceneMgr.curSceneIndex == 1 && !SceneMgr.isLeft2Right)
            {
                TurnLeft();
                this.gameObject.transform.localPosition = new Vector3(8.91f, -2.74f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            if (SceneMgr.curSceneIndex == 2 && SceneMgr.isLeft2Right)
            {
                TurnRight();
                this.gameObject.transform.localPosition = new Vector3(-9.03f, -1.98f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 180, -20);
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            if (SceneMgr.curSceneIndex == 2 && !SceneMgr.isLeft2Right)
            {
                TurnLeft();
                this.gameObject.transform.localPosition = new Vector3(8.43f, -0.01f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 0, -25);
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            if (SceneMgr.curSceneIndex == 3 && SceneMgr.isLeft2Right)
            {
                TurnRight();
                this.gameObject.transform.localPosition = new Vector3(-8.52f, -0.67f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 180, 20);
                this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }
}
