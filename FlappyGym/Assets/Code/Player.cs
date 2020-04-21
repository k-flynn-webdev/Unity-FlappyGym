using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISubscribe
{

    private float _deathWait = 0.4f;

    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    void OnTriggerEnter(Collider hitBy) { this.TriggerCol(hitBy); }

    void OnTriggerStay(Collider hitBy) { this.TriggerCol(hitBy); }

    void OnTriggerExit(Collider hitBy) { this.TriggerCol(hitBy); }


    void TriggerCol(Collider hitBy)
    {

        if (hitBy.CompareTag("Bounce"))
        {

        }

        if (hitBy.tag == "Death")
        {
            StartCoroutine(OnDeath());
        }
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
