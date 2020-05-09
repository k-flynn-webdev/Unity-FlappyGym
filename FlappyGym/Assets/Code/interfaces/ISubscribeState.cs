using System.Collections.Generic;

public interface ISubscribeState
{
    GameStateObj State { get; set; }

    void ReactState(GameStateObj state);
}
