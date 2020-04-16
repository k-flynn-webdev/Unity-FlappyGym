using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IObservable
{

    [SerializeField]
    private Level[] Levels;

    [SerializeField]
    private Level _current;


    void Awake()
    {
        ServiceLocator.Register<LevelManager>(this);
    }

    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    void LoadALevel(int levelId = 0)
    {

        Level levelToLoad = null;
        for (int i = 0, max = Levels.Length; i < max; i++)
        {
            if (Levels[i].Id == levelId)
            {
                levelToLoad = Levels[i];
                break;
            }
        }

        if (Levels.Length == 0 || levelToLoad == null)
        {
            Debug.Log("No levels to load.");
            return;
        }

        if (_current == null)
        {
            _current = Instantiate(levelToLoad);
            SetupLevel();
            return;
        }

        if (_current.Id == levelToLoad.Id)
        {
            ResetLevel();
            return;
        }

        if (levelToLoad.Id != _current.Id)
        {
            UnloadLevel();
            _current = Instantiate(levelToLoad);
            SetupLevel();
        }
    }


    void SetupLevel()
    {
        if (_current != null)
        {
            _current.Setup();
        }
    }

    void TitleLevel()
    {
        if (_current != null)
        {
            _current.Title();
        }
    }

    void PlayLevel(GameStateObj state)
    {
        if (_current != null)
        {
            _current.Play();
        }
    }

    void ResetLevel()
    {
        if (_current != null)
        {
            _current.Reset();
        }
    }

    void PauseLevel()
    {
        if (_current != null)
        {
            _current.Pause();
        }
    }

    void OverLevel()
    {
        if (_current != null)
        {
            _current.Over();
        }
    }

    void UnloadLevel()
    {
        if (_current != null)
        {
            _current.UnLoad();
        }
    }




    public List<IObservable> Subscribers
    { get; }

    public void Notify() { }

    public void React(GameStateObj state)
    {
        switch (state.state)
        {
            case GameStateObj.gameStates.Main:
                if (_current == null)
                {
                    LoadALevel();
                    break;
                }
                TitleLevel();
                break;
            case GameStateObj.gameStates.Play:
                PlayLevel(state);
                break;
            case GameStateObj.gameStates.Pause:
                PauseLevel();
                break;
        }
    }

    public void Subscribe(IObservable listener) { }
    public void UnSubscribe(IObservable listener) { }




}