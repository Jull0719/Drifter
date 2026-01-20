using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player inventory { get; private set; }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;

    /// <summary>
    /// 从仓库移动到角色背包
    /// </summary>
    /// <param name="item">物品</param>
    public void FromStorageToPlayer(Inventory_Item item)
    {
        if (inventory.IsFull(item) == false)
        {
            RemoveItem(item);
            inventory.AddItem(item);
        }

        TriggerUpdateUI();
    }

    /// <summary>
    /// 从角色背包移动到仓库
    /// </summary>
    /// <param name="item">物品</param>
    public void FromPlayerToStorage(Inventory_Item item)
    {
        if (IsFull(item) == false)
        {
            inventory.RemoveItem(item);
            AddItem(item);
        }

        TriggerUpdateUI();
    }
}
