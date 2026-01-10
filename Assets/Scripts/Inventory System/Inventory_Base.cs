using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnUpdateUI;

    // 装备栏
    public List<Inventory_EquipSlot> equipSlotList = new List<Inventory_EquipSlot>();

    // 背包栏
    [SerializeField] protected int maxSize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
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
        itemToRemove.RemoveStack();

        if (itemToRemove.itemStackSize < 1)
            itemList.Remove(itemToRemove);

        TriggerUpdateUI();
    }

    // 移除物品项
    public void RemoveItem(Inventory_Item itemToRemove)
    {
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

    // 尝试穿戴装备
    public void TryToEquipItem(Inventory_Item itemToEquip)
    {
        Debug.Log("装备" + itemToEquip.itemDataSO.itemName);

        var matchingSlots = equipSlotList.FindAll(slot => slot.slotType == itemToEquip.itemDataSO.itemType);

        // 1.查找一个空格子，装备上对应物品
        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(itemToEquip, slot);
                return;
            }
        }

        // 2.与对应类型格子中的物品发生交换
        var slotToReplace = matchingSlots[0];
        if (itemToEquip.itemDataSO.itemType == ItemType.Accessory
            && matchingSlots[0].equipedItem.itemDataSO == itemToEquip.itemDataSO)
            slotToReplace = matchingSlots[1];
        var itemToUnequip = slotToReplace.equipedItem;

        EquipItem(itemToEquip, slotToReplace);
        UnEquipItem(itemToUnequip, slotToReplace != null);
    }

    // 穿戴装备
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipSlot slot)
    {
        slot.equipedItem = itemToEquip;
        itemToEquip.AddModifiers(player.stats);
        itemToEquip.AddEffect(player);
        RemoveItem(itemToEquip);
    }

    // 卸除装备
    public void UnEquipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (IsFull(itemToUnequip) && replacingItem == false) return;

        foreach (var equipSlot in equipSlotList)
        {
            if (equipSlot.equipedItem == itemToUnequip)
            {
                equipSlot.equipedItem = null;
                break;
            }
        }

        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveEffect();
        AddItem(itemToUnequip);
    }

    // 更新UI
    public void TriggerUpdateUI() => OnUpdateUI?.Invoke();
}
