using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnUpdateUI;

    [SerializeField] protected int maxSize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    public void AddItem(Inventory_Item itemToAdd)
    {
        itemList.Add(itemToAdd);
        OnUpdateUI?.Invoke();
    }

    // 检查背包是否已经满了
    public bool IsFull()
    {
        if (itemList.Count >= maxSize)
        {
            string warningText = "背包已满";
            UI.instance.SetWarningText(warningText, true);
            return true;
        }

        return false;
    }
}
