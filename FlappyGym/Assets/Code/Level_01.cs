using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ItemConfig _itemConfig;

    [SerializeField]
    private Texture2D _levelImage;

    [SerializeField]
    private float _tileSize = 10f;

    private List<ObjectPoolItem> _items = new List<ObjectPoolItem>();

    //private ObjectPoolItem _player;

    //private List<ObjectPoolItem> _floors = new List<ObjectPoolItem>();

    //[SerializeField]
    //private float _pipeStart = 20f;
    //[SerializeField]
    //private float _pipeMin = 5f;
    //[SerializeField]
    //private float _pipeMax = 15f;
    //private List<ObjectPoolItem> _pipes = new List<ObjectPoolItem>();


    //[SerializeField]
    //private Vector3 _startPos;
    //[SerializeField]
    //private float _levelSize = 40f;
    //[SerializeField]
    //private float _levelOffset = 20f;

    [SerializeField]
    private float _offset = 40f;
    private float _xProgress = 0f;
    //private float _xlevelMin = 0f;
    //private float _xlevelMax = 0f;

    private float _nextUpdate = 0f;
    private int _lastRender = -1;

    private void Update()
    {
        if (_isPlaying)
        {
            GetUpdateXProgress();

            if (_xProgress > _nextUpdate)
            {
                UpdateNextUpdate();
                //UpdateMinMaxLevel();
                //UpdateFloors();
                //UpdatePipes();
            }
        }
    }

    private void GetUpdateXProgress()
    {
        //_xProgress = _player.transform.position.x;
    }

    public override void Setup()
    {
        ImageRead.SetImage(_levelImage);

        RenderWorld(0);

        //UpdateMinMaxLevel();
        //_player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player", true);
        //ResetPlayer();
        //CreateFloors();
        //CreatePipes();

        base.Setup();
    }

    //private void CreateFloors()
    //{
        //for (int i = 0; i < 20; i++)
        //{
        //    _floors.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Floor", true));
        //}
    //}

    //private void CreatePipes()
    //{
        //for (int i = 0; i < 10; i++)
        //{
        //    _pipes.Add(ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Pipe", true));
        //}
    //}

    //private bool CheckXLevelMax(float posX)
    //{
        //return posX < _xProgress + _offset;
    //}

    //private void SetupFloors()
    //{
        //float _currentPos = _xlevelMin;

        //for (int i = 0; i < _floors.Count; i++)
        //{
        //    if (_floors[i].Item != null)
        //    {
        //        if (CheckXLevelMax(_currentPos))
        //        {
        //            _floors[i].Item.Place(_currentPos);
        //            _floors[i].SetItemActive();
        //        } else
        //        {
        //            _floors[i].SetItemNotActive();
        //        }
        //    }   

        //    _currentPos += _tileSize;
        //}
    //}

    //private void SetupPipes()
    //{
        //float _currentPos = _pipeStart;

        //for (int i = 0; i < _pipes.Count; i++)
        //{
        //    if (_pipes[i].Item != null)
        //    {
        //        if (CheckXLevelMax(_currentPos))
        //        {
        //            _pipes[i].Item.Place(_currentPos);
        //            _pipes[i].SetItemActive();
        //        }
        //        else
        //        {
        //            _pipes[i].SetItemNotActive();
        //        }
        //    }

        //    _currentPos += Random.Range(_pipeMin, _pipeMax);
        //}
    //}


    private void UpdateNextUpdate()
    {
        _nextUpdate = _xProgress + (_tileSize * 0.33f);
        int tmpIndex = Mathf.CeilToInt(_xProgress);
        RenderWorld(tmpIndex);
    }


    private void RenderWorld(int posX)
    {
        if (posX == _lastRender)
        {
            return;
        }

        Color[] _tmpStrip = ImageRead.GetPixelStripX(posX);

        for (int i = 0, max = _tmpStrip.Length; i < max; i++)
        {
            Debug.Log(_itemConfig.GetItemFromColour(_tmpStrip[i]));
        }
    }


    //private void UpdateMinMaxLevel()
    //{
    //_xlevelMin = _levelOffset + _xProgress - (_levelSize / 2);
    //_xlevelMax = _levelOffset + _xProgress + (_levelSize / 2);
    //}

    public override void Reset()
    {
        _xProgress = 0f;
        UpdateNextUpdate();
        //ResetPlayer();
        //UpdateMinMaxLevel();
        //SetupFloors();
        //SetupPipes();
        ServiceLocator.Resolve<ScoreManager>().SetScore(0f);
        base.Reset();
    }

    //private void ResetPlayer()
    //{
        //_player.transform.position = _startPos;
        //_player.transform.localEulerAngles = Vector3.zero;
        //_player.GetComponent<CharacterMove>().Reset();
        //_xProgress = _startPos.x;
    //}


    public override void Title()
    {
        Reset();
        //ServiceLocator.Resolve<CameraControl>().SetTarget(_player.transform);
    }

    public override void Play()
    {
        base.Play();
    }


    //private void UpdateFloors()
    //{
    //    float xEnd = 0f;
    //    float xPos = 0f;
    //    int floorIdx = -1;

    //    for (int i = 0; i < _floors.Count; i++)
    //    {
    //        xPos = _floors[i].Active ? _floors[i].transform.position.x : 0f;

    //        if (xPos > xEnd)
    //        {
    //            xEnd = xPos;
    //        }

    //        if (xPos < _xlevelMin)
    //        {
    //            floorIdx = i;
    //            _floors[i].SetItemNotActive();
    //        }
    //    }

    //    UpdateLastUpdate();

    //    if (floorIdx == -1)
    //    {
    //        return;
    //    }

    //    xEnd += _tileSize;

    //    if (!CheckXLevelMax(xEnd))
    //    {
    //        return;
    //    }

    //    if (_floors[floorIdx].Item != null)
    //    {
    //        _floors[floorIdx].Item.Place(xEnd);
    //        _floors[floorIdx].SetItemActive();
    //    }
    //}

    //private void UpdatePipes()
    //{
    //    float xEnd = 0f;
    //    float xPos = 0f;
    //    int pipeIdx = -1;

    //    for (int i = 0; i < _pipes.Count; i++)
    //    {
    //        xPos = _pipes[i].Active ? _pipes[i].transform.position.x : 0f;

    //        if (xPos > xEnd)
    //        {
    //            xEnd = xPos;
    //        }

    //        if (xPos < _xlevelMin)
    //        {
    //            pipeIdx = i;
    //            _pipes[i].SetItemNotActive();
    //        }
    //    }

    //    if (pipeIdx == -1)
    //    {
    //        return;
    //    }

    //    xEnd += Random.Range(_pipeMin, _pipeMax);

    //    if (!CheckXLevelMax(xEnd))
    //    {
    //        return;
    //    }

    //    if (_pipes[pipeIdx].Item != null)
    //    {
    //        _pipes[pipeIdx].Item.Place(xEnd);
    //        _pipes[pipeIdx].SetItemActive();
    //    }
    //}

    public override void UnLoad()
    {
        ServiceLocator.Resolve<CameraControl>().SetTarget();

        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetItemNotActive();
        }

        base.UnLoad();
    }
}
