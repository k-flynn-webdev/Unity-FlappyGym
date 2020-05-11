using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01 : Level
{

    [SerializeField]
    private ItemConfig _itemConfig;

    [SerializeField]
    private float _tileSize = 5f;

    [SerializeField]
    private Texture2D _levelTitle;
    [SerializeField]
    private float _levelTitleSize = 40f;

    [SerializeField]
    private Texture2D _levelPlay;
    [SerializeField]
    private float _levelPlaySize = 40f;


    [SerializeField]
    private Vector3 _playerStartPos;
    private bool _hasPlayer = false;
    private ObjectPoolItem _player;

    private int _offsetTiles = 0;
    private int _offsetTilesHalf = 0;
    private List<ObjectPoolItem> _items = new List<ObjectPoolItem>();

    // Next position to meet before update
    private Vector3 _lastUpdate = new Vector3();

    // value used to base an update
    private float _distanceBias = 1f;
    private float _offscreenDist = 0f;

    // Last rendered position
    private int _lastRender = -1;


    void Awake()
    {
        _offsetTiles = Mathf.RoundToInt(_levelPlaySize / _tileSize);
        _offsetTilesHalf = Mathf.RoundToInt(_offsetTiles / 2);
        _offscreenDist = _levelPlaySize * 1.25f;
    }

    public override void Load()
    {
        GetPlayer();

        base.Load();
    }

    public override void TitlePre(GameStateObj state)
    {
        ImageRead.SetImage(_levelTitle);

        base.TitlePre(state);
    }

    public override void Title()
    {
        RenderLevel();

        base.Title();
    }

    public override void PlayPre(GameStateObj state)
    {
        if (state.last == GameStateObj.gameStates.Pause)
        {
            return;
        }

        if (state.last == GameStateObj.gameStates.Title)
        {
            ImageRead.SetImage(_levelPlay);
        }

        PlayReset();

        base.PlayPre(state);
    }

    public override void Play()
    {
        RenderLevel();

        base.Play();
    }

    public override void Pause()
    {
        RenderLevel();

        base.Pause();
    }

    public override void Over()
    {
        RenderLevel();

        base.Over();
    }

    public override void OverPost(GameStateObj state)
    {
        PlayReset();

        base.OverPost(state);
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


    public void PlayReset()
    {
        for (int i = 0, max = _items.Count; i < max; i++)
        {
            _items[i].Reset();
        }

        _lastUpdate = _player.transform.position;
        _player.transform.position = _playerStartPos;

        if (_lastUpdate.x > 50f)
        {
            Vector3 _newPos = _playerStartPos;
            _newPos.z = -12f;
            ServiceLocator.Resolve<CameraControl>().SetPosition(_newPos);
        }

        UpdatePlayerProgress();

        _player.Reset();

        ClearItemsOffScreen();
        RenderLevel();
        RenderWorld(Progress);
        ServiceLocator.Resolve<GameEvent>().NewEvent("DisablePlayerInput");
    }

    private void RenderLevel()
    {
        UpdatePlayerProgress();

        float distance = Vector3.Distance(Progress, _lastUpdate);

        if (distance > _distanceBias)
        {
            _lastUpdate = Progress;
            ClearItemsOffScreen();
            RenderWorld(Progress);
        }
    }

    private void UpdatePlayerProgress()
    {
        SetProgress(_hasPlayer ? _player.transform.position : Vector3.zero);
    }



    private void GetPlayer()
    {
        _player = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Player", true);
        _player.transform.position = _playerStartPos;
        _items.Add(_player);
        _hasPlayer = true;
        ServiceLocator.Resolve<CameraControl>().SetTarget(_player.transform);
    }

    private void UnloadPlayer()
    {
        ServiceLocator.Resolve<CameraControl>().SetTarget();
        _player.SetItemNotActive();
        _items.Remove(_player);
        _hasPlayer = false;
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
                RenderPixel(x, y);
            }
        }

        _lastRender = posHash;
    }


    private void RenderPixel(int x, int y)
    {
        Color tmpCol = ImageRead.GetPixelXY(x, y);
        string objectType = _itemConfig.GetItemFromColour(tmpCol);

        if (objectType == "")
        {
            return;
        }

        Vector3 tmpPos = PositionFromPixel(x, y, 0);

        ObjectPoolItem _itemFound = FindItemAtPosition(tmpPos, objectType);
        if (_itemFound != null)
        {
            return;
        }

        ObjectPoolItem tmp = ServiceLocator.Resolve<ObjectPoolManager>().GetItem(objectType, true);
        tmp.gameObject.transform.position = tmpPos;
        _items.Add(tmp);
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
            _tmpDistance = Vector3.Distance(_items[i].transform.position, Progress);

            if (_tmpDistance > _offscreenDist)
            {
                _items[i].SetItemNotActive();
                _items.RemoveAt(i);
            }
        }
    }
}
