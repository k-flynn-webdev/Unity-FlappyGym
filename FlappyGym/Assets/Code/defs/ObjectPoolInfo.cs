using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInfo
{
    public ObjectPoolItem prefab;
    public List<ObjectPoolItem> items = new List<ObjectPoolItem>();

    public ObjectPoolInfo(ObjectPoolItem item)
    {
        this.prefab = item;
    }

    public ObjectPoolItem GetItem(bool activate)
    {
        for (int i = 0, max = items.Count; i < max; i++)
        {
            if (!items[i].Active)
            {
                if (activate)
                {
                    items[i].SetItemActive();
                    ServiceLocator.Resolve<ObjectPoolManager>().CheckCount();
                }
                return items[i];
            }
        }

        ObjectPoolItem tmpItem = this.prefab.CreateItem(activate, this.prefab.name);
        this.items.Add(tmpItem);
        ServiceLocator.Resolve<ObjectPoolManager>().CheckCount();
        return tmpItem;
    }
}
