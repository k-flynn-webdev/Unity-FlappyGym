using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemConfig : ScriptableObject
{
    [SerializeField]
    public List<ItemColor> Items = new List<ItemColor>();


    public string GetItemFromColour(Color col)
    {

        //Debug.Log(col);

        for (int i = 0, max = Items.Count; i < max; i++)
        {
            if (Items[i]._color.r == col.r &&
                Items[i]._color.g == col.g &&
                Items[i]._color.b == col.b)
            {
                return Items[i]._item.name;
            }
        }

        return "";
    }
}
