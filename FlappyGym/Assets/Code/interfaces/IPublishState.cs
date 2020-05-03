using System.Collections.Generic;

public interface IPublishState
{
    List<ISubscribeState> StateSubscribers { get; }

    void NotifyState();

    void SubscribeState(ISubscribeState listener);
    void UnSubscribeState(ISubscribeState listener);
}
