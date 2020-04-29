using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPoolAssets))]
public class ObjectPoolAssetsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPoolAssets myScript = (ObjectPoolAssets)target;
        if (GUILayout.Button("AddAll"))
        {
            AddAllAssets();
            serializedObject.ApplyModifiedProperties();
        }

    }

    // for use by editor only!
    public void AddAllAssets()
    {
        var itemsFound = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Resources/Prefabs/Objects" });
        List<ObjectPoolItem> tmpItems = new List<ObjectPoolItem>();

        for (int i = 0; i < itemsFound.Length; i++)
        {
            var itemGUID = itemsFound[i];
            var itemPath = AssetDatabase.GUIDToAssetPath(itemGUID);
            var itemObj = (ObjectPoolItem)AssetDatabase.LoadAssetAtPath(itemPath, typeof(ObjectPoolItem));
            tmpItems.Add(itemObj);
        }

        ObjectPoolAssets myScript = (ObjectPoolAssets)target;
        myScript.SetAllAssets(tmpItems);
        EditorUtility.SetDirty(myScript);
    }
}
