using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDirector_21 : MonoBehaviour 
{
    enum State
    {
        None,
        BegainDownBike,
        DownBikeComplete,
        BegainClampBike,
        ClampBikeComplete
    }
    public GameObject doorClose;
    public GameObject doorOpen;
    public GameObject dog;


    public GameObject lightOn;        // 夜间灯光相关
    public GameObject lightOff;
    public GameObject pretendBike;    // 假装的用于展示的Bike
    public GameObject lightling; // 手电筒
    public GameObject enterRoomPoint; // 进屋子的判定点
    public GameObject girl;
    private GirlCtrl girlCtrl;

    public GameObject wife;
    public GameObject husband;

    Vector3 wifePosition;
    Vector3 husbandPosition;
    // 需要根据天气调整颜色的房屋部件
    public SpriteRenderer house;
    public SpriteRenderer housewall;
    public SpriteRenderer housewallback;
    public SpriteRenderer doorCloseSprite;
    public SpriteRenderer doorOpenSprite;


    State curState = State.None;

    public GameObject bg1_3;
    public GameObject bg3;
    public GameObject bg4;
    public GameObject bg5;

    bool isSetDownBike = false;
    bool isSetClampBike = false;
	// Use this for initialization
	void Start () 
    {
        if (SceneMgr.curtainIndex != 2)
        {
            dog.SetActive(false);
        }
        
        girlCtrl = girl.GetComponent<GirlCtrl>();
        CloseDoor();
        if(SkyMgr.curState==SkyState.Day)
        {
            lightOn.SetActive(false);
            lightOff.SetActive(false);
        }
        else
        {
            lightOn.SetActive(true);
            lightOff.SetActive(true);
        }
        EventManager.addEventListener(EventType.DayNightChange, OnUpdateLightView);

        wifePosition = wife.gameObject.transform.localPosition;
        husbandPosition = husband.gameObject.transform.localPosition;
	}

    void OnUpdateLightView(EventData data)
    {
        SkyState state = (SkyState)data.data;
        if (state == SkyState.Day)
        {
            lightOn.SetActive(false);
            lightOff.SetActive(false);
        }
        else
        {
            lightOn.SetActive(true);
            lightOff.SetActive(true);
        }
    }
    void CloseDoor()
    {
        if (doorOpen.activeSelf)
        {
            AudioManager.Instance.SetVolume(AudioPlayerType.Sound, 0.7f);
            AudioPlayerManager.Instance.PlaySoundById(0);
        }

        doorClose.SetActive(true);
        doorOpen.SetActive(false);
    }
    void OpenDoor()
    {
        if (doorOpen.activeSelf == false)
        {
            AudioManager.Instance.SetVolume(AudioPlayerType.Sound, 0.7f);
            AudioPlayerManager.Instance.PlaySoundById(4);
            
        }
        
        doorClose.SetActive(false);
        doorOpen.SetActive(true);
    }

    void GirlDownBike()
    {
        if(!isSetDownBike)
        {
            girlCtrl.ForbidContrl(true);
            girlCtrl.Idle();
            girlCtrl.CurAnimator.SetBool("isDownBike", true);
            isSetDownBike = true;
        }
        
        AnimatorStateInfo animatorInfo;
        animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsTag("Idle1")||animatorInfo.IsTag("Walk1"))
        {
            pretendBike.SetActive(true);
            if(SceneMgr.curtainIndex==4)
            {
                 wife.gameObject.transform.localPosition=wifePosition ;
                 husband.gameObject.transform.localPosition=husbandPosition ;
                husband.SetActive(true);
                wife.SetActive(true);
            }
            girlCtrl.ForbidContrl(false);
            curState = State.DownBikeComplete;
            isSetDownBike = false;
            girl.gameObject.transform.localPosition -= new Vector3(0.3f, 0, 0);
        }
    }

    void GirlClampBike()
    {
        if(!isSetClampBike)
        {
            girlCtrl.ForbidContrl(true);
            girlCtrl.Idle();
            girlCtrl.CurAnimator.SetBool("isDownBike", false);
            pretendBike.SetActive(false);
            husband.SetActive(false);
            wife.SetActive(false);
            isSetClampBike = true;
        }
        
        AnimatorStateInfo animatorInfo;
        animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsTag("Idle") || animatorInfo.IsTag("Idle2") || animatorInfo.IsTag("Walk2"))
        {
            girlCtrl.ForbidContrl(false);
            curState = State.ClampBikeComplete;
            isSetClampBike = false;
            girl.gameObject.transform.localPosition += new Vector3(0.3f, 0, 0);
        }
    }
    void UpdateDoorState()
    {
        float a=GlobalMethods.GetDistance(girl,enterRoomPoint);
        if(a<0.2f)
        {
            AnimatorStateInfo animatorInfo;
            animatorInfo = girlCtrl.CurAnimator.GetCurrentAnimatorStateInfo(0);
            if (girlCtrl.CurDirect == Direction.LEFT&&girl.gameObject.transform.localPosition.x>enterRoomPoint.gameObject.transform.localPosition.x)
            {
                //lightling.SetActive(false);
                //if(SceneMgr.curtainIndex!=4)
                {
                    if (curState != State.DownBikeComplete)
                    {
                        GirlDownBike();
                    }
                }
               
                OpenDoor();
            }
            else if (girlCtrl.CurDirect == Direction.RIGHT&&girl.gameObject.transform.localPosition.x <= enterRoomPoint.gameObject.transform.localPosition.x)
            {
                //if (SceneMgr.curtainIndex != 4)
                {
                    if (curState != State.ClampBikeComplete)
                    {
                        GirlClampBike();
                    }
                }
                if(SkyMgr.curState==SkyState.Night)
                {
                    lightling.SetActive(true);
                }
                CloseDoor();
            }
        }
        else
        {
            float b = girl.transform.position.x - enterRoomPoint.transform.position.x;
            if(b < -2f && b >-2.2f){
                if(girlCtrl.CurDirect==Direction.LEFT) CloseDoor();
                else OpenDoor();
            }
            
                
            if(SkyMgr.curState==SkyState.Night)
            {
                if(b <= -2.2f)  OpenDoor();
                if (b < -1.8f) lightling.SetActive(false);
                else lightling.SetActive(true);
            }
            curState = State.None;
        }
    }

    // 根据天色改变房屋的颜色值
    void UpdateHouseColor()
    {
        float curClorValue=1.1f-SkyMgr.Alphavalue;
        Color curColor = new Color(curClorValue, curClorValue, curClorValue, 1);
        house.color = curColor;
        housewall.color = curColor;
        housewallback.color = curColor;
        doorCloseSprite.color = curColor;
        doorOpenSprite.color = curColor;
    }
	// Update is called once per frame
	void Update () 
    {
        if (SceneMgr.curtainIndex != 2)
        {
            dog.SetActive(false);
        }
       
        if(SceneMgr.curtainIndex==4)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(true);
            bg5.SetActive(false);
            bg3.SetActive(false);
        }
        else if(SceneMgr.curtainIndex==5)
        {
            bg1_3.SetActive(false);
            bg4.SetActive(false);
            bg5.SetActive(true);
            bg3.SetActive(false);
        }
        else if (SceneMgr.curtainIndex == 3)
        {
            bg1_3.SetActive(false);
            bg3.SetActive(true);
            bg4.SetActive(false);
            bg5.SetActive(false);
        }
        else
        {
            bg1_3.SetActive(true);
            bg4.SetActive(false);
            bg5.SetActive(false);
            bg3.SetActive(false);
        }
	}
    void FixedUpdate()
    {
        UpdateDoorState();
        UpdateHouseColor();
    }
    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.DayNightChange, OnUpdateLightView);
    }
}
