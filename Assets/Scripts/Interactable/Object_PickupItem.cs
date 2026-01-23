using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_PickupItem : MonoBehaviour
{
    [Header("数据")]
    [SerializeField] private ItemDataSO itemDataSO;

    [Header("物品掉落")]
    [SerializeField] private Vector2 dropPower = new Vector2(4, 5);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sr;

    private Inventory_Player inventory;
    private Inventory_Storage storage;

    private void OnValidate()
    {
        if (itemDataSO == null) return;

        SetVisuals();
    }

    private void SetVisuals()
    {
        sr.sprite = itemDataSO.itemIcon;
        gameObject.name = itemDataSO.GetItemType(itemDataSO.itemType) + " - " + itemDataSO.itemName;
    }

    public void SetItem(Inventory_Item itemToSet)
    {
        this.itemDataSO = itemToSet.itemDataSO;
        SetVisuals();

        // 掉落时物理设置
        float xDropForce = Random.Range(-dropPower.x, dropPower.x);
        rb.velocity = new Vector2(xDropForce, dropPower.y);
        col.isTrigger = false; // 防止刚要掉落就被Player拾取
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventory = collision.GetComponent<Inventory_Player>();
        if (inventory == null) return;

        storage = inventory.storage;

        var item = new Inventory_Item(itemDataSO);
        // 如果是材料，直接加入材料贮藏处
        if (itemDataSO.itemType == ItemType.Material)
        {
            storage.AddToStash(item);
            PickUpItem(true);
            return;
        }

        if (inventory.IsFull(item)) return;
        inventory.AddItem(item);
        PickUpItem(false);
    }

    public void PickUpItem(bool addToStash)
    {
        string infoText = addToStash ? "，已加入材料贮藏室" : "";
        string warningText = "拾取了" + itemDataSO.itemName + infoText;
        UI.instance.SetWarningText(warningText, false);
        Destroy(gameObject);
    }
}