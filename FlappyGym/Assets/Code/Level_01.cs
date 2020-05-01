using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ObjectPoolItem _player;

    public float _floorSize = 10f;
    private List<ObjectPoolItem> _floors = new List<ObjectPoolItem>();

    [SerializeField]
    private float _pipeStart = 20f;
    [SerializeField]
    private float _pipeMin = 5f;
    [SerializeField]
    private float _pipeMax = 15f;
    private List<ObjectPoolItem> _pipes = new List<ObjectPoolItem>();


    [SerializeField]
    private Vector3 _startPos;
    [SerializeField]
    private float _levelSize = 40f;
    [SerializeField]
    private float _levelOffset = 20f;


    private float _xProgress = 0f;
    private float _xlevelMin = 0f;
    private float _xlevelMax = 0f;

    private float _lastUpdate = 0f;

    private void Update()
    {
        if (_isPlaying)
        {
            _xProgress = _player.transform.position.x + _levelOffset;
            
            if (_xProgress > _lastUpdate)
            {
                UpdateMinMaxLevel();
                UpdateFloors();
                UpdatePipes();
            }
        }
    }

    public override void Setup()
    {
        _xProgress = 0f;
        CreateFloors();
        CreatePipes();
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");

        base.Setup();
    }

    private void CreateFloors()
    {
        for (int i = 0; i < 25; i++)
        {
            _floors.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Floor"));
        }
    }

    private void CreatePipes()
    {
        for (int i = 0; i < 10; i++)
        {
            _pipes.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Pipe"));
        }
    }


    private void SetupFloors()
    {
        float _currentPos = _xlevelMin;

        for (int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].Item != null)
            {
                _floors[i].Item.Place(_currentPos);
                _currentPos += _floorSize;
            }
        }
    }

    private void SetupPipes()
    {
        float _currentPos = _pipeStart;

        for (int i = 0; i < _pipes.Count; i++)
        {
            if (_pipes[i].Item != null)
            {
                _pipes[i].Item.Place(_currentPos);
                _currentPos += Random.Range(_pipeMin, _pipeMax);
            }
        }
    }


    private void UpdateLastUpdate()
    {
        _lastUpdate = _xProgress + 1f;
    }

    private void UpdateMinMaxLevel()
    {
        _xlevelMin = _xProgress - _levelSize + _levelOffset;
        _xlevelMax = _xProgress + _levelSize + _levelOffset;
    }

    public override void Reset()
    {
        _xProgress = 0f;
        _lastUpdate = 1f;
        ResetPlayer();
        UpdateMinMaxLevel();
        SetupFloors();
        SetupPipes();
        ServiceLocator.Resolve<ScoreManager>().SetScore(0f);
        base.Reset();
    }

    private void ResetPlayer()
    {
        _player.transform.position = _startPos;
        _player.transform.localEulerAngles = Vector3.zero;
        _player.GetComponent<CharacterMove>().Reset();
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


    private void UpdateFloors()
    {
        float xEnd = 0f;
        int floorIdx = -1;

        for (int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].transform.position.x > xEnd)
            {
                xEnd = _floors[i].transform.position.x;
            }

            if (_floors[i].transform.position.x < _xlevelMin)
            {
                floorIdx = i;
            }
        }

        xEnd += _floorSize;

        if (floorIdx == -1)
        {
            return;
        }

        UpdateLastUpdate();
        if (_floors[floorIdx].Item != null)
        {
            _floors[floorIdx].Item.Place(xEnd);
        }
    }

    private void UpdatePipes()
    {
        float xEnd = 0f;
        int pipeIdx = -1;

        for (int i = 0; i < _pipes.Count; i++)
        {

            if (_pipes[i].transform.position.x > xEnd)
            {
                xEnd = _pipes[i].transform.position.x;
            }

            if (_pipes[i].transform.position.x < _xlevelMin)
            {
                pipeIdx = i;
            }
        }

        if (pipeIdx == -1)
        {
            return;
        }

        if (_pipes[pipeIdx].Item != null)
        {
            _pipes[pipeIdx].Item.Place(xEnd + Random.Range(_pipeMin, _pipeMax));
        }
    }

    public override void UnLoad()
    {
        if (_player == null)
        {
            return;
        }

        ServiceLocator.Resolve<CameraControl>().SetTarget();
        _player.IsNotActive();

        for (int i = 0; i < _floors.Count; i++)
        {
            _floors[i].IsNotActive();
        }

        for (int i = 0; i < _pipes.Count; i++)
        {
            _pipes[i].IsNotActive();
        }

        base.UnLoad();
    }
}
