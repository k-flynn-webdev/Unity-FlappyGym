using System.Collections.Generic;

public interface IPublish
{
    List<ISubscribe> Subscribers { get; }

    void Notify();

    void Subscribe(ISubscribe listener);
    void UnSubscribe(ISubscribe listener);
}
