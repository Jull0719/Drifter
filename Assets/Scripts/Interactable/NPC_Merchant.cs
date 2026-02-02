using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : NPC
{
    private Inventory_Shop shop;

    protected override void Awake()
    {
        base.Awake();
        shop = GetComponent<Inventory_Shop>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Inventory_Player inventory = collision.GetComponent<Inventory_Player>();
        shop.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        //ui.OpenShopUI(false);
    }

    public override void Interact()
    {
        base.Interact();
        ui.shopUI.SetupShop(shop);
        ui.OpenShopUI(true);
        if (!ui.inventoryUI.gameObject.activeSelf)
            ui.ToggleInventoryUI();
    }
}
