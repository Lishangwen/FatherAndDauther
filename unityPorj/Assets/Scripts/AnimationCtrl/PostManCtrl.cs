using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostManCtrl : BaseActor 
{

	// Use this for initialization
	void Start () 
    {
        SetOriginDirect(Direction.RIGHT);
        Init();
        TurnLeft();
	}
    void OnMouseDown()
    {
        EventManager.dispatchEvent(EventType.PostManOnClick, null);
    }
	// Update is called once per frame
	void Update () 
    {
		
	}
    void FixedUpdate()
    {
        AddDragForce();
        AddDownForce();
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
