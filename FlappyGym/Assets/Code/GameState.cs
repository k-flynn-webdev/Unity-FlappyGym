using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(GameState))]
public class SomeScriptEditor : Editor
{

    public GameState.gameStates newState;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        newState = (GameState.gameStates)EditorGUILayout.EnumPopup(newState);

        GameState myScript = (GameState)target;
        if (GUILayout.Button("Change"))
        {
            myScript.ChangeState(newState);
        }
    }
}

public class GameState : MonoBehaviour, IObservable 
{

    public enum gameStates { Load, Main, Play, Pause, Over };

    [SerializeField]
    private gameStates _gameState;
    [SerializeField]
    private gameStates _gameStateLast;


    [SerializeField]
    public gameStates state
    { get { return this._gameState; } }



    public void ChangeState(gameStates state)
    {

        if (this._gameState == state)
        {
            return;
        }

        switch (state)
        {
            case gameStates.Load:
                this.SetState(gameStates.Load);
                break;
            case gameStates.Main:
                this.SetState(gameStates.Main);
                break;
            case gameStates.Play:
                this.SetState(gameStates.Play);
                break;
            case gameStates.Pause:
                this.SetState(gameStates.Pause);
                break;
            case gameStates.Over:
                this.SetState(gameStates.Over);
                break;
        }
    }

    public void SetState(gameStates state)
    {
        this._gameStateLast = this._gameState;
        this._gameState = state;
        Debug.Log(this.state); // todo
        // anounce reactive change
        this.Notify();
    }


    public List<IObservable> Subscribers
    { get { return this.subscribers; } }
    
    private List<IObservable> subscribers = new List<IObservable>();

    public void Notify()
    {
        for (int i = subscribers.Count - 1; i >= 0; i--)
        {
            subscribers[i].React();
        }
    }

    public void React() { }

    public void Subscribe(IObservable listener)
    { subscribers.Add(listener); }

    public void UnSubscribe(IObservable listener)
    { subscribers.Remove(listener); }
}
