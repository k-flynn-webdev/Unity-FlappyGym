using System;
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
        if (GUILayout.Button("Create"))
        {
            myScript.GetItem("TestItem");
        }
    }
}

public class ObjectPoolManager : MonoBehaviour
{

    [SerializeField]
    private static readonly Dictionary<string, ObjectPoolInfo>
    ItemPrefabs = new Dictionary<string, ObjectPoolInfo>();

    [SerializeField]
    private ObjectPoolAssets ItemPrefabsAsset;

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
        for (int i = 0, max = ItemPrefabsAsset.Assets.Count; i < max; i++)
        {
            ItemPrefabs[ItemPrefabsAsset.Assets[i].name] = new ObjectPoolInfo(ItemPrefabsAsset.Assets[i]);
        }
    }

    public ObjectPoolItem GetItem(string item)
    {
        Debug.Log("Getting " + item);
        return ItemPrefabs[item].GetItem();
    }
}
