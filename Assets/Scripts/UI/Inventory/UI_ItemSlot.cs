using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected Image defaultIcon;
    [SerializeField] protected TextMeshProUGUI itemStackSize;

    protected UI ui;

    protected RectTransform rect;

    protected Inventory_Item itemInSlot;
    protected Inventory_Player inventory;
    protected Inventory_Storage storage;
    protected Inventory_Shop shop;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();

        inventory = FindAnyObjectByType<Inventory_Player>();
        storage = FindAnyObjectByType<Inventory_Storage>();
        shop = FindAnyObjectByType<Inventory_Shop>();
    }

    // 更新格子显示
    public virtual void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;
        defaultIcon?.gameObject.SetActive(itemInSlot == null);

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        itemIcon.color = Color.white;
        itemIcon.sprite = item.itemDataSO.itemIcon;
        itemStackSize.text = item.itemStackSize > 1 ? item.itemStackSize.ToString() : "";
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // 仓库开启时，存储物品
        if (ui.storageUI.gameObject.activeSelf)
            HandledStorageSlot(storage);
        // 商店开启时，交易物品
        else if (ui.shopUI.gameObject.activeSelf)
        {
            bool leftButton = eventData.button == PointerEventData.InputButton.Left;
            bool rightButton = eventData.button == PointerEventData.InputButton.Right;

            if (leftButton)
                HandledItemSlot();
            else if (rightButton)
                HandledShopSlot(shop);
        }
        else
            HandledItemSlot();

        ui.itemToolTip.ShowToolTip(false, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    public void HandledItemSlot()
    {
        if (itemInSlot == null) return;

        // 同时按下LeftControl和鼠标，可以删除物品
        bool deleteInput = Input.GetKey(KeyCode.LeftControl);
        var itemToRemove = itemInSlot;

        if (deleteInput)
        {
            inventory.RemoveOneItem(itemInSlot);
            inventory.GetComponent<Player_DropManager>().CreateDropItem(itemToRemove);
        }
        else
        {
            if (itemInSlot.itemDataSO.itemType == ItemType.Material)
                return;
            else if (itemInSlot.itemDataSO.itemType == ItemType.Consumable)
                inventory.TryToUse(itemInSlot);
            else
                inventory.TryToEquipItem(itemInSlot);
        }
    }

    /// <summary>
    /// 存储物品到仓库
    /// </summary>
    /// <param name="storage">仓库</param>
    private void HandledStorageSlot(Inventory_Storage storage)
    {
        if (itemInSlot == null) return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);

        storage.FromPlayerToStorage(itemInSlot, transferFullStack);
    }

    /// <summary>
    /// 卖出物品
    /// </summary>
    /// <param name="shop"></param>
    private void HandledShopSlot(Inventory_Shop shop)
    {
        if (itemInSlot == null) return;

        bool sellFullStack = Input.GetKey(KeyCode.LeftControl);

        shop.TryToSellItem(itemInSlot, sellFullStack);
    }
}
