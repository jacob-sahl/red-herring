using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
}

// A simple Event System that can be used for remote systems communication
public static class EventManager
{
    static readonly Dictionary<Type, Action<GameEvent>> Events = new Dictionary<Type, Action<GameEvent>>();

    static readonly Dictionary<Delegate, Action<GameEvent>> EventLookups =
        new Dictionary<Delegate, Action<GameEvent>>();

    public static void AddListener<T>(Action<T> evt) where T : GameEvent
    {
        if (EventLookups.ContainsKey(evt)) return;
        Action<GameEvent> newAction = (e) => evt((T) e);
        EventLookups[evt] = newAction;

        if (Events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
            Events[typeof(T)] = internalAction += newAction;
        else
            Events[typeof(T)] = newAction;
    }

    public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
    {
        if (!EventLookups.TryGetValue(evt, out var action)) return;
        if (Events.TryGetValue(typeof(T), out var tempAction))
        {
            tempAction -= action;
            if (tempAction == null)
                Events.Remove(typeof(T));
            else
                Events[typeof(T)] = tempAction;
        }

        EventLookups.Remove(evt);
    }

    public static void Broadcast(GameEvent evt)
    {
        if (Events.TryGetValue(evt.GetType(), out var action))
            action.Invoke(evt);
    }

    public static void Clear()
    {
        Events.Clear();
        EventLookups.Clear();
    }
}