using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    private UI_Slot[] slots;

    private Inventory_Base inventory;

    protected override void Awake()
    {
        base.Awake();
        slots = GetComponentsInChildren<UI_Slot>();

        inventory = FindFirstObjectByType<Inventory_Base>();
        inventory.OnUpdateUI += UpdateInventoryUI;

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.itemList.Count)
                slots[i].UpdateSlot(inventory.itemList[i]);
            else
                slots[i].UpdateSlot(null);
        }
    }
}
