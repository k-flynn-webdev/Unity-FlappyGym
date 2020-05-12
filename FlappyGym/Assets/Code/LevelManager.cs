using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, ISubscribeState
{

    [SerializeField]
    private Level[] Levels;

    [SerializeField]
    private Level _current = null;
    [SerializeField]
    private bool _ready = false;

    [SerializeField]
    public GameStateObj State { get; set; }

    private float _loadWaitTime = 1f;

    void Awake()
    {
        ServiceLocator.Register<LevelManager>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
        StartCoroutine(LoadState());
    }

    // staggered init load ..
    IEnumerator LoadState()
    {
        yield return new WaitForSeconds(0.1f);
        ServiceLocator.Resolve<GameState>().SetStateLoad();
        StartCoroutine(LoadLevelDelay());
    }

    // staggered first load
    IEnumerator LoadLevelDelay()
    {
        yield return new WaitForSeconds(_loadWaitTime);
        LoadLevel();
    }

    public Vector3 GetProgress()
    {
        return _current != null ? _current.Progress : Vector3.zero; 
    }

    void Update()
    {
        if (!_ready)
        {
            return;
        }

        if (State.state == GameStateObj.gameStates.Title ||
            State.state == GameStateObj.gameStates.Settings)
        {
            _current.Title();
        }

        if (State.state == GameStateObj.gameStates.Play)
        {
            _current.Play();
        }

        if (State.state == GameStateObj.gameStates.Pause)
        {
            _current.Pause();
        }

        if (State.state == GameStateObj.gameStates.Over)
        {
            _current.Over();
        }
    }

    private Level GetLevelByID(string level)
    {
        if (Levels.Length == 0 || level == null)
        {
            throw new System.Exception("No levels to load");
        }

        for (int i = 0, max = Levels.Length; i < max; i++)
        {
            if (Levels[i].ID.Equals(level))
            {
                return Levels[i];
            }
        }

        throw new System.Exception("No levels match");
    }

    public void LoadLevel(string levelId = "01")
    {
        _ready = false;

        Level levelToLoad = GetLevelByID(levelId);

        if (_current == null)
        {
            _current = Instantiate(levelToLoad, new Vector3(0, 0, 0), Quaternion.identity);
            _current.Load();
            return;
        }

        if (_current.ID.Equals(levelId))
        {
            ServiceLocator.Resolve<GameState>().SetStateTitle();
            return;
        }

        _current.UnLoad();
        _current = Instantiate(levelToLoad, new Vector3(0, 0, 0), Quaternion.identity);
        _current.Load();
    }

    public void ReactState(GameStateObj state)
    {
        State = state;

        // cleanup previous mode
        switch (state.last)
        {
            case GameStateObj.gameStates.Title:
                _current.TitlePost(state);
                break;
            case GameStateObj.gameStates.Play:
                _current.PlayPost(state);
                break;
            case GameStateObj.gameStates.Pause:
                _current.PausePost(state);
                break;
            case GameStateObj.gameStates.Over:
                _current.OverPost(state);
                break;
        }

        // set new mode
        switch (state.state)
        {
            case GameStateObj.gameStates.Title:
                _current.TitlePre(state);
                _ready = true;
                break;
            case GameStateObj.gameStates.Play:
                _current.PlayPre(state);
                break;
            case GameStateObj.gameStates.Pause:
                _current.PausePre(state);
                break;
            case GameStateObj.gameStates.Over:
                _current.OverPre(state);
                break;
        }
    }
}