using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationMgr : MonoBehaviour 
{
    enum ActorID
    {
        Girl,
        PostMan
    }
    class ConversationContent
    {
        public ActorID id;
        public string message;
        public ConversationContent(ActorID mId,string mMessage)
        {
            id = mId;
            message = mMessage;
        }
    }
    //public GameObject girlHead;
   // public GameObject postManHead;
    public GameObject GirlConversationPanel;
    public GameObject postManConversationPanel;
    public Text girlMessageText;
    public Text postmanMessageText;

    public GameObject panel;
    public Button switchBtn;
    List<ConversationContent> convList = new List<ConversationContent>();
    
    bool isConvPanelInit=false;
    int counter = 0;
    
    // Use this for initialization
	void Start () 
    {
        counter = 0;
        switchBtn.onClick.AddListener(OnSwitchBtnClick);
        if(SceneMgr.curtainIndex==2)
        {
            InitConvContent2();
        }
        if(SceneMgr.curtainIndex==3)
        {
            InitConvContent3();
        }
        if(SceneMgr.curtainIndex==4)
        {
            InitConvContent4();
        }
        EventManager.addEventListener(EventType.SwitchCurtain, OnCurtainChange);
	}
	
    void OnDestroy()
    {
        EventManager.removeEventListener(EventType.SwitchCurtain, OnCurtainChange);
    }

    void OnCurtainChange(EventData data)
    {
        if((int)data.data==3)
        {
            InitConvContent3();
        }
        if((int)data.data==4)
        {
            InitConvContent4();
        }
    }

    // 第二幕对话内容
    void InitConvContent2()
    {
        convList.Clear();
        convList.Add(new ConversationContent(ActorID.Girl, "我：你好，有我的信吗？"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：你叫什么？"));
        convList.Add(new ConversationContent(ActorID.Girl, "我：媛媛"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：没有"));
    }
    // 第三幕对话内容
    void InitConvContent3()
    {
        convList.Clear();
        convList.Add(new ConversationContent(ActorID.Girl, "我：你好，今天有我的"+"\n"+"信吗？"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：......还是没有"));
        convList.Add(new ConversationContent(ActorID.Girl, "我：谢谢，那我下次再"+"\n"+"来看看。"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：你每天在这里等" + "\n" + "谁的信？"));
        convList.Add(new ConversationContent(ActorID.Girl, "我：我爸爸"));
        convList.Add(new ConversationContent(ActorID.PostMan, "......"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：我似乎见过他。"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：不如你给他写一" + "\n" + "封信给他？下次见到他" + "\n" + "我会转交。"));
    }

    void InitConvContent4()
    {
        convList.Clear();
        convList.Add(new ConversationContent(ActorID.Girl, "男孩：伯伯，爷爷是不" + "\n" + "是还没回来呀？"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：嘿，臭小子，真" + "\n" + "可爱！爷爷就快回来啦。"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：丫头......这个" + "\n" + "渡口邮点取消了，我明" + "\n" + "天往后就来不了了。"));
        convList.Add(new ConversationContent(ActorID.PostMan, "邮差：上次我好像看" + "\n" + "到你爸爸了......别" + "\n" + "担心，他快回来了。"));
    }
	// Update is called once per frame
	void Update () 
    {
        if (GlobalState.startTalk && !isConvPanelInit)
        {
            panel.SetActive(true);
            OnSwitchBtnClick();
            isConvPanelInit = true;
        }
	}
    void OnSwitchBtnClick()
    {
        
        if(!GlobalState.isTalked&&counter<convList.Count)
        {
            if(convList[counter].id==ActorID.Girl)
            {
                GirlConversationPanel.SetActive(true);
                postManConversationPanel.SetActive(false);
                girlMessageText.text = convList[counter].message;
                //girlHead.SetActive(true);
                //postManHead.SetActive(false);
            }
            else if (convList[counter].id == ActorID.PostMan)
            {
                //girlHead.SetActive(false);
               // postManHead.SetActive(true);
                GirlConversationPanel.SetActive(false);
                postManConversationPanel.SetActive(true);
                postmanMessageText.text = convList[counter].message;
            }
            //messageText.text = convList[counter].message;
            counter++;

        }
        else
        {
            if(SceneMgr.curtainIndex==3)
            {
                EventManager.dispatchEvent(EventType.ShowLetter, null);
            }
            else
            {
                GlobalState.startTalk = false;
                GlobalState.isTalked = true;
            }
            counter = 0;
            panel.SetActive(false);
            GirlConversationPanel.SetActive(false);
            postManConversationPanel.SetActive(false);
        }
    }
}
