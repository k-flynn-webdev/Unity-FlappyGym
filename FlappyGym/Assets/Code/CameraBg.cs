using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBg : MonoBehaviour, ISubscribeState
{

    List<ObjectPoolItem> _clouds = new List<ObjectPoolItem>();

    [SerializeField]
    private Transform _screenLeft;
    [SerializeField]
    private Transform _screenRight;

    private Vector3 _ScreenMid;

    [SerializeField]
    private int _cloudsMin = 10;
    [SerializeField]
    private int _cloudsMax = 10;

    private bool _ready = false;
    private float _checkTime = 0f;
    private float _screenWidthDist = 0f;

    [SerializeField]
    public GameStateObj State { get; set; }


    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }


    void Update()
    {
        if (!_ready)
        {
            return;
        }

        if (Time.time > _checkTime)
        {
            CheckClouds();
            _checkTime = Time.time + 0.5f;
        }
    }

    private void Init()
    {
        _ScreenMid = Vector3.Lerp(_screenLeft.position, _screenRight.position, 0.5f);
        _screenWidthDist = Vector3.Distance(_screenLeft.position, _screenRight.position) * 0.6f;

        int cloudCount = Random.Range(_cloudsMin, _cloudsMax);

        for (int i = 0; i < cloudCount; i++)
        {
            ObjectPoolItem tmp = ServiceLocator.Resolve<ObjectPoolManager>().GetItem("Cloud", true);
            _clouds.Add(tmp);
        }
    }

    private void ResetClouds()
    {
        for (int i = 0; i < _clouds.Count; i++)
        {
            ResetCloud(_clouds[i]);
        }
    }

    private void ResetCloud(ObjectPoolItem cloud)
    {
        Vector3 newPos = Vector3.Lerp(_screenLeft.position, _screenRight.position, Random.Range(0f, 1f));
        cloud.transform.position = newPos;
        cloud.Reset();
    }

    private void CheckClouds()
    {
        float tmpDistance = 0f;

        for (int i = 0, max = _clouds.Count; i < max; i++)
        {
            tmpDistance = Vector3.Distance(_ScreenMid, _clouds[i].transform.position);

            if (tmpDistance > _screenWidthDist)
            {
                _clouds[i].transform.position = _screenLeft.position;
                _clouds[i].Reset();
            }
        }
    }

    public void ReactState(GameStateObj state)
    {
        State = state;

        if (!_ready)
        {
            Init();
            ResetClouds();
            _ready = true;
        }
    }
}
