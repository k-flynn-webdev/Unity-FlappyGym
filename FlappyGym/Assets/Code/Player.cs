using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISubscribe
{

    private float _deathWait = 0.4f;

    void Awake()
    {
        ServiceLocator.Register<Player>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void Kill()
    {
        StartCoroutine(OnDeath());
    }

    private IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(_deathWait);

        ServiceLocator.Resolve<GameState>().SetStateOver();
    }

    public void React(GameStateObj state)
    {
        //_gameInPlay = state.state == GameStateObj.gameStates.Play;
    }
}
