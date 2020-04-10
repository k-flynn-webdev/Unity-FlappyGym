using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateObj
{

    public enum gameStates { Load, Main, Play, Pause, Over };

    [SerializeField]
    public gameStates state;
    [SerializeField]
    public gameStates last;
}
