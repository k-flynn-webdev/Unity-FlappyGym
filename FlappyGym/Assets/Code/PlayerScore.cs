using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour, IReset
{
    [SerializeField]
    public bool _active = true;
    [SerializeField]
    public bool _hasFired = false;


    [SerializeField]
    public float _score = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (_active && other.CompareTag("Player"))
        {
            ServiceLocator.Resolve<ScoreManager>().AddScore(_score);
            _hasFired = true;
        }
    }

    public void Reset()
    {
        _hasFired = false;
    }
}
