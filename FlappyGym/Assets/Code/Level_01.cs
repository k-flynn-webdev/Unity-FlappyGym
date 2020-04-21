using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ObjectPoolItem _player;

    [SerializeField]
    private Vector3 _startPos;


    public override void Setup()
    {
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");

        base.Setup();
    }

    public override void Reset()
    {
        ResetPlayer();
        base.Reset();
    }


    public override void Title()
    {
        Reset();
    }

    private void ResetPlayer()
    {
        _player.transform.position = _startPos;
        _player.transform.localEulerAngles = Vector3.zero;
        _player.GetComponent<CharacterMove>().Reset();
    }

    public override void UnLoad()
    {
        if (_player == null)
        {
            return;
        }

        _player.IsNotActive();

        base.UnLoad();
    }
}
