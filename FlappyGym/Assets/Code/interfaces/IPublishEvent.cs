using System.Collections.Generic;

public interface IPublishEvent
{
    List<ISubscribeEvent> EventSubscribers { get; }

    void NotifyEvent();

    void SubscribeEvent(ISubscribeEvent listener);
    void UnSubscribeEvent(ISubscribeEvent listener);
}
