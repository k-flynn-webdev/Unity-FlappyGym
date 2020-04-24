using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(GameState))]
public class SomeScriptEditor : Editor
{
    public GameStateObj.gameStates newState;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();

        newState = (GameStateObj.gameStates)EditorGUILayout.EnumPopup(newState);

        GUILayout.Space(50f);

        GameState myScript = (GameState)target;
        if (GUILayout.Button("Change"))
        {
            myScript.ChangeState(newState);
        }

        GUILayout.EndHorizontal();
    }
}

public class GameState : MonoBehaviour, IPublish
{

    [SerializeField]
    private GameStateObj _gameState = new GameStateObj();

    [SerializeField]
    private GameStateObj.gameStates _state;
    [SerializeField]
    private GameStateObj.gameStates _last;


    void Awake()
    {
        ServiceLocator.Register<GameState>(this);
    }

    private void Start()
    {
        StartCoroutine(DelayLoad());
    }


    IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(1f);
        SetStateLoad();
    }

    public void ChangeState(GameStateObj.gameStates state)
    {
        if (this._gameState.state == state)
        {
            return;
        }

        switch (state)
        {
            case GameStateObj.gameStates.Load:
                this.SetState(GameStateObj.gameStates.Load);
                break;
            case GameStateObj.gameStates.Main:
                this.SetState(GameStateObj.gameStates.Main);
                break;
            case GameStateObj.gameStates.Play:
                this.SetState(GameStateObj.gameStates.Play);
                break;
            case GameStateObj.gameStates.Pause:
                this.SetState(GameStateObj.gameStates.Pause);
                break; 
            case GameStateObj.gameStates.Over:
                this.SetState(GameStateObj.gameStates.Over);
                break;
        }
    }

    public void SetState(GameStateObj.gameStates state)
    {
        this._gameState.last = this._gameState.state;
        this._gameState.state = state;

        this._state = state;
        this._last = this._gameState.last;

        this.Notify();
    }

    public void SetStateLoad() => ChangeState(GameStateObj.gameStates.Load);
    public void SetStateMain() => ChangeState(GameStateObj.gameStates.Main);
    public void SetStatePlay() => ChangeState(GameStateObj.gameStates.Play);
    public void SetStatePause() => ChangeState(GameStateObj.gameStates.Pause);
    public void SetStateOver() => ChangeState(GameStateObj.gameStates.Over);

    public List<ISubscribe> Subscribers
    { get { return this.subscribers; } }
    
    private List<ISubscribe> subscribers = new List<ISubscribe>();

    public void Notify()
    {
        for (int i = subscribers.Count - 1; i >= 0; i--)
        {
            subscribers[i].React(this._gameState);
        }
    }

    public void Subscribe(ISubscribe listener)
    { subscribers.Add(listener); }

    public void UnSubscribe(ISubscribe listener)
    { subscribers.Remove(listener); }
}
