//Author: Tim Boettcher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData
{
    public EventTypes Type { get; private set; }
    public object Data { get; private set; }

    public EventData(EventTypes type)
        : this(type, null)
    {
        Type = type;
    }

    public EventData(EventTypes type, object data)
    {
        Type = type;
        Data = data;
    }
}
