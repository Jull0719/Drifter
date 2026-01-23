using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DropManager : Entity_DropManager
{
    [SerializeField] private float chanceToLoseItem = 0.9f;
    private Inventory_Player inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory_Player>();
    }

    public override void DropItems()
    {
        List<Inventory_Item> itemListCopy = new List<Inventory_Item>(inventory.itemList);
        List<Inventory_EquipSlot> equipSlotListCopy = new List<Inventory_EquipSlot>(inventory.equipSlotList);

        // 背包中物品掉落
        foreach (var item in itemListCopy)
        {
            if (Random.Range(0, 100) < chanceToLoseItem * 100)
            {
                CreateDropItem(item);
                inventory.RemoveItem(item);
            }
        }

        // 背包中装备掉落
        foreach (var equipSlot in equipSlotListCopy)
        {
            if (Random.Range(0, 100) < chanceToLoseItem * 100 && equipSlot.HasItem())
            {
                var equipedItem = equipSlot.equipedItem;
                CreateDropItem(equipedItem);
                inventory.UnEquipItem(equipedItem);
                inventory.RemoveOneItem(equipedItem);
            }
        }
    }
}
