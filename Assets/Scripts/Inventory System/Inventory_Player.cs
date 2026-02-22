using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    public override void SaveData(ref GameData data)
    {
        data.money = money;

        data.inventoryDict.Clear();
        data.equipmentDict.Clear();

        // 物品存储
        foreach (var item in itemList)
        {
            if (item != null && item.itemDataSO != null)
            {
                string saveId = item.itemDataSO.saveId;

                if (!data.inventoryDict.ContainsKey(saveId))
                    data.inventoryDict[saveId] = 0;

                data.inventoryDict[saveId] += item.itemStackSize;
            }
        }

        // 装备存储
        foreach (var equipSlot in equipSlotList)
        {
            if (equipSlot.HasItem())
            {
                string saveId = equipSlot.equipedItem.itemDataSO.saveId;

                if (!data.equipmentDict.ContainsKey(saveId))
                    data.equipmentDict[saveId] = 0;

                data.equipmentDict[saveId]++;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        money = data.money;

        itemList.Clear();
        foreach (var equipSlot in equipSlotList)
        {
            if (equipSlot.HasItem())
            {
                equipSlot.equipedItem.RemoveModifiers(player.stats);
                equipSlot.equipedItem.RemoveEffect();
                equipSlot.equipedItem = null;
            }
        }

        // 物品
        foreach (var item in data.inventoryDict)
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

        // 装备
        foreach (var equipment in data.equipmentDict)
        {
            string saveId = equipment.Key;
            int itemSize = equipment.Value;

            var itemDataSO = itemDataBase.FindItemDataById(saveId);
            if (itemDataSO == null)
            {
                Debug.LogWarning("找不到对应的物品数据");
                continue;
            }

            var itemToEquipment = new Inventory_Item(itemDataSO);
            for (int i = 0; i < itemSize; i++)
            {
                TryToEquipItem(itemToEquipment);
            }
        }

        player.ui.inGameUI.UpdateUI();
        player.inventory.TriggerUpdateUI();
    }
}
