﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour, ISubscribeState
{

    [SerializeField]
    private float _waitTime = 3f;

    void Awake()
    {
        ServiceLocator.Register<GameLoader>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }

    IEnumerator BeginGame()
    {
        Debug.Log("Loading game");
        yield return new WaitForSeconds(_waitTime);
        Debug.Log("Starting game");
        ServiceLocator.Resolve<GameState>().SetStateMain();
    }

    public void ReactState(GameStateObj state)
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
}
