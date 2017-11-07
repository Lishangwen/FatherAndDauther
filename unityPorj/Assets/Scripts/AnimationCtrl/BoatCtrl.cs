using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCtrl:BaseActor
{
    public GameObject sound;

    public bool isReachLeft = false;
    //float startTime = 0;  // 开始碰到左侧的时间
	// Use this for initialization

    private bool isRightMove = false;

    private bool isDestroy = false;

	void Start () 
    {
        isDestroy = false;
        SetOriginDirect(Direction.LEFT);
        Init();
        //MoveLeft();  // 初始默认向左移动
	}

    private void OnDestroy()
    {
        isDestroy = true;
    }




    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.name == "Engage_right")
        {
            isReachLeft = true;
            

        }
            /*
        else if (col.gameObject.name == "boatEngage_right")
        {
            Idle();
            //MoveLeft();
        }
         * */
    }
	// Update is called once per frame
	void Update () 
    {
		
	}
    public void Idle()
    {
        base.QuickStop();
        sound.SetActive(false);
        CurAnimator.SetBool("isStop", true);
    }

    public void MoveLeft()
    {
        base.MoveLeft();
        sound.SetActive(true);
        CurAnimator.SetBool("isStop", false);
    }
    public void MoveRight()
    {
        base.MoveRight();
        
        sound.SetActive(true);
        CurAnimator.SetBool("isStop", false);

        if (!isRightMove)
        {
            AudioPlayerManager.Instance.DelayInvoke(3.5f, () =>
            {
                if (!isDestroy)
                {
                    gameObject.SetActive(false);
                }
            });
            isRightMove = false;
        }
    }
    void FixedUpdate()
    {
        AddDragForce();
        AddDownForce();
        /*
        if (isReachLeft)
        {
            if (GlobalState.startTalk || (TimeMgr.GameTime - startTime) < 5)
            {
                Idle();
            }
            else
            {
                isReachLeft = false;
                MoveRight();
            }
        }
        else if (CurDirect==Direction.LEFT)
        {
            MoveLeft();
        }
        else if (CurDirect == Direction.RIGHT)
        {
            MoveRight();
        }
         * */
    }
}
