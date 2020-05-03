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



    void Awake()
    {
        ServiceLocator.Register<GameEvent>(this);
    }

    private void Start()
    {
    }


    public void ChangeEvent(string eventType)
    {
        float timeNow = Time.time;
        float timeDiff = timeNow - _time;

        if (_current == eventType && timeDiff < .1f)
        {
            return;
        }

        SetEvent(eventType);
    }

    public void SetEvent(string eventType)
    {
        _last = _current;
        _current = eventType;
        _time = Time.time;

        this.NotifyEvent();
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
