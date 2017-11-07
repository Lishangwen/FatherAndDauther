using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkyState
{
    Day,
    Night
}
public class SkyMgr : MonoBehaviour 
{
   
    public AnimationCurve skyChangeCurve;
    public SpriteRenderer nightMask;

    static float dayTimeLen = 80;  // 一天的时间
    float mTime;                    // 一天中的时间归一化时间，0-0.5为白天到黑夜，0.5-0为黑夜到白天
    static public SkyState curState;
    static bool  ignoreSkyChange = false;
    static float skyTime = 0;     // 用于控制天气变化的时间,单位为秒
    static float alphavalue=0;
    static float alphavalue_ignoreState=0;
	// Use this for initialization
	void Start () 
    {
        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainSwitch);
	}

    void OnCurtainSwitch(EventData data)
    {
        int curtainIndex = (int)data.data;
        if(curtainIndex==2)
        {

        }
    }
    public static float Alphavalue
    {
        get { return alphavalue; }
    }
    static public void ResetSkyTime(float value)
    {
        skyTime = value * dayTimeLen;
    }
    void UpdateSkyState()
    {
        if(mTime>0.35&&mTime<0.6)
        {
            if(curState!=SkyState.Night)
            {
                curState = SkyState.Night;
                EventManager.dispatchEvent(EventType.DayNightChange, curState);
            }
           
        }
        else
        {
            if(curState!=SkyState.Day)
            {
                curState = SkyState.Day;
                EventManager.dispatchEvent(EventType.DayNightChange, curState);
            }
            
        }
    }

    // 开始逐渐变黑
    public static void BegainFade()
    {
        IgnoreSkyState(false);
        ResetSkyTime(0.2f);
    }

    // 忽略夜色变化
    public static void IgnoreSkyState(bool igore)
    {
        ignoreSkyChange = igore;
        ResetSkyTime(0);
    }

    // 逐渐变黑
    public static void BecomeDark(float totalTime,float curTime)
    {
        alphavalue_ignoreState = curTime % totalTime / totalTime;
    }

    // 逐渐变亮
    public static void BecomeBright(float totalTime, float curTime)
    {
        alphavalue_ignoreState = Mathf.Abs(1-curTime % totalTime / totalTime);
    }
	// Update is called once per frame
	void Update () 
    {
        /*
        if(SceneMgr.curtainIndex==1||SceneMgr.curtainIndex==5)
        {
            IgnoreSkyState(true);
        }
        else
        {
            IgnoreSkyState(false);
        }
         * */
        if(!ignoreSkyChange)
        {
            mTime = skyTime % dayTimeLen / dayTimeLen;
            UpdateSkyState();
            alphavalue = skyChangeCurve.Evaluate(mTime);
            nightMask.color = new Color(1, 1, 1, alphavalue);
            skyTime += Time.deltaTime;
        }
        else
        {
            alphavalue = alphavalue_ignoreState;
            nightMask.color = new Color(1, 1, 1, alphavalue_ignoreState);
        }
        
	}
}
