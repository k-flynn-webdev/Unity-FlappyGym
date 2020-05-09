using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_State : MonoBehaviour, ISubscribeState
{

    public GameStateObj State { get; set; }

    [SerializeField]
    public GameObject[] _uiGO;

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

    [SerializeField]
    private float _waitDelay = 0f;


    void Awake()
    {
        TurnOff();
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }

    private void CheckTurnOn()
    {
        if (_waitDelay > 0f)
        {
            StartCoroutine(TurnOnDelay(_waitDelay));
        } else
        {
            TurnOn();
        }
    }

    private IEnumerator TurnOnDelay(float time)
    {
        yield return new WaitForSeconds(time);
        TurnOn();
    }


    private void TurnOn()
    {
        for (int i = 0; i < _uiGO.Length; i++)
        {
            _uiGO[i].SetActive(true);
        }
    }

    private void TurnOff()
    {
        for (int i = 0; i < _uiGO.Length; i++)
        {
            _uiGO[i].SetActive(false);
        }
    }

    public void ReactState(GameStateObj state)
    {
        TurnOff();

        State = state;

        if (_loading && state.state == GameStateObj.gameStates.Load)
        {
            CheckTurnOn();
            return;
        }
        if (_main && state.state == GameStateObj.gameStates.Title)
        {
            CheckTurnOn();
            return;
        }
        if (_play && state.state == GameStateObj.gameStates.Play)
        {
            CheckTurnOn();
            return;
        }
        if (_pause && state.state == GameStateObj.gameStates.Pause)
        {
            CheckTurnOn();
            return;
        }
        if (_over && state.state == GameStateObj.gameStates.Over)
        {
            CheckTurnOn();
            return;
        }
    }
}
