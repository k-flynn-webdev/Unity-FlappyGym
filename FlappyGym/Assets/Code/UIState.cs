using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : MonoBehaviour, ISubscribeState
{

    [SerializeField]
    public GameObject _uiGO;

    [SerializeField]
    public bool _loading = false;
    [SerializeField]
    public bool _main = false;
    [SerializeField]
    public bool _play = false;
    [SerializeField]
    public bool _pause = false;
    [SerializeField]
    public bool _over = false;

    void Awake()
    {
        _uiGO.SetActive(false);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }

    public void ReactState(GameStateObj state)
    {
        _uiGO.SetActive(false);

        if (_loading && state.state == GameStateObj.gameStates.Load)
        {
            _uiGO.SetActive(true);
            return;
        }
        if (_main && state.state == GameStateObj.gameStates.Main)
        {
            _uiGO.SetActive(true);
            return;
        }
        if (_play && state.state == GameStateObj.gameStates.Play)
        {
            _uiGO.SetActive(true);
            return;
        }
        if (_pause && state.state == GameStateObj.gameStates.Pause)
        {
            _uiGO.SetActive(true);
            return;
        }
        if (_over && state.state == GameStateObj.gameStates.Over)
        {
            _uiGO.SetActive(true);
            return;
        }
    }
}
