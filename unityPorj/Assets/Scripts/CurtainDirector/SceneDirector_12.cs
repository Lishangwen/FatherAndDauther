using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector_12 : MonoBehaviour 
{
    public GameObject girlWalk;
    public GameObject fatherOnBike;
    public GameObject dog;

    GirlCtrl girlCtrl;
    FatherOnBikeCtrl fatherCtrl;
    DogCtrl dogCtrl;
    float timeCounter=3;
	// Use this for initialization
	void Start () 
    {
        girlCtrl = girlWalk.GetComponent<GirlCtrl>();
        fatherCtrl = fatherOnBike.GetComponent<FatherOnBikeCtrl>();
        dogCtrl = dog.GetComponent<DogCtrl>();
        girlCtrl.ForbidContrl(false);
        fatherCtrl.ForbidContrl(true);
        fatherCtrl.Idle();
        EventManager.addEventListener(EventType.StartHugGirl, OnStartHugGirl);
        BgmCtrl.PlayBGM(BGM.BGM1_1);
	}
	
    void OnStartHugGirl(EventData data)
    {
        GlobalState.startHugGirl = true;
        girlCtrl.ForbidContrl(true);
        
    }

    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.StartHugGirl, OnStartHugGirl);
    }
	// Update is called once per frame
	void Update () 
    {
		
	}

    void FixedUpdate()
    {
        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            timeCounter = 0;
        }
        SkyMgr.BecomeDark(3, timeCounter);

        if (GlobalState.startHugGirl&&!GlobalState.hugGirlComplete)
        {
            if (GetDistance(girlWalk,fatherOnBike)>0.9f)
            {
                girlCtrl.ForbidContrl(true);
                MoveToGirl();
            }
            else
            {
                fatherCtrl.Idle();
                AnimatorStateInfo animatorInfo;
                animatorInfo = fatherCtrl.fatherAnimator.GetCurrentAnimatorStateInfo(0);
                if (animatorInfo.IsTag("OnBike"))
                {
                    fatherCtrl.fatherAnimator.SetBool("startHug", false);

                    GlobalState.hugGirlComplete = true;
                }
                else if (animatorInfo.IsTag("hug"))
                {
                    girlWalk.SetActive(false);
                }
                else if (!GlobalState.hugGirlComplete)
                {
                    fatherCtrl.fatherAnimator.SetBool("startHug", true);
                }
            }
            
        }
        else if(GlobalState.hugGirlComplete)
        {
            fatherCtrl.ForbidContrl(false);
            fatherCtrl.fatherAnimator.SetBool("girlOn", true);
            
        }
    }

    void MoveToGirl()
    {
        Vector3 girlPosition = girlCtrl.gameObject.transform.position;
        Vector3 fatherPosition = fatherCtrl.transform.position;
        if (girlPosition.x > fatherPosition.x)
        {
            fatherCtrl.MoveRight();
        }
        else
        {
            fatherCtrl.MoveLeft();
        }
    }
    float GetDistance(GameObject a,GameObject b)
    {
        float distance = 0;
        Vector3 aPosition = a.transform.position;
        Vector3 bPosition = b.transform.position;
        distance = Vector3.Distance(aPosition, bPosition);
        return distance;
    }
}
