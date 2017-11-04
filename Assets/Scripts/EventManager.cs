using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
    private Dictionary<string, UnityEvent> events;
    public static EventManager eventManager;
    public EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = GameObject.FindObjectOfType<EventManager>() as EventManager;
                if (!eventManager)
                {
                    Debug.Log("There needs to be atleast one active EventManager");
                }
                else
                {
                    eventManager.Init();
                }

            }
            return eventManager;
        }
    }

    public void Init()
    {
        if(events==null)
        {
            events = new Dictionary<string, UnityEvent>();
        }
    }
    public static void StartListening(string name,UnityAction action)
    {
        UnityEvent tempEvent = null;
        if(eventManager.events.TryGetValue(name,out tempEvent))
        {
            tempEvent.AddListener(action);
        }
        else
        {
            tempEvent = new UnityEvent();
            tempEvent.AddListener(action);
            eventManager.events.Add(name, tempEvent);
        }
    }
    public static void StopListening(string name,UnityAction action)
    {
        if (!eventManager)
            return;
        UnityEvent tempEvent = null;
        if (eventManager.events.TryGetValue(name, out tempEvent))
        {
            tempEvent.RemoveListener(action);
        }

    }
    public static void TriggerEvent(string name)
    {
        UnityEvent tempEvent = null;
        if (eventManager.events.TryGetValue(name, out tempEvent))
        {
            tempEvent.Invoke();
        }
    }
}
