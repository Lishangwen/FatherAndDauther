using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCtrl : MonoBehaviour {

    public GameObject host; // 狗主人
    private Animator myAnimator;
    private bool isLeft = false;
    private bool isSparked = false;
    private float distanceThreshold = 4;
    private float offsetDistance = 0.5f;
    private int frameCount = 0;
    private Rigidbody2D rigid;
    void Start()
    {
        myAnimator = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Engage_right")
        {
            SceneMgr.Switch2NextScene();
        }
        
        //Application.LoadLevel("Scene2");
    }
    // Update is called once per frame
    void Update()
    {
        if(GetHostDistance()<distanceThreshold-0.5)
        {
            frameCount = 0;
            WalkRight();
        }
        else if (GetHostDistance() >= distanceThreshold - 0.5 && GetHostDistance() <= distanceThreshold + 0.5)
        {
            if(!isSparked&&!isLeft&&frameCount<40)
            {
                frameCount++;
                Idle();
                Spark();
                
            }
            else 
            {
                isSparked = true;
                StopSpark();
                Idle();
            }

        }
        else if(GetHostDistance()>distanceThreshold+0.5)
        {
            frameCount = 0;
            WalkLeft();
        }
    }


    float GetHostDistance()
    {
        float distance = 0;
        Vector3 hostPosition=host.gameObject.transform.position;
        Vector3 dogPosition=gameObject.transform.position;
        distance = Vector3.Distance(hostPosition, dogPosition);
        return distance;
    }
    void WalkLeft()
    {
        TurnLeft();
        isSparked = false;
        isLeft = true;
        //this.transform.localPosition += new Vector3(-2.0f * Time.deltaTime, 0, 0);
        rigid.velocity = new Vector3(-2, 0);
        myAnimator.SetBool("isStop", false);
    }
    void WalkRight()
    {
        TurnRight();
        isSparked = false;
        isLeft = false;
        //this.transform.localPosition += new Vector3(2.0f * Time.deltaTime, 0, 0);
        rigid.velocity = new Vector3(2, 0);
        myAnimator.SetBool("isStop", false);
    }
    void Spark()
    {
        myAnimator.SetBool("isSpark", true);
    }
    void StopSpark()
    {
        myAnimator.SetBool("isSpark", false);
    }
    void Idle()
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
    void TurnLeft()
    {
        this.transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }
    void TurnRight()
    {
        this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
