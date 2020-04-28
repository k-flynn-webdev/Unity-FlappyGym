using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour
{

    [SerializeField]
    public bool _active = true;

    void OnTriggerEnter(Collider hitBy) { this.TriggerCol(hitBy); }

    void OnTriggerStay(Collider hitBy) { this.TriggerCol(hitBy); }

    void OnTriggerExit(Collider hitBy) { this.TriggerCol(hitBy); }


    void TriggerCol(Collider hitBy)
    {
        if (_active && hitBy.CompareTag("Player"))
        {
            ServiceLocator.Resolve<Player>().Kill();
        }
    }
}
