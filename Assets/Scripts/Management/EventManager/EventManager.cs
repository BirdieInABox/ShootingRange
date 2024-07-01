//Author: Tim Boettcher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager MainStatic;
    List<IEventListener> listeners;
    List<EventData> events;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (MainStatic == null)
            MainStatic = this;
        else
            Destroy(this);
        listeners = new List<IEventListener>();
        events = new List<EventData>();
    }

    public void Update()
    {
        // Make sure no new events are added to the list that is now executed.
        List<EventData> eventsToFireNow = events;
        events = new List<EventData>();

        foreach (EventData eventToFire in eventsToFireNow)
        {
            foreach (IEventListener listener in listeners)
            {
                listener.OnEventReceived(eventToFire);
            }
        }
    }

    public void AddListener(IEventListener listener)
    {
        if (listener != null && !listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void FireEvent(EventData eventToFire)
    {
        events.Add(eventToFire);
    }
}
