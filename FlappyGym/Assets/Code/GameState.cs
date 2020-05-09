using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, IPublishState
{

    [SerializeField]
    private GameStateObj _gameState = new GameStateObj();

    [SerializeField]
    private GameStateObj.gameStates _state;
    [SerializeField]
    private GameStateObj.gameStates _last;


    void Awake()
    {
        ServiceLocator.Register<GameState>(this);
    }


    public void ChangeState(GameStateObj.gameStates state)
    {
        if (this._gameState.state == state)
        {
            return;
        }

        switch (state)
        {
            case GameStateObj.gameStates.Load:
                this.SetState(GameStateObj.gameStates.Load);
                break;
            case GameStateObj.gameStates.Title:
                this.SetState(GameStateObj.gameStates.Title);
                break;
            case GameStateObj.gameStates.Settings:
                this.SetState(GameStateObj.gameStates.Settings);
                break;
            case GameStateObj.gameStates.Play:
                this.SetState(GameStateObj.gameStates.Play);
                break;
            case GameStateObj.gameStates.Pause:
                this.SetState(GameStateObj.gameStates.Pause);
                break; 
            case GameStateObj.gameStates.Over:
                this.SetState(GameStateObj.gameStates.Over);
                break;
        }
    }

    public void SetState(GameStateObj.gameStates state)
    {
        this._gameState.last = this._gameState.state;
        this._gameState.state = state;

        this._state = state;
        this._last = this._gameState.last;

        this.NotifyState();
    }

    public void SetStateLoad() => ChangeState(GameStateObj.gameStates.Load);
    public void SetStateTitle() => ChangeState(GameStateObj.gameStates.Title);
    public void SetStateSettings() => ChangeState(GameStateObj.gameStates.Settings);
    public void SetStatePlay() => ChangeState(GameStateObj.gameStates.Play);
    public void SetStatePause() => ChangeState(GameStateObj.gameStates.Pause);
    public void SetStateOver() => ChangeState(GameStateObj.gameStates.Over);

    public List<ISubscribeState> StateSubscribers
    { get { return this.statesubscribers; } }
    
    private List<ISubscribeState> statesubscribers = new List<ISubscribeState>();

    public void NotifyState()
    {
        for (int i = statesubscribers.Count - 1; i >= 0; i--)
        {
            statesubscribers[i].ReactState(this._gameState);
        }
    }

    public void SubscribeState(ISubscribeState listener)
    { statesubscribers.Add(listener); }

    public void UnSubscribeState(ISubscribeState listener)
    { statesubscribers.Remove(listener); }
}
