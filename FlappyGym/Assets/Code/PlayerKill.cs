using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour, IReset
{

    [SerializeField]
    public bool _active = true;

    private bool _isConsumed = false;

    [SerializeField]
    private float _force = 0f;
    [SerializeField]
    private float _time = 3f;

    void OnTriggerEnter(Collider hitBy) { this.TriggerCol(hitBy); }

    void OnTriggerStay(Collider hitBy) { this.TriggerCol(hitBy); }

    void OnTriggerExit(Collider hitBy) { this.TriggerCol(hitBy); }


    void TriggerCol(Collider hitBy)
    {
        if (_isConsumed)
        {
            return;
        }

        if (_active && hitBy.CompareTag("Player"))
        {
            ServiceLocator.Resolve<Player>().Kill(_time);
            _isConsumed = true;

            if (_force > 0f)
            {
                ServiceLocator.Resolve<Player>().Force(_force, this.transform.position);
            }
        }
    }

    public void Reset()
    {
        _isConsumed = false;
    }
}
