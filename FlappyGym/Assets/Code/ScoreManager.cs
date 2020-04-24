﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour, ISubscribe
{

    [SerializeField]
    public float Score
    { get { return this._score; } }

    [SerializeField]
    public bool DisplayLarge
    { get { return this._displayLarge; } }

    private float _score = 0f;
    private bool _displayLarge = false;

    [SerializeField]
    private GameObject _ScoreUI;
    [SerializeField]
    private Text _ScoreText;

    public void SetScore(float newScore)
    {
        _score = newScore;
        _ScoreText.text = _score.ToString();
    }

    void Awake()
    {
        ServiceLocator.Register<ScoreManager>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void React(GameStateObj state)
    {
        _displayLarge = state.state == GameStateObj.gameStates.Over;

        _ScoreUI.transform.localScale = Vector3.one;

        if (_displayLarge)
        {
            _ScoreUI.transform.localScale = Vector3.one * 2f;
        }

    }
}
