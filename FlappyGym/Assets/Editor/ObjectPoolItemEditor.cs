using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPoolItem)), CanEditMultipleObjects]
public class ObjectPoolItemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Object[] monoObjects = targets;

        if (GUILayout.Button("Return"))
        {
            for (int i = 0; i < monoObjects.Length; i++)
            {
                ObjectPoolItem myScript = (ObjectPoolItem)monoObjects[i];
                myScript.SetItemNotActive();
            }
        }
    }
}
