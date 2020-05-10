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
        ServiceLocator.Resolve<ScoreManager>().SetScore(3f);
        yield return new WaitForSeconds(0.9f);
        StartCoroutine(CountDown_2());
    }

    IEnumerator CountDown_2()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(2f);
        yield return new WaitForSeconds(0.9f);
        StartCoroutine(CountDown_1());
    }

    IEnumerator CountDown_1()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(1f);
        yield return new WaitForSeconds(0.9f);
        StartCoroutine(CountDown_Go());
    }

    IEnumerator CountDown_Go()
    {
        ServiceLocator.Resolve<ScoreManager>().SetScore(0f);
        _UI_Text_GO.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        _UI_Text_GO.SetActive(false);
    }
}
