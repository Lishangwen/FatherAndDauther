using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCtrl : BaseActor 
{
    public GameObject host; // 狗主人
    public GameObject father;
   
    private bool isSparked = false;  // 是否已经叫过
    private bool isInEngage = false;  // 是否到达边界
    private float distanceThreshold = 4;
    private float offsetDistance = 0.5f;

    public AudioClip[] clips;

    private AudioSource audioSource;

    bool cancelFollow = false;
    
    void Start()
    {
        SetOriginDirect(Direction.RIGHT);
        Init();
        InitPosition();
        audioSource = GetComponent<AudioSource>();
    }
    public void CancelFollow(bool isCancel)
    {
        cancelFollow = isCancel;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Engage_right")
        {
            isInEngage = true;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == "Engage_right")
        {
            isInEngage = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        AddDragForce();
        AddDownForce();
        if(!cancelFollow)
        {
            if (host == null || !host.activeSelf)
            {
                host = father;
            }

            if ((GetHostDistance() < distanceThreshold - 0.5) && !isInEngage)
            {
                LeaveHost();
            }
            else if ((GetHostDistance() >= distanceThreshold - 0.5 && GetHostDistance() <= distanceThreshold + 0.5) || isInEngage)
            {
				float hostspeed = host.gameObject.GetComponent<Rigidbody2D> ().velocity.x;
				if (!isSparked && Mathf.Abs(hostspeed) < 0.05)
                {
                    isSparked = true;
                    EventManager.dispatchEvent(EventType.DogSpark, null);
                    Spark();
                }
            }
            else if ((GetHostDistance() > distanceThreshold + 0.5) && !isInEngage)
            {
                WalkToHost();
            }
        }
    }
    void WalkToHost()
    {
        Vector3 hostPosition = host.gameObject.transform.position;
        Vector3 dogPosition = gameObject.transform.position;
        if(hostPosition.x>dogPosition.x)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }
    void LeaveHost()
    {
        Vector3 hostPosition = host.gameObject.transform.position;
        Vector3 dogPosition = gameObject.transform.position;
        if (hostPosition.x > dogPosition.x)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
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
    public void MoveLeft()
    {
        base.MoveLeft();
        isSparked = false;
        CurAnimator.SetFloat("speed", CurSpeed);
        CurAnimator.SetBool("isSpark", false);
    }
    public void MoveRight()
    {
        base.MoveRight();
        isSparked = false;
        CurAnimator.SetFloat("speed", CurSpeed);
        CurAnimator.SetBool("isSpark", false);
    }
    void Spark()
    {
        Idle();
        CurAnimator.SetBool("isSpark", true);
        StartCoroutine(PlayAudio());
    }
    
    public void Idle()
    {
        QuickStop();
        CurAnimator.SetBool("isSpark", false);
        CurAnimator.SetFloat("speed",0);
    }
    

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        if (!audioSource.isPlaying)
        {
            if (SceneMgr.curtainIndex == 1)
            {
                audioSource.PlayOneShot(clips[0]);
            }
            else if (SceneMgr.curtainIndex == 2)
            {
                audioSource.PlayOneShot(clips[1]);
            }
        }
    }
    void InitPosition()
    {
        if (SceneMgr.curtainIndex == 2 || SceneMgr.curtainIndex==3)
        {
            if (SceneMgr.curSceneIndex == 0 && !SceneMgr.isLeft2Right)
            {
                TurnLeft();
                this.gameObject.transform.localPosition = new Vector3(7.16f, -3.57f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            if (SceneMgr.curSceneIndex == 1 && SceneMgr.isLeft2Right)
            {
                TurnRight();
                this.gameObject.transform.localPosition = new Vector3(-6.98f, -3.57f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }

            if (SceneMgr.curSceneIndex == 1 && !SceneMgr.isLeft2Right)
            {
                TurnLeft();
                this.gameObject.transform.localPosition = new Vector3(7.06f, -3.56f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }

            if (SceneMgr.curSceneIndex == 2 && SceneMgr.isLeft2Right)
            {
                TurnRight();
                this.gameObject.transform.localPosition = new Vector3(-6.97f, -1.25f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 0, 20);
                this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }

            if (SceneMgr.curSceneIndex == 2 && !SceneMgr.isLeft2Right)
            {
                TurnLeft();
                this.gameObject.transform.localPosition = new Vector3(6.76f, 0.29f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 180, 32);
                this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            if (SceneMgr.curSceneIndex == 3 && SceneMgr.isLeft2Right)
            {
                TurnRight();
                this.gameObject.transform.localPosition = new Vector3(-7.21f, -1.74f, 0);
                this.gameObject.transform.eulerAngles = new Vector3(0, 0, -20);
                this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
        }
    }
}
