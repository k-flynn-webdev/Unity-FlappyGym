using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemConfig : ScriptableObject
{
    [SerializeField]
    public List<ItemColor> Items = new List<ItemColor>();

}
