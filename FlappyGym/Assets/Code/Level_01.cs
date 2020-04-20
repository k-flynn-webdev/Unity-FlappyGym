using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ObjectPoolItem _player;

    void Start()
    {
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");
    }

    private void OnDestroy()
    {
        if (_player == null)
        {
            return;
        }

        _player.IsNotActive(); 
    }
}
