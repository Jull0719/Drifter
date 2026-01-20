using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public List<Inventory_Item> materialStashList;

    public Inventory_Player inventory { get; private set; }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;

    /// <summary>
    /// 从仓库移动到角色背包
    /// </summary>
    /// <param name="item">物品</param>
    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.itemStackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {

            if (inventory.IsFull(item) == false)
            {
                Inventory_Item itemToAdd = new Inventory_Item(item.itemDataSO);

                RemoveOneItem(item);
                inventory.AddItem(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }

    /// <summary>
    /// 从角色背包移动到仓库
    /// </summary>
    /// <param name="item">物品</param>
    public void FromPlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.itemStackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (IsFull(item) == false)
            {
                Inventory_Item itemToAdd = new Inventory_Item(item.itemDataSO);

                inventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }

    public void AddToStash(Inventory_Item item)
    {
        var stackableItem = FindStackableItemInStash(item);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
            materialStashList.Add(item);
    }

    public Inventory_Item FindStackableItemInStash(Inventory_Item itemToFind)
    {
        return materialStashList.Find(item => item.itemDataSO == itemToFind.itemDataSO && item.CanStacked());
    }
}
