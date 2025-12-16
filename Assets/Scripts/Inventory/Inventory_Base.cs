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
        if (itemList.Count >= maxSize && FindItem(item) == null)
        {
            string warningText = "背包已满";
            UI.instance.SetWarningText(warningText, true);
            return true;
        }

        return false;
    }

    // 查找数据相同的物品
    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item.itemDataSO == itemToFind.itemDataSO && item.CanStacked());
    }

    public void AddItem(Inventory_Item itemToAdd)
    {
        // 查找可堆叠物品
        Inventory_Item itemToStack = FindItem(itemToAdd);

        if (itemToStack != null)
        {
            itemToStack.AddStack();
        }
        else
            itemList.Add(itemToAdd);

        OnUpdateUI?.Invoke();
    }
}
