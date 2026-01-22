using UnityEngine;

public class NPC_InnKeeper : NPC
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = collision.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        ui.HideAllTooltip();
        ui.storageUI.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        ui.storageUI.SetupStorage(storage);
        ui.OpenStorageUI();
        ui.ToggleInventoryUI();
    }
}
