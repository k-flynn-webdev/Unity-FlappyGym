using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public ObjectPoolItem GetItem(string item, bool activate)
    {
        if (!ItemPrefabs.ContainsKey(item))
        {
            #if UNITY_EDITOR
                Debug.Log(item + " key does not exist - object manager");
            #endif
            return null;
        }

        return ItemPrefabs[item].GetItem(activate);
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
