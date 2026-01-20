using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_PickupItem : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemDataSO;

    private SpriteRenderer sr;

    private Inventory_Player inventory;
    private Inventory_Storage storage;

    private Inventory_Item item;

    private void Awake()
    {
        item = new Inventory_Item(itemDataSO);
    }

    private void OnValidate()
    {
        gameObject.name = itemDataSO.GetItemType(itemDataSO.itemType) + " - " + itemDataSO.itemName;
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = itemDataSO.itemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventory = collision.GetComponent<Inventory_Player>();
        storage = inventory.storage;

        // 如果是材料，直接加入材料贮藏处
        if (itemDataSO.itemType == ItemType.Material)
        {
            storage.AddToStash(item);
            PickUpItem();
            return;
        }

        if (inventory.IsFull(item)) return;
        inventory.AddItem(item);
        PickUpItem();
    }

    public void PickUpItem()
    {
        string warningText = "拾取了" + itemDataSO.itemName;
        UI.instance.SetWarningText(warningText, false);
        Destroy(gameObject);
    }
}