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
    private float _tileSize = 5f;

    private List<ObjectPoolItem> _items = new List<ObjectPoolItem>();

    private bool _hasPlayer = false;
    private ObjectPoolItem _player;
    [SerializeField]
    private Vector3 _playerStartPos;


    [SerializeField]
    private float _levelDisplaySize = 40f;

    private int _offsetTiles = 0;
    private int _offsetTilesHalf = 0;


    // Current position of interest
    [SerializeField]
    private Vector3 _progress = new Vector3();

    // Next position to meet before update
    private Vector3 _lastUpdate = new Vector3();

    // value used to base an update
    private float _distanceBias = 1f;
    private float _offscreenDist = 0f;

    // Last rendered position
    private int _lastRender = -1;


    void Awake()
    {
        _offsetTiles = Mathf.RoundToInt(_levelDisplaySize / _tileSize);
        _offsetTilesHalf = Mathf.RoundToInt(_offsetTiles / 2);
        _offscreenDist = _levelDisplaySize * 1.25f;
    }


    private void Update()
    {
        if (_isPlaying)
        {
            UpdateProgress();

            float distance = Vector3.Distance(_progress, _lastUpdate);

            if (distance > _distanceBias)
            {
                UpdateLastUpdate();
            }
        }
    }

    private void UpdateProgress()
    {
        _progress = _hasPlayer ? _player.transform.position : Vector3.zero;
    }

    public override void Setup()
    {
        ImageRead.SetImage(_levelImage);
        ClearItemsOffScreen();
        RenderWorld(_progress);
        GetPlayer();

        base.Setup();
    }

    private void GetPlayer()
    {
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player", true);
        _player.transform.position = _playerStartPos;
        _items.Add(_player);
        _hasPlayer = true;
    }

    private void UnloadPlayer()
    {
        ServiceLocator.Resolve<CameraControl>().SetTarget();
        _player.SetItemNotActive();
        _hasPlayer = false;
    }

    private void UpdateLastUpdate()
    {
        _lastUpdate = _progress;
        ClearItemsOffScreen();
        RenderWorld(_progress);
    }

    private void RenderWorld(Vector3 pos)
    {
        int[] posArray = PositionToPixel(pos);
        int posHash = posArray[0] + posArray[1] + posArray[2];

        if (posHash == _lastRender)
        {
            return;
        }

        int startX = posArray[0] - _offsetTilesHalf;
        int endX = posArray[0] + _offsetTilesHalf + 1;
        int startY = posArray[1] - _offsetTilesHalf;
        int endY = posArray[1] + _offsetTilesHalf + 1;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {

                Color tmpCol = ImageRead.GetPixelXY(x, y);
                string objectType = _itemConfig.GetItemFromColour(tmpCol);

                if (objectType == "")
                {
                    continue;
                }

                Vector3 tmpPos = PositionFromPixel(x, y, 0);

                ObjectPoolItem _itemFound = FindItemAtPosition(tmpPos, objectType);
                if (_itemFound != null)
                {
                    continue;
                }

                ObjectPoolItem tmp = ServiceLocator.Resolve<ObjectPoolManager>().GetItem(objectType, true);
                tmp.gameObject.transform.position = tmpPos;
                _items.Add(tmp);
            }
        }

        _lastRender = posHash;
    }

    private int[] PositionToPixel(Vector3 pos)
    {
        int[] tmp = new int[3];
        tmp[0] = (int)pos.x / (int)_tileSize;
        tmp[1] = (int)pos.y / (int)_tileSize;
        tmp[2] = (int)pos.z / (int)_tileSize;

        return tmp;
    }

    private Vector3 PositionFromPixel(int x, int y, int z)
    {
        return new Vector3(x * _tileSize, y * _tileSize, z * _tileSize);
    }

    private ObjectPoolItem FindItemAtPosition(Vector3 pos, string type)
    {
        float _tmpDistance = 0f;

        for (int i = 0, max = _items.Count; i < max; i++)
        {
            _tmpDistance = Vector3.Distance(_items[i].transform.position, pos);

            if (_tmpDistance < _distanceBias && _items[i].name.Equals(type))
            {
                return _items[i];
            }
        }

        return null;
    }


    private void ClearItemsOffScreen()
    {
        float _tmpDistance = 0f;

        for (int i = _items.Count - 1; i >= 0; i--)
        {
            _tmpDistance = Vector3.Distance(_items[i].transform.position, _progress);

            if (_tmpDistance > _offscreenDist)
            {
                _items[i].SetItemNotActive();
                _items.RemoveAt(i);
            }
        }
    }

    public override void Reset()
    {
        _progress = Vector3.zero;
        _player.transform.position = _playerStartPos;

        for (int i = 0, max = _items.Count; i < max; i++)
        {
            _items[i].Reset();
        }

        UpdateLastUpdate();
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

    public override void UnLoad()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetItemNotActive();
        }

        _items.Clear();

        UnloadPlayer();

        base.UnLoad();
    }
}
