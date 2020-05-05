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
    public GameObject _hideOnContact;

    [SerializeField]
    public float _score = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (_active &&
            !_hasFired &&
            other.CompareTag("Player"))
        {
            ServiceLocator.Resolve<ScoreManager>().AddScore(_score);
            _hasFired = true;

            if (_hideOnContact != null)
            {
                _hideOnContact.SetActive(false);
            }
        }
    }

    public void Reset()
    {
        _hasFired = false;
        _hideOnContact.SetActive(true);
    }
}
