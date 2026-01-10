using System;

[Serializable]
public class Inventory_EquipSlot
{
    public ItemType slotType;
    public Inventory_Item equipedItem;

    public bool HasItem() => equipedItem != null && equipedItem.itemDataSO != null;
}
