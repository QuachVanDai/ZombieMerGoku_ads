using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EventListener : MonoBehaviour
{
    public string eventName;
    public UnityEvent handler;

    EventName eventType;

    void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(EventName));
        int index = Array.IndexOf(names, eventName);
        if (index < 0)
        {
            eventType = EventName.NONE;
        }
        else
        {
            eventType = (EventName)Enum.GetValues(typeof(EventName)).GetValue(index);
        }

        if (eventType != EventName.NONE)
        {
            EventDispatcher.Instance.AddListener(eventType, OnEvent);
        }
    }

    void OnDisable()
    {
        if (eventType != EventName.NONE)
        {
            EventDispatcher.Instance.RemoveListener(eventType, OnEvent);
        }
    }

    void OnEvent(EventName key, object data)
    {
        handler.Invoke();
    }
}
