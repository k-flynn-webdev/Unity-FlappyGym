using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemColor
{
    [SerializeField]
    public Color _color;
    [SerializeField]
    public GameObject _item;

    public ItemColor()
    {
        _color = new Color();
    }
}
