using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ObjectPoolItem _player;

    public override void Setup()
    {
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");

        base.Setup();
    }

    public override void UnLoad()
    {
        if (_player == null)
        {
            return;
        }

        _player.IsNotActive(); 
    }
}
