using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameState))]
public class GameStateEditor : Editor
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