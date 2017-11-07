using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifeCtrl : BaseActor {

    public GameObject target;
    public bool CancelFollow = false;
    // Use this for initialization
    void Start()
    {
        SetOriginDirect(Direction.LEFT);
        Init();
        Idle();
        //TurnLeft();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        AddDragForce();
        AddDownForce();
        if(!CancelFollow)
        {
            if (GlobalMethods.GetDistance(this.gameObject, target) > 0.3f)
            {
                Move2Target();
            }
            else
            {
                Idle();
            }
        }
        else
        {
            Idle();
        }
        
    }
    void Move2Target()
    {
        if (target.gameObject.transform.localPosition.x < this.gameObject.transform.localPosition.x)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
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
        base.QuickStop();
        CurAnimator.SetBool("isStop", true);
    }
}
