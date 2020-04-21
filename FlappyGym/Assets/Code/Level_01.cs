using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ObjectPoolItem _player;

    [SerializeField]
    private List<ObjectPoolItem> _walls = new List<ObjectPoolItem>();
    [SerializeField]
    private List<Item> _wallItems = new List<Item>();


    [SerializeField]
    private Vector3 _startPos;


    public override void Setup()
    {
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");

        for (int i = 0; i < 8; i++)
        {
            _walls.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Wall_t_01"));
            _walls.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Wall_b_01"));
        }

        for (int i = 0; i < _walls.Count; i++)
        {
            _wallItems.Add(_walls[i].GetComponent<Item>());
        }

        base.Setup();
    }

    public override void Reset()
    {
        ResetPlayer();
        SetupWalls(10f, 70f);
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


    private void SetupWalls(float xMin, float xMax)
    {
        for (int i = 0; i < _wallItems.Count; i++)
        {
            _wallItems[i].Place(Mathf.Lerp(xMin,xMax, (1f/ _wallItems.Count) * i ));
        }
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
