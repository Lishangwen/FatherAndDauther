using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �¼�����
/// (������Ҫȡ���ƣ������ظ�)
/// </summary>
public enum EventType
{
    None,
    StartGame,
    ClickBlock,
    StartHugGirl,
    FatherOclick,
    DayNightChange,
    PostManOnClick,
    SwitchCurtain,
    ShowLetter,
    DogSpark,
    GrandmaLieDown,
    FatherLastClick,
    PlayBGM,
    ShowTip,
    PlayEffect,
    PlayLoopEffect,
    EnterHome,
    LeaveHome,
    TitleComplete,
    SwitchBgm,
    ClickMailBox,
    SceneChanged
}


/// <summary>
/// �¼�������
/// </summary>
public class EventManager {

    /// <summary>
    /// �¼�������
    /// </summary>
    private static Dictionary<EventType, DelegateEvent> eventTypeListeners = new Dictionary<EventType, DelegateEvent>();

    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="type">�¼�����</param>
    /// <param name="listenerFunc">��������</param>
    public static void addEventListener(EventType type,DelegateEvent.EventHandler listenerFunc)
    {
        DelegateEvent delegateEvent;
        if(eventTypeListeners.ContainsKey(type))
        {
            delegateEvent = eventTypeListeners[type];
        }else
        {
            delegateEvent = new DelegateEvent();
            eventTypeListeners[type] = delegateEvent;
        }
        delegateEvent.addListener(listenerFunc);
    }

    /// <summary>
    /// ɾ���¼�
    /// </summary>
    /// <param name="type">�¼�����</param>
    /// <param name="listenerFunc">��������</param>
    public static void removeEventListener(EventType type,DelegateEvent.EventHandler listenerFunc)
    {
        if (listenerFunc == null)
        {
            return;
        }
        if(!eventTypeListeners.ContainsKey(type))
        {
            return;
        }
        DelegateEvent delegateEvent = eventTypeListeners[type];
        delegateEvent.removeListener(listenerFunc);
    }
    
    /// <summary>
    /// ����ĳһ���͵��¼�  ����������
    /// </summary>
    /// <param name="type">�¼�����</param>
    /// <param name="data">�¼�������(��Ϊnull)</param>
    public static void dispatchEvent(EventType type,object data)
    {
        if(!eventTypeListeners.ContainsKey(type))
        {
            return;
        }
        //�����¼�����
        EventData eventData = new EventData();
        eventData.type = type;
        eventData.data = data;

        DelegateEvent delegateEvent = eventTypeListeners[type];
        delegateEvent.Handle(eventData);
    }

}

/// <summary>
/// �¼���
/// </summary>
public class DelegateEvent
{
    /// <summary>
    /// ����ί�к���
    /// </summary>
    /// <param name="data"></param>
    public delegate void EventHandler(EventData data);
    /// <summary>
    /// �������ί�к������¼�
    /// </summary>
    public event EventHandler eventHandle;

    /// <summary>
    /// ���������¼�
    /// </summary>
    /// <param name="data"></param>
    public void Handle(EventData data)
    {
        if(eventHandle!=null)
             eventHandle(data);
    }

    /// <summary>
    /// ɾ����������
    /// </summary>
    /// <param name="removeHandle"></param>
    public void removeListener(EventHandler removeHandle)
    {
        if (eventHandle != null)
            eventHandle -= removeHandle;
    }

    /// <summary>
    /// ��Ӽ�������
    /// </summary>
    /// <param name="addHandle"></param>
    public void addListener(EventHandler addHandle)
    {
        eventHandle += addHandle;
    }
}

/// <summary>
/// �¼�����
/// </summary>
public class EventData
{
    /// <summary>
    /// �¼�����
    /// </summary>
    public EventType type;
    /// <summary>
    /// �¼����ݵ�����
    /// </summary>
    public object data;
}
