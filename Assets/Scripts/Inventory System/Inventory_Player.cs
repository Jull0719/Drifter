using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    // 装备栏
    public List<Inventory_EquipSlot> equipSlotList = new List<Inventory_EquipSlot>();
    // 金钱
    public int money = 0;

    public Inventory_Storage storage { get; private set; }

    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
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
        float healthPercent = player.health.GetHealthPercent();

        slot.equipedItem = itemToEquip;
        itemToEquip.AddEffect(player);
        itemToEquip.AddModifiers(player.stats);

        // 更新Player血量
        player.health.TriggerUpdateHealth();

        RemoveItem(itemToEquip);
    }

    // 卸除装备
    public void UnEquipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (IsFull(itemToUnequip) && replacingItem == false) return;

        float healthPercent = player.health.GetHealthPercent();

        foreach (var equipSlot in equipSlotList)
        {
            if (equipSlot.equipedItem == itemToUnequip)
            {
                equipSlot.equipedItem = null;
                break;
            }
        }

        itemToUnequip.RemoveEffect();
        itemToUnequip.RemoveModifiers(player.stats);

        // 更新Player血量
        player.health.TriggerUpdateHealth();

        AddItem(itemToUnequip);
    }
}
