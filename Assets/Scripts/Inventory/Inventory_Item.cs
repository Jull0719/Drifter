using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemDataSO;
    public int itemStackSize;

    public Inventory_Item(ItemDataSO itemDataSO)
    {
        this.itemDataSO = itemDataSO;
    }
}
