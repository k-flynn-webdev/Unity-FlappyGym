using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePlay : MonoBehaviour, IObservable
{

    public GameObject _playButton;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void SetPlay()
    {
        ServiceLocator.Resolve<GameState>().SetState(GameStateObj.gameStates.Play);
    }

    public List<IObservable> Subscribers
    { get; }

    public void Notify() { }

    public void React(GameStateObj state)
    {
        _playButton.SetActive(state.state != GameStateObj.gameStates.Play);
    }

    public void Subscribe(IObservable listener) { }
    public void UnSubscribe(IObservable listener) { }
}
