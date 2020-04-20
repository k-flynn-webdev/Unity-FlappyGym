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

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 40f;
        EditorGUILayout.PrefixLabel("Types");
        EditorGUILayout.IntField(myScript._types, GUILayout.Width(40f));
        EditorGUILayout.PrefixLabel("Active");
        EditorGUILayout.IntField(myScript._active, GUILayout.Width(40f));
        EditorGUILayout.PrefixLabel("Total");
        EditorGUILayout.IntField(myScript._total, GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        if (GUILayout.Button("Create"))
        {
            myScript.GetItem("TestItem");
        }
    }
}

public class ObjectPoolManager : MonoBehaviour
{

    [SerializeField]
    private readonly Dictionary<string, ObjectPoolInfo>
    ItemPrefabs = new Dictionary<string, ObjectPoolInfo>();

    [SerializeField]
    private ObjectPoolAssets ItemPrefabsAsset;

    [HideInInspector]
    public int _types = 0;
    [HideInInspector]
    public int _active = 0;
    [HideInInspector]
    public int _total = 0;

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
        if (!ItemPrefabs.ContainsKey(item))
        {
            return null;
        }

        Debug.Log("Getting " + item);
        return ItemPrefabs[item].GetItem();
    }

    public void CheckCount()
    {
        _types = 0;
        _active = 0;
        _total = 0;

        foreach (var item in ItemPrefabs)
        {
            _total += item.Value.items.Count;

            if (item.Value.items.Count > 0)
            {
                _types += 1;
            }
            
            for (int i = 0; i < item.Value.items.Count; i++)
            {
                if (item.Value.items[i].Active)
                {
                    _active += 1;
                }
            }
        }
    }
}
