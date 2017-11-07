using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Direction
{
    LEFT,
    RIGHT
}
public class BaseActor : MonoBehaviour 
{
    public Animator myAnimator;
    private Direction curDirect;        // 当前朝向是否向左
    private Direction originDirect;      //动画初始朝向是否向右
    public Rigidbody2D rigid;

    public float Speed;            // 速度
    public float dragForce;        // 上坡时阻力


    bool forbidCtrl = false;   // 禁止控制
    bool isLeftDown = false;  //左侧按钮按下
    bool isRightDown = false;  // 右侧按钮按下

    public Animator CurAnimator
    {
        get { return myAnimator; }
        set { myAnimator = value; }
    }

    public bool LeftBtnDown
    {
        get { return isLeftDown; }
    }

    public bool RightBtnDown
    {
        get { return isRightDown; }
    }
    public bool ForbidCtrl
    {
        get { return forbidCtrl; }
    }

    public float CurSpeed
    {
        get { return rigid.velocity.magnitude; }
        set { Speed = value; }
    }
    public float CurDragForce
    {
        get { return dragForce; }
        set { dragForce = value; }
    }
    public Direction CurDirect
    {
        get { return curDirect; }
        set { curDirect = value; }
    }
    public void ForbidContrl(bool isfobid)
    {
        forbidCtrl = isfobid;
    }
    void InitActorDirect()
    {
        if(curDirect==Direction.LEFT)
        {
            TurnLeft();
        }
        else
        {
            TurnRight();
        }
    }
   
    public void Init()
    {
        myAnimator = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        curDirect = Direction.RIGHT;
        InitActorDirect();
    }
    // 设置动画的朝向，有些动画初始朝左，有些初始朝右
    public void SetOriginDirect(Direction direct)
    {
        originDirect = direct;
    }

    // 正常情况会因为摩擦力而停止，调用此方法可以立即停止
    public void QuickStop()
    {
        rigid.velocity = new Vector2(0,0);
    }
    public void MoveRight()
    {
        if (curDirect==Direction.LEFT)
        {
            TurnRight();
        }
        curDirect = Direction.RIGHT;
        float theta = gameObject.transform.localEulerAngles.z;

        if(originDirect==Direction.LEFT)
        {
            theta = -theta;
            rigid.velocity = Speed * new Vector2(Mathf.Cos(theta * Mathf.PI / 180.0f), Mathf.Sin(theta * Mathf.PI / 180.0f));
        }
        else if(originDirect==Direction.RIGHT)
        {
            rigid.velocity = Speed * new Vector2(Mathf.Cos(theta * Mathf.PI / 180.0f), Mathf.Sin(theta * Mathf.PI / 180.0f));
        }
        
    }
    public void MoveLeft()
    {
        if (curDirect==Direction.RIGHT)
        {
            TurnLeft();
        }
        curDirect = Direction.LEFT;
        float theta = gameObject.transform.localEulerAngles.z;
        if (originDirect == Direction.LEFT)
        {
            theta = -theta;
            rigid.velocity = Speed * new Vector2(-Mathf.Cos(theta * Mathf.PI / 180.0f), Mathf.Sin(theta * Mathf.PI / 180.0f));
        }
        else if (originDirect == Direction.RIGHT)
        {
            rigid.velocity = Speed * new Vector2(-Mathf.Cos(theta * Mathf.PI / 180.0f), Mathf.Sin(theta * Mathf.PI / 180.0f));
        }
        
    }
    public void TurnLeft()
    {
        curDirect =Direction.LEFT;
        if (originDirect == Direction.LEFT)
        {
            if (gameObject.transform.localEulerAngles.y > 0.5 || gameObject.transform.localEulerAngles.y < -0.5)
            {
                this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, 0, -this.transform.localEulerAngles.z);
            }
        }
        else if (originDirect == Direction.RIGHT)
        {
            if (gameObject.transform.localEulerAngles.y > 180 + 0.5 || gameObject.transform.localEulerAngles.y < 180 - 0.5)
            {
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 180, -this.transform.eulerAngles.z);
            }
        }
        
    }
    public void TurnRight()
    {
        curDirect = Direction.RIGHT;
        if (originDirect == Direction.LEFT)
        {
            if (gameObject.transform.localEulerAngles.y > 180 + 0.5 || gameObject.transform.localEulerAngles.y < 180 - 0.5)
            {
                this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, 180, -this.transform.localEulerAngles.z);
            }
        }
        else if (originDirect == Direction.RIGHT)
        {
            if (gameObject.transform.localEulerAngles.y > 0.5 || gameObject.transform.localEulerAngles.y < -0.5)
            {
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 0, -this.transform.eulerAngles.z);
            }
        }
        
        
    }
    public void AddDragForce()
    {
        if (originDirect == Direction.LEFT)
        {
            // 在爬坡
            if (this.transform.localEulerAngles.z > 270 && this.transform.localEulerAngles.z < 350)
            {
                rigid.drag = Mathf.Abs((360-this.transform.localEulerAngles.z) * dragForce);
            }
            else
            {
                rigid.drag = 0;
            }
        }
        else if (originDirect == Direction.RIGHT)
        {
            if (this.transform.localEulerAngles.z > 10 && this.transform.localEulerAngles.z < 90)
            {
                rigid.drag = dragForce * transform.localEulerAngles.z;
            }
        }
        

    }

    // 添加沿法线向下的力
    public void AddDownForce()
    {
        float theta1 = gameObject.transform.localEulerAngles.z;
        Vector2 force=new Vector2(0,0);
        if(originDirect==Direction.LEFT)
        {
            if (curDirect == Direction.RIGHT)
            {
                theta1 = -theta1;
            }
            force = new Vector2(Mathf.Sin(theta1 * Mathf.PI / 180.0f), -Mathf.Cos(theta1 * Mathf.PI / 180.0f));
        }
        else if ((originDirect == Direction.RIGHT))
        {
            if (curDirect == Direction.LEFT)
            {
                theta1 = -theta1;
            }
            force = new Vector2(Mathf.Sin(theta1 * Mathf.PI / 180.0f), -Mathf.Cos(theta1 * Mathf.PI / 180.0f));
        }
        force = 100 * force;
        rigid.AddForce(force);
    }

    // 左侧按钮按下
    public void OnLeftBtnDown(GameObject go, BaseEventData eventData)
    {
        isLeftDown = true;
    }

    // 右侧按钮按下
    public void OnRightBtnDown(GameObject go, BaseEventData eventData)
    {
        isRightDown = true;
    }

    // 左侧按钮松开
    public void OnLeftBtnUp(GameObject go, BaseEventData eventData)
    {
        isLeftDown = false;
    }

    // 右侧按钮松开
    public void OnRightBtnUp(GameObject go, BaseEventData eventData)
    {
        isRightDown = false;
    }
}
