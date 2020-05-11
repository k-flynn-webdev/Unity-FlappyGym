using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CountDown : MonoBehaviour, ISubscribeState
{
    public GameObject _UI_Text_GO;

    public GameStateObj State { get; set; }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }

    private float _waitTime_Secs = 0.55f;

    public void ReactState(GameStateObj state)
    {
        if (state.state == GameStateObj.gameStates.Play &&
            state.last == GameStateObj.gameStates.Over)
        {
            ResetCountDown();
        }
        if (state.state == GameStateObj.gameStates.Play &&
            state.last == GameStateObj.gameStates.Title)
        {
            ResetCountDown();
        }
    }

    private void ResetCountDown()
    {
        StartCoroutine(CountDown_3());
    }

    IEnumerator CountDown_3()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(3f, true);
        yield return new WaitForSeconds(_waitTime_Secs);
        StartCoroutine(CountDown_2());
    }

    IEnumerator CountDown_2()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(2f, true);
        yield return new WaitForSeconds(_waitTime_Secs);
        StartCoroutine(CountDown_1());
    }

    IEnumerator CountDown_1()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(1f, true);
        yield return new WaitForSeconds(_waitTime_Secs);
        StartCoroutine(CountDown_Go());
    }

    IEnumerator CountDown_Go()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(0f, false);
        ServiceLocator.Resolve<GameEvent>().NewEvent("AllowPlayerInput");
        _UI_Text_GO.SetActive(true);
        yield return new WaitForSeconds(2f);
        _UI_Text_GO.SetActive(false);
    }
}
