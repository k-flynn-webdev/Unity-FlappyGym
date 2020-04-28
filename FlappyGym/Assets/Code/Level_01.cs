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

    public float _floorSize = 10f;
    private List<ObjectPoolItem> _floors = new List<ObjectPoolItem>();
    private List<Item> _floorItems = new List<Item>();


    [SerializeField]
    private Vector3 _startPos;
    [SerializeField]
    private float _levelSize = 40f;
    [SerializeField]
    private float _levelOffset = 20f;


    private float _xProgress = 0f;
    private float _xlevelMin = 0f;
    private float _xlevelMax = 0f;

    private void Update()
    {
        if (_isPlaying)
        {
            _xProgress = _player.transform.position.x + _levelOffset;
            
            if (_xProgress + _levelSize > _xlevelMax)
            {
                UpdateMinMaxLevel();
                UpdateFloors();
                //UpdateWalls();
            }
        }
    }

    public override void Setup()
    {
        _xProgress = 0f;
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");

        CreateFloors();



        //for (int i = 0; i < 8; i++)
        //{
        //    _walls.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Wall_t_01"));
        //    _walls.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Wall_b_01"));
        //}

        //for (int i = 0; i < _walls.Count; i++)
        //{
        //    _wallItems.Add(_walls[i].GetComponent<Item>());
        //}



        base.Setup();
    }

    public override void Reset()
    {
        _xProgress = 0f;
        UpdateMinMaxLevel();
        SetupFloors();
        SetupWalls();
        ResetPlayer();
        ServiceLocator.Resolve<ScoreManager>().SetScore(0f);
        base.Reset();
    }


    public override void Title()
    {
        Reset();
        ServiceLocator.Resolve<CameraControl>().SetTarget(_player.transform);
    }

    public override void Play()
    {
        base.Play();
    }

    private void ResetPlayer()
    {
        _player.transform.position = _startPos;
        _player.transform.localEulerAngles = Vector3.zero;
        _player.GetComponent<CharacterMove>().Reset();
    }


    private void UpdateMinMaxLevel()
    {
        _xlevelMin = _xProgress - _levelSize;
        _xlevelMax = _xProgress + _levelSize;
    }


    private void CreateFloors()
    {
        for (int i = 0; i < 10; i++)
        {
            _floors.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Floor"));
        }

        for (int i = 0; i < _floors.Count; i++)
        {
            _floorItems.Add(_floors[i].GetComponent<Item>());
        }
    }


    private void SetupFloors()
    {
        float _currentPos = _xlevelMin;

        for (int i = 0; i < _floorItems.Count; i++)
        {
            _floorItems[i].Place(_currentPos);
            _currentPos += _floorSize;
        }
    }

    private void UpdateFloors()
    {
        float xEnd = 0f;
        int floorIdx = -1;

        for (int i = 0; i < _floorItems.Count; i++)
        {
            if (_floorItems[i].transform.position.x > xEnd)
            {
                xEnd = _floorItems[i].transform.position.x;
            }

            if (_floorItems[i].transform.position.x < _xlevelMin)
            {
                floorIdx = i;
            }
        }

        xEnd += _floorSize;

        if (floorIdx == -1)
        {
            return;
        }

        _floorItems[floorIdx].Place(xEnd);
    }



    private void SetupWalls()
    {
        //_xlevelMin = _xProgress - _levelSize;
        //_xlevelMax = _xProgress + _levelSize;

        //for (int i = 0; i < _wallItems.Count; i++)
        //{
        //    _wallItems[i].Place(Mathf.Lerp(_xlevelMin, _xlevelMax, (1f/ _wallItems.Count) * i ));
        //}
    }

    private void UpdateWalls()
    {
        //_xlevelMin = _xProgress - _levelSize;
        //_xlevelMax = _xProgress + _levelSize;

        //for (int i = 0; i < _wallItems.Count; i++)
        //{
        //    if (_wallItems[i].transform.position.x < _xlevelMin)
        //    {
        //        _wallItems[i].Place(_xlevelMax);
        //        return;
        //    }
        //}
    }

    public override void UnLoad()
    {
        if (_player == null)
        {
            return;
        }

        ServiceLocator.Resolve<CameraControl>().SetTarget();
        _player.IsNotActive();

        base.UnLoad();
    }
}
