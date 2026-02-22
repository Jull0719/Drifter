using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 将物品添加到材料贮藏处
    /// </summary>
    /// <param name="item">物品</param>
    public void AddToStash(Inventory_Item item)
    {
        var stackableItem = FindStackableItemInStash(item);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
            materialStashList.Add(item);

        // 将物品按照名称进行排序
        materialStashList = materialStashList.OrderBy(item => item.itemDataSO.itemName).ToList();
    }

    public Inventory_Item FindStackableItemInStash(Inventory_Item itemToFind)
    {
        return materialStashList.Find(item => item.itemDataSO == itemToFind.itemDataSO && item.CanStacked());
    }

    public override void SaveData(ref GameData data)
    {
        data.storageDict.Clear();
        data.stashDict.Clear();

        // 仓库
        foreach (var item in itemList)
        {
            if (item != null && item.itemDataSO != null)
            {
                string saveId = item.itemDataSO.saveId;
                int itemSize = item.itemStackSize;

                if (!data.storageDict.ContainsKey(saveId))
                    data.storageDict[saveId] = 0;

                data.storageDict[saveId] += itemSize;
            }
        }

        // 材料贮藏室
        foreach (var item in materialStashList)
        {
            if (item != null && item.itemDataSO != null)
            {
                string saveId = item.itemDataSO.saveId;
                int itemSize = item.itemStackSize;

                if (!data.stashDict.ContainsKey(saveId))
                    data.stashDict[saveId] = 0;

                data.stashDict[saveId] += itemSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialStashList.Clear();

        // 仓库
        foreach (var item in data.storageDict)
        {
            string saveId = item.Key;
            int itemSize = item.Value;

            var itemDataSO = itemDataBase.FindItemDataById(saveId);
            if (itemDataSO == null)
            {
                Debug.LogWarning("找不到对应的物品数据");
                continue;
            }

            var itemToLoad = new Inventory_Item(itemDataSO);
            for (int i = 0; i < itemSize; i++)
            {
                AddItem(itemToLoad);
            }
        }

        // 物品贮藏室
        foreach (var item in data.stashDict)
        {
            string saveId = item.Key;
            int itemSize = item.Value;

            var itemDataSO = itemDataBase.FindItemDataById(saveId);
            if (itemDataSO == null)
            {
                Debug.LogWarning("找不到对应的物品数据");
                continue;
            }

            var itemToLoad = new Inventory_Item(itemDataSO);
            for (int i = 0; i < itemSize; i++)
            {
                AddToStash(itemToLoad);
            }
        }

        TriggerUpdateUI();
    }
}
