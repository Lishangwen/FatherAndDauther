using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatherOnBikeCtrl :BaseActor 
{
    public Animator bikeAnimator;
    public Animator fatherAnimator;
    public GameObject HeadTip;
    public GameObject bike;
    public GameObject father;

    public Button leftBtn;
    public Button rightBtn;
    void Start()
    {
        //bikeAnimator = bike.GetComponent<Animator>();
        //fatherAnimator = father.GetComponent<Animator>();
        SetOriginDirect(Direction.RIGHT);
        Init();

        EventTriggerListener.GetListener(leftBtn.gameObject).onPointerDown += OnLeftBtnDown;
        EventTriggerListener.GetListener(rightBtn.gameObject).onPointerDown += OnRightBtnDown;
        EventTriggerListener.GetListener(leftBtn.gameObject).onPointerUp += OnLeftBtnUp;
        EventTriggerListener.GetListener(rightBtn.gameObject).onPointerUp += OnRightBtnUp;

        if(GlobalState.hugGirlComplete)
        {
            fatherAnimator.SetBool("girlOn", true);
        }
        if(SceneMgr.curtainIndex==1&&SceneMgr.curSceneIndex==0)
        {
            HeadTip.SetActive(true);
        }
        else
        {
            HeadTip.SetActive(false);
        }
    }

    
    void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.name == "Engage_right")&&GlobalState.hugGirlComplete)
        {
            SceneMgr.Switch2NextScene();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (ShouldChangeCurtain())
        {
            //SceneMgr.Switch2NextCurtain();
            //TimeMgr.GameTime += (120 - TimeMgr.GameTime % 120);
        }
        
    }

    bool ShouldChangeCurtain()
    {
        bool shouldChange = false;
        if((TimeMgr.GameTime%120)>49&&SceneMgr.curtainIndex==1&&SceneMgr.curSceneIndex==2)
        {
            shouldChange = true;
        }
        return shouldChange;
    }
    void OnMouseDown()
    {
        EventManager.dispatchEvent(EventType.StartHugGirl, null);
        EventManager.dispatchEvent(EventType.FatherOclick, null);
        HeadTip.SetActive(false);
    }
    void FixedUpdate()
    {
        AddDragForce();
        AddDownForce();
        if(!ForbidCtrl)
        {
            if (Input.GetKey(KeyCode.A)||LeftBtnDown) //左移动
            {
                MoveLeft();
            }
            else if (Input.GetKey(KeyCode.D)||RightBtnDown)  // 右移动
            {
                MoveRight();
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
        if (bikeAnimator == null || fatherAnimator == null)
        {
            return;
        }
        bikeAnimator.SetBool("isStop", false);
        fatherAnimator.SetBool("isStop", false);
    }
    public void MoveLeft_1()
    {
        base.MoveLeft();

    }
    public void MoveRight()
    {
        base.MoveRight();
        if (bikeAnimator == null || fatherAnimator == null)
        {
            return;
        }
        bikeAnimator.SetBool("isStop", false);
        fatherAnimator.SetBool("isStop", false);
    }
    
    public void Idle()
    {
        if(bikeAnimator==null||fatherAnimator==null)
        {
            return;
        }
        bikeAnimator.SetBool("isStop", true);
        fatherAnimator.SetBool("isStop", true);
    }
}
