using System.Collections;
using System.Collections.Generic;

public class GameStateObj
{
    public enum gameStates {
        Init,
        Load,
        Title,
        Settings,
        Play,
        Pause,
        Over,
        Exit
    };

    public gameStates state;
    public gameStates last;
}
