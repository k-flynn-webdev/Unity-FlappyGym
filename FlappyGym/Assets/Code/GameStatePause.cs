using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePause : MonoBehaviour, IObservable
{

    public GameObject _pauseButton;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void SetPause()
    {
        ServiceLocator.Resolve<GameState>().SetState(GameStateObj.gameStates.Pause);
    }

    public List<IObservable> Subscribers
    { get; }

    public void Notify() { }

    public void React(GameStateObj state)
    {
        _pauseButton.SetActive(state.state == GameStateObj.gameStates.Play);
    }

    public void Subscribe(IObservable listener) { }
    public void UnSubscribe(IObservable listener) { }
}
