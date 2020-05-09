using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateUI: MonoBehaviour
{
    public void SetStateLoad()
    {
        ServiceLocator.Resolve<GameState>().ChangeState(GameStateObj.gameStates.Load);
    }
    public void SetStateMain()
    {
        ServiceLocator.Resolve<GameState>().ChangeState(GameStateObj.gameStates.Title);
    }
    public void SetStatePlay()
    {
        ServiceLocator.Resolve<GameState>().ChangeState(GameStateObj.gameStates.Play);
    }
    public void SetStatePause()
    {
        ServiceLocator.Resolve<GameState>().ChangeState(GameStateObj.gameStates.Pause);
    }
    public void SetStateOver()
    {
        ServiceLocator.Resolve<GameState>().ChangeState(GameStateObj.gameStates.Over);
    }
}
