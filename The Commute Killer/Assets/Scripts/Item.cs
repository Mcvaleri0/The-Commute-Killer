using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        SharpWeapon,
        BluntWeapon,
        Tool,
        Food
    }

    public Agent Owner { get; set; }

    public float Durability { get; set; }

    public List<ItemType> Types { get; set; }

    public string Name { get; set; }

    public Item(ItemType type, string name) {
        this.Types = new List<ItemType>()
        {
            type
        };

        this.Name = Name;

        this.Owner = null;

        this.Durability = 1;
    }

    public Item(List<ItemType> types, string name)
    {

    }
}
