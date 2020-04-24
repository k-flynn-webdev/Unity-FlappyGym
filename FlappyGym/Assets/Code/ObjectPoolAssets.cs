using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectPoolAssets : ScriptableObject
{
    [SerializeField]
    public List<ObjectPoolItem> Assets = new List<ObjectPoolItem>();

    // for use by editor only!
    public void SetAllAssets(List<ObjectPoolItem> items)
    {
        Assets.Clear();
        Assets = items;
    }
}
