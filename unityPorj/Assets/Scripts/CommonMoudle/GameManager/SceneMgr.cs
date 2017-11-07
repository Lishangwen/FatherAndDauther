using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour 
{

	// Use this for initialization
    static public bool isLeft2Right = true;
    static List<string> sceneListCurtain2 = new List<string>() { "Scene2_1","Scene2_2","Scene2_3","Scene2_4","Scene5_5","End"};
    static List<string> sceneListCurtain1 = new List<string>() { "Scene1_2", "Scene1_3", "Scene1_4" };
    static public int curSceneIndex;            // 当前场景编号
    static public int curtainIndex=0;           // 幕的序号
    static float lastChangeTime = 0;           // 上一次进行切换到上一幕操作的时间
    static float lastChangeTime_1 = 0;         // 上一次进行切换到下一幕操作的时间


    int InfoCunter = 0;
	void Start () 
    {
        DontDestroyOnLoad(this);
        EventManager.addEventListener(EventType.DayNightChange, OnChange2Curtain3);
	}

    public static void RestartGame()
    {
        InitFirstCurtain();
    }
    void OnChange2Curtain3(EventData data)
    {
        SkyState state = (SkyState)data.data;
        if(curtainIndex==2&&state==SkyState.Day)
        {
            InfoCunter++;
            if(InfoCunter==2)
            {
                Switch2NextCurtain();
            }
        }
        else if(curtainIndex==3&&state==SkyState.Day)
        {
            Switch2NextCurtain();
        }
        else if(curtainIndex==4&&state==SkyState.Day)
        {
            Switch2NextCurtain();
        }
    }

    public static void InitFirstCurtain()
    {
        curSceneIndex = 0;
        curtainIndex = 1;
        SceneManager.LoadScene(sceneListCurtain1[curSceneIndex]);
        EventManager.dispatchEvent(EventType.SwitchCurtain, curtainIndex);
    }
	public static void Switch2NextScene()
    {
        if ((TimeMgr.GameTime - lastChangeTime_1) > 0.2f)   // 0.2秒内的连续切换忽略
        {
            lastChangeTime_1 = TimeMgr.GameTime;
            if (curSceneIndex < 3)
            {
                isLeft2Right = true;
                curSceneIndex++;
                ChangeScene();
            }
            else if (curtainIndex == 5 && curSceneIndex < 5)
            {
                isLeft2Right = true;
                curSceneIndex++;
                ChangeScene();
            }
        }
    }
    public static void Switch2PreScene()
    {
        if(curSceneIndex>0)
        {
            if((TimeMgr.GameTime-lastChangeTime)>0.2f)   // 0.2秒内的连续切换忽略
            {
                lastChangeTime = TimeMgr.GameTime;
                isLeft2Right = false;
                curSceneIndex--;
                ChangeScene();
            }
            
        }
    }

    // 切换到下一幕
    public static void Switch2NextCurtain()
    {
        if (curtainIndex<5)
        {
            curtainIndex++;
            GlobalState.startTalk = false;
            GlobalState.isTalked = false;
            GlobalState.isEnteredWharf = false;
            if (SceneMgr.curtainIndex == 5)
            {
                SkyMgr.IgnoreSkyState(true);
                SkyMgr.ResetSkyTime(0.61f);
            }
            if(curSceneIndex==3||(curtainIndex==2&&curSceneIndex==2))
            {
                isLeft2Right = true;
                curSceneIndex = 0;
                ChangeScene();
            }
            EventManager.dispatchEvent(EventType.SwitchCurtain, curtainIndex);
            
        }
    }

    static void ChangeScene()
    {
        if(curtainIndex==1)
        {
            if(curSceneIndex<sceneListCurtain1.Count)
            {
                SceneManager.LoadScene(sceneListCurtain1[curSceneIndex]);
            }
            else
            {
                curSceneIndex = sceneListCurtain1.Count - 1;
            }
        }
        else if (curtainIndex == 2 || curtainIndex == 3 || curtainIndex == 4 || curtainIndex == 5)
        {
            SceneManager.LoadScene(sceneListCurtain2[curSceneIndex]);
        }
        if(curtainIndex==5)
        {

        }
    }
	// Update is called once per frame
	void Update () 
    {
		
	}
}
