using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolItem : MonoBehaviour
{
    [SerializeField]
    public bool Active
    { get { return this._isActive; } }

    [SerializeField]
    private bool _isActive = false;

    public Item Item;
    public IReset[] ItemResets;

    private void Awake()
    {
        Item = GetComponent<Item>();
        ItemResets = GetComponentsInChildren<IReset>();
        _isActive = false;
        this.gameObject.SetActive(false);
    }

    public void SetItemActive()
    {
        _isActive = true;
        this.gameObject.SetActive(true);
    }

    public void SetItemNotActive()
    {
        _isActive = false;
        this.gameObject.SetActive(false);
        for (int i = 0; i < ItemResets.Length; i++)
        {
            ItemResets[i].Reset();
        }
        ServiceLocator.Resolve<ObjectPoolManager>().CheckCount();
    }

    public ObjectPoolItem CreateItem(bool activate)
    {
        ObjectPoolItem tmpObj = Instantiate(this, new Vector3(0, 0, 0), Quaternion.identity);
        if (activate)
        {
            tmpObj.SetItemActive();
        }
        return tmpObj;
    }
}
