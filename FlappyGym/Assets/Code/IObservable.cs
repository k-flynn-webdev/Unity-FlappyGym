using System.Collections.Generic;

public interface IObservable
{

    List<IObservable> Subscribers { get; }

    // tiggers all liisteners
    void Notify();

    // how to react on being notified
    void React();

    void Subscribe(IObservable listener);

    void UnSubscribe(IObservable listener);
}
