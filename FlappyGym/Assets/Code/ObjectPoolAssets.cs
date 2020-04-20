using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPoolAssets))]
public class ObjectPoolAssetsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPoolAssets myScript = (ObjectPoolAssets)target;
        if (GUILayout.Button("AddAll"))
        {
            myScript.AddAllAssets();
        }
    }
}

[CreateAssetMenu]
public class ObjectPoolAssets : ScriptableObject
{
    [SerializeField]
    public List<ObjectPoolItem> Assets = new List<ObjectPoolItem>();

    // for use by editor only!
    public void AddAllAssets()
    {
        Assets.Clear();

        var itemsFound = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Resources/Prefabs/Objects" });

        for (int i = 0; i < itemsFound.Length; i++)
        {
            var itemGUID = itemsFound[i];
            var itemPath = AssetDatabase.GUIDToAssetPath(itemGUID);
            var itemObj = (ObjectPoolItem)AssetDatabase.LoadAssetAtPath(itemPath, typeof(ObjectPoolItem));
            Assets.Add(itemObj);
        }
        EditorUtility.SetDirty(this);
    }
}
