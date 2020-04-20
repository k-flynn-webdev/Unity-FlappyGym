using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPoolManager))]
public class ObjectPoolManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectPoolManager myScript = (ObjectPoolManager)target;
        if (GUILayout.Button("AddAll"))
        {
            myScript.AddAllAssets();
        }

        if (GUILayout.Button("CreateTest"))
        {
            Debug.Log("creating");
            ServiceLocator.Resolve<ObjectPoolManager>().GetItem("TestItem");
        }
    }
}

public class ObjectPoolManager : MonoBehaviour
{

    [SerializeField]
    private static readonly Dictionary<string, ObjectPoolInfo>
    ItemPrefabs = new Dictionary<string, ObjectPoolInfo>();

    [SerializeField]
    private List<ObjectPoolItem> ItemPrefabsList = new List<ObjectPoolItem>();

    void Awake()
    {
        ServiceLocator.Register<ObjectPoolManager>(this);
    }

    void Start()
    {
        SetupObjectPrefabs();
    }

    private void SetupObjectPrefabs()
    {
        for (int i = 0, max = ItemPrefabsList.Count; i < max; i++)
        {
            ItemPrefabs[ItemPrefabsList[i].name] = new ObjectPoolInfo(ItemPrefabsList[i]);
        }
    }

    public ObjectPoolItem GetItem(string item)
    {
        Debug.Log("Getting " + item);
        return ItemPrefabs[item].GetItem();
    }


    // for use by editor only!
    public void AddAllAssets()
    {
        ItemPrefabsList.Clear();

        var itemsFound = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Resources/Prefabs/Objects" });

        for (int i = 0; i < itemsFound.Length; i++)
        {
            var itemGUID = itemsFound[i];
            var itemPath = AssetDatabase.GUIDToAssetPath(itemGUID);
            var itemObj = (ObjectPoolItem)AssetDatabase.LoadAssetAtPath(itemPath, typeof(ObjectPoolItem));
            ItemPrefabsList.Add(itemObj);
        }


        SetupObjectPrefabs();
    }
}
