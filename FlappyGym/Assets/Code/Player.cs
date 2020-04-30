using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISubscribe
{

    private float _deathWait = 0.4f;

    private CharacterMove _charcterMove;


    void Awake()
    {
        ServiceLocator.Register<Player>(this);
        _charcterMove = GetComponent<CharacterMove>();
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void Kill(float time)
    {
        StartCoroutine(OnDeath(time));
    }

    public void Force(float force, Vector3 point)
    {
        if (_charcterMove != null)
        {
            _charcterMove.Force(force, point);
        }
    }

    private IEnumerator OnDeath(float time)
    {
        yield return new WaitForSeconds(time);

        ServiceLocator.Resolve<GameState>().SetStateOver();
    }

    public void React(GameStateObj state)
    {
        //_gameInPlay = state.state == GameStateObj.gameStates.Play;
    }
}
