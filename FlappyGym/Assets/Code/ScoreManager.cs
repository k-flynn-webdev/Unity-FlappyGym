﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    public float Score
    { get { return this._score; } }

    private float _score = 0f;

    [SerializeField]
    private TextMeshProUGUI[] _ScoreText;
    [SerializeField]
    Animator _scoreAnim;

    public void SetScore(float newScore)
    {
        _score = newScore;

        _scoreAnim.SetTrigger("score");

        for (int i = 0; i < _ScoreText.Length; i++)
        {
            _ScoreText[i].text = _score.ToString();
        }

        ServiceLocator.Resolve<GameEvent>().SetEvent("score");
    }

    public void AddScore(float addScore)
    {
        SetScore(Score + addScore);
    }

    void Awake()
    {
        ServiceLocator.Register<ScoreManager>(this);
    }
}
