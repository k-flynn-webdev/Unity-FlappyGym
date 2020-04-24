using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour, ISubscribe
{

    [SerializeField]
    public float Score
    { get { return this._score; } }

    [SerializeField]
    public bool Display
    { get { return this._display; } }

    private float _score = 0f;
    private bool _display = false;

    [SerializeField]
    private GameObject _ScoreUI;
    [SerializeField]
    private Text _ScoreText;

    public void SetScore(float newScore)
    {
        if (!_display)
        {
            _display = true;
        }

        _score = newScore;
        _ScoreText.text = _score.ToString();
    }

    public void HideScore()
    {
        _score = 0f;
        _display = false;
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
        _gameInPlay = state.state == GameStateObj.gameStates.Play;
    }

}
