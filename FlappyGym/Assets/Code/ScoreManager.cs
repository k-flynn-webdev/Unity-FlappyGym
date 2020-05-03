using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour, ISubscribeState
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

    private GameStateObj.gameStates _state;

    public void SetScore(float newScore)
    {
        _score = newScore;
        _ScoreText.text = _score.ToString();
    }

    public void AddScore(float addScore)
    {
        if (_state != GameStateObj.gameStates.Play)
        {
            return;
        }

        SetScore(Score + addScore);
    }

    void Awake()
    {
        ServiceLocator.Register<ScoreManager>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }

    public void ReactState(GameStateObj state)
    {
        _state = state.state;
        _displayLarge = state.state == GameStateObj.gameStates.Over;

        _ScoreUI.transform.localScale = Vector3.one;

        if (_displayLarge)
        {
            _ScoreUI.transform.localScale = Vector3.one * 2f;
        }

    }
}
