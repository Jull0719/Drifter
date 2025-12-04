using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_PickupItem : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemDataSO;

    private SpriteRenderer sr;

    private Inventory_Base inventory;
    private Inventory_Item item;

    private void Awake()
    {
        item = new Inventory_Item(itemDataSO);
    }

    private void OnValidate()
    {
        gameObject.name = "Object - " + itemDataSO.itemName;
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = itemDataSO.itemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventory = collision.GetComponent<Inventory_Base>();

        if (inventory == null || inventory.IsFull()) return;

        inventory.AddItem(item);
        string warningText = "拾取了" + itemDataSO.itemName;
        UI.instance.SetWarningText(warningText, false);

        Destroy(gameObject);
    }
}
