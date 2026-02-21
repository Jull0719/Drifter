using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour, ISaveable
{
    public event Action OnUpdateUI;

    // 背包栏
    [SerializeField] protected int maxSize = 10;
    public ItemDataBaseSO itemDataBase;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {

    }

    // 添加物品时，检查背包是否已满
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

    // 查找相同数据物品
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

    // 移除一个物品
    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        if (itemToRemove.itemStackSize > 1)
            itemToRemove.RemoveStack();
        else
            itemList.Remove(itemToRemove);

        TriggerUpdateUI();
    }

    // 移除物品项
    public void RemoveItem(Inventory_Item itemToRemove)
    {
        itemList.Remove(itemToRemove);
        TriggerUpdateUI();
    }

    // 更新UI
    public void TriggerUpdateUI() => OnUpdateUI?.Invoke();

    public virtual void SaveData(ref GameData saveData)
    {

    }

    public virtual void LoadData(GameData loadData)
    {

    }
}
