using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnUpdateUI;

    [SerializeField] protected int maxSize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    // 检查背包是否已经满了
    public bool IsFull(Inventory_Item item)
    {
        if (itemList.Count >= maxSize && FindStackableItem(item) == null)
        {
            string warningText = "背包已满";
            UI.instance.SetWarningText(warningText, true);
            return true;
        }

        return false;
    }

    // 查找物品
    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }

    // 查找可堆叠物品
    public Inventory_Item FindStackableItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item.itemDataSO == itemToFind.itemDataSO && item.CanStacked());
    }

    // 添加物品
    public void AddItem(Inventory_Item itemToAdd)
    {
        // 查找可堆叠物品
        Inventory_Item itemToStack = FindStackableItem(itemToAdd);

        if (itemToStack != null)
        {
            itemToStack.AddStack();
        }
        else
            itemList.Add(itemToAdd);

        TriggerUpdateUI();
    }

    // 移除物品
    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        itemToRemove.RemoveStack();

        if (itemToRemove.itemStackSize < 1)
            itemList.Remove(itemToRemove);

        TriggerUpdateUI();
    }

    // 使用物品
    public void TryToUse(Inventory_Item itemToUse)
    {
        if (itemToUse.itemDataSO.effectDataSO.CanBeUsed() == false)
            return;
        else
        {
            itemToUse.itemDataSO.effectDataSO.Execute();
            RemoveOneItem(itemToUse);
        }
    }

    // 更新UI
    public void TriggerUpdateUI() => OnUpdateUI?.Invoke();
}
