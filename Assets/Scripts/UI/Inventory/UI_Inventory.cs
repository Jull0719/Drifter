using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    private UI_ItemSlotParent itemSlotParent;
    private UI_EquipSlotParent equipSlotParent;

    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();

        itemSlotParent = GetComponentInChildren<UI_ItemSlotParent>();
        equipSlotParent = GetComponentInChildren<UI_EquipSlotParent>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnUpdateUI += UpdateInventoryUI;

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        itemSlotParent.UpdateItemSlots(inventory.itemList);
        equipSlotParent?.UpdateEquipSlot(inventory.equipSlotList);
    }
}
