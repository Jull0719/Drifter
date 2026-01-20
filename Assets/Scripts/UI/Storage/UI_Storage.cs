using UnityEngine;

public class UI_Storage : UI_Base
{
    private Inventory_Storage storage;
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent storageSlotParent;
    [SerializeField] private UI_ItemSlotParent stashSlotParent;
    [SerializeField] private UI_ItemSlotParent inventorySlotParent;

    public void SetupStorage(Inventory_Storage storage)
    {
        this.storage = storage;
        this.inventory = storage.inventory;

        storage.OnUpdateUI += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        storageSlotParent.UpdateItemSlots(storage.itemList);
        stashSlotParent.UpdateItemSlots(storage.materialStashList);
        inventorySlotParent.UpdateItemSlots(inventory.itemList);
    }
}
