using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPoolManager))]
public class ObjectPoolManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPoolManager myScript = (ObjectPoolManager)target;

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        EditorGUILayout.Separator();
        EditorGUIUtility.labelWidth = 40f;
        EditorGUILayout.PrefixLabel("Types");
        EditorGUILayout.IntField(myScript._types, GUILayout.Width(40f));
        EditorGUILayout.PrefixLabel("Active");
        EditorGUILayout.IntField(myScript._active, GUILayout.Width(40f));
        EditorGUILayout.PrefixLabel("Total");
        EditorGUILayout.IntField(myScript._total, GUILayout.Width(40f));
        EditorGUILayout.Separator();
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        if (GUILayout.Button("Create"))
        {
            myScript.GetItem("TestItem", true);
        }
    }
}