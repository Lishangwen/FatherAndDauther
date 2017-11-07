using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger2D : MonoBehaviour
{
    public string compTag;
    public EventType enterType;
    public EventType exitType;
    public UnityEvent enter;
    public UnityEvent exit;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == compTag)
        {
            if (enterType != EventType.None)
            {
                EventManager.dispatchEvent(enterType, null);
            }
            enter.Invoke();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == compTag)
        {
            if (exitType != EventType.None)
            {
                EventManager.dispatchEvent(exitType, null);
            }
            exit.Invoke();
        }
    }


}
