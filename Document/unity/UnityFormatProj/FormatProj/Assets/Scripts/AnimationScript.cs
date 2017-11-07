using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {
    private Animator myAnimator;
    private bool isLeft=false;
	// Use this for initialization
	void Start () 
    {
        myAnimator = this.GetComponent<Animator>();
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Engage_right")
        {
            SceneMgr.Switch2NextScene();
        }
        else if(col.gameObject.name == "Engage_left")
        {
            SceneMgr.Switch2PreScene();
        }
    }
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKey(KeyCode.A)) //左移动
        {
            TurnLeft();
            isLeft = true;
            this.transform.localPosition += new Vector3(-2.0f * Time.deltaTime, 0, 0);
            myAnimator.SetBool("isStop", false);
        }
        if(Input.GetKey(KeyCode.D))  // 右移动
        {
            TurnRight();
            isLeft = false;
            this.transform.localPosition += new Vector3(2.0f * Time.deltaTime, 0, 0);
            myAnimator.SetBool("isStop", false);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (!isLeft)
            {
                TurnRight();
            }
            else
            {

                TurnLeft();
            }
            myAnimator.SetBool("isStop", true);
        }
        
	}
    void TurnLeft()
    {
        this.transform.eulerAngles = new Vector3(0f,0,0f);
    }
    void TurnRight()
    {
        this.transform.eulerAngles = new Vector3(0f, 180, 0f);
    }
}
