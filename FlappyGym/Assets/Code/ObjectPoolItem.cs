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

    private void Awake()
    {
        Item = GetComponent<Item>();
    }

    public void IsActive()
    {
        _isActive = true;
        this.gameObject.SetActive(true);
    }

    public void IsNotActive()
    {
        _isActive = false;
        this.gameObject.SetActive(false);
        ServiceLocator.Resolve<ObjectPoolManager>().CheckCount();
    }

    public ObjectPoolItem CreateItem()
    {
        ObjectPoolItem tmpObj = Instantiate(this, new Vector3(0, 0, 0), Quaternion.identity);
        tmpObj.IsActive();
        return tmpObj;
    }
}
