using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour, IPublishEvent
{

    [SerializeField]
    private string _current;
    [SerializeField]
    private string _last;

    private float _time;

    [SerializeField]
    private string[] _events = new string[10];


    void Awake()
    {
        ServiceLocator.Register<GameEvent>(this);
    }

    public void NewEvent(string eventType)
    {
        float timeNow = Time.time;
        float timeDiff = timeNow - _time;

        if (_current == eventType && timeDiff < .05f)
        {
            return;
        }

        SetEvent(eventType);
    }

    private void SetEvent(string eventType)
    {
        _last = _current;
        _current = eventType;
        _time = Time.time;

        #if UNITY_EDITOR
            UpdateHistory(eventType);
        #endif

        this.NotifyEvent();
    }

    private void UpdateHistory(string newEvent)
    {
        for (int i = _events.Length - 2; i > 0; i--)
        {
            _events[i] = _events[i - 1];
        }
        _events[0] = newEvent;
    }

    public List<ISubscribeEvent> EventSubscribers
    { get { return this.eventsubscribers; } }

    private List<ISubscribeEvent> eventsubscribers = new List<ISubscribeEvent>();


    public void NotifyEvent()
    {
        for (int i = eventsubscribers.Count - 1; i >= 0; i--)
        {
            eventsubscribers[i].ReactEvent(_current);
        }
    }

    public void SubscribeEvent(ISubscribeEvent listener)
    { eventsubscribers.Add(listener); }

    public void UnSubscribeEvent(ISubscribeEvent listener)
    { eventsubscribers.Remove(listener); }

}
