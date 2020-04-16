using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour, IObservable
{

    // todo this is my object pooler / loader

    [SerializeField]
    private GameObject[] _items;


    void Awake()
    {
        ServiceLocator.Register<GameLoader>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    IEnumerator BeginGame()
    {
        Debug.Log("Loading game");
        yield return new WaitForSeconds(6.0f);
        Debug.Log("Starting game");
        ServiceLocator.Resolve<GameState>().SetState(GameStateObj.gameStates.Main);
    }

    public List<IObservable> Subscribers
    { get; }

    public void Notify() { }

    public void React(GameStateObj state)
    {
        switch (state.state)
        {
            case GameStateObj.gameStates.Load:
                StartCoroutine(BeginGame());
                break;
            case GameStateObj.gameStates.Main:
                break;
            case GameStateObj.gameStates.Play:
                break;
            case GameStateObj.gameStates.Pause:
                break;
            case GameStateObj.gameStates.Over:
                break;
        }
    }

    public void Subscribe(IObservable listener) { }
    public void UnSubscribe(IObservable listener) { }
}
