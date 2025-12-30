using System;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemDataSO;
    public int itemStackSize = 1;


    public Inventory_Item(ItemDataSO itemDataSO)
    {
        this.itemDataSO = itemDataSO;
    }

    public bool CanStacked() => itemStackSize < itemDataSO.maxStackSize;
    public void AddStack() => itemStackSize++;
    public void RemoveStack() => itemStackSize--;
}
