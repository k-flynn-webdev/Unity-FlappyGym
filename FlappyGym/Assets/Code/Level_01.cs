using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ObjectPoolItem _player;

    public float _houseSize = 10f;
    private List<ObjectPoolItem> _houses = new List<ObjectPoolItem>();


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
                UpdateHouses();
            }
        }
    }

    public override void Setup()
    {
        _xProgress = 0f;
        CreateHouses();
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player");

        base.Setup();
    }

    private void CreateHouses()
    {
        for (int i = 0; i < 10; i++)
        {
            string item = "";
            float rand = Random.value;
            if (rand < 0.33)
            {
                item = "House_01";
            }
            if (rand >= 0.33 && rand < 0.66)
            {
                item = "House_02";
            }
            if (rand >= 0.66)
            {
                item = "House_03";
            }

            _houses.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem(item));
        }
    }

    private void SetupHouses()
    {
        float _currentPos = _xlevelMin;

        for (int i = 0; i < _houses.Count; i++)
        {
            if (_houses[i].Item != null)
            {
                _houses[i].Item.Place(_currentPos);
                _currentPos += _houseSize;
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
        SetupHouses();
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


    private void UpdateHouses()
    {
        float xEnd = 0f;
        int houseIdx = -1;

        for (int i = 0; i < _houses.Count; i++)
        {
            if (_houses[i].transform.position.x > xEnd)
            {
                xEnd = _houses[i].transform.position.x;
            }

            if (_houses[i].transform.position.x < _xlevelMin)
            {
                houseIdx = i;
            }
        }

        xEnd += _houseSize;

        if (houseIdx == -1)
        {
            return;
        }

        UpdateLastUpdate();
        if (_houses[houseIdx].Item != null)
        {
            _houses[houseIdx].Item.Place(xEnd);
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

        for (int i = 0; i < _houses.Count; i++)
        {
            _houses[i].IsNotActive();
        }

        base.UnLoad();
    }
}
