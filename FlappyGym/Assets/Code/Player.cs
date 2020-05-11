using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterMove _characterMove;


    void Awake()
    {
        ServiceLocator.Register<Player>(this);
        _characterMove = GetComponent<CharacterMove>();
    }

    public void Kill(float time)
    {
        StartCoroutine(OnDeath(time));
    }

    public void Force(float force, Vector3 point)
    {
        if (_characterMove != null)
        {
            _characterMove.Force(force, point);
        }
    }

    private IEnumerator OnDeath(float time)
    {
        ServiceLocator.Resolve<GameEvent>().NewEvent("Died");

        yield return new WaitForSeconds(time);

        ServiceLocator.Resolve<GameState>().SetStateOver();
    }
}
