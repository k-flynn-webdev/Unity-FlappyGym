using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemConfig))]
public class ItemConfigEditor : Editor
{

    public Color test;

    public override void OnInspectorGUI()
    {

        ItemConfig myScript = (ItemConfig)target;

        if (GUILayout.Button("Add"))
        {
            myScript.Items.Add(new ItemColor());
            serializedObject.ApplyModifiedProperties();
        }

        if (myScript.Items.Count > 0)
        {
            for (int i = 0; i < myScript.Items.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                myScript.Items[i]._color = EditorGUILayout.ColorField(myScript.Items[i]._color);
                myScript.Items[i]._item = (GameObject)EditorGUILayout.ObjectField(myScript.Items[i]._item, typeof(GameObject), true);
                EditorGUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Save"))
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(myScript);
        }
    }
}
