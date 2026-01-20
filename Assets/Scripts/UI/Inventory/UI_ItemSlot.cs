using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemStackSize;

    protected UI ui;

    private RectTransform rect;

    protected Inventory_Item itemInSlot;
    protected Inventory_Player inventory;
    protected Inventory_Storage storage;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();

        inventory = FindAnyObjectByType<Inventory_Player>();
        storage = FindAnyObjectByType<Inventory_Storage>();
    }

    // 更新格子显示
    public virtual void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (item == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        itemIcon.color = Color.white;
        itemIcon.sprite = item.itemDataSO.itemIcon;
        itemStackSize.text = item.itemStackSize > 1 ? item.itemStackSize.ToString() : "";
    }

    public void OnPointerEnter(PointerEventData eventData)
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
            HandledShopSlot();
        else
            HandledItemSlot();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }

    private void HandledItemSlot()
    {
        if (itemInSlot == null || itemInSlot.itemDataSO.itemType == ItemType.Material) return;

        if (itemInSlot.itemDataSO.itemType == ItemType.Consumable)
            inventory.TryToUse(itemInSlot);
        else
            inventory.TryToEquipItem(itemInSlot);

        ui.itemToolTip.ShowToolTip(false, null);
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
        ui.itemToolTip.ShowToolTip(false, null);
    }

    private void HandledShopSlot()
    {
        Debug.Log("卖出");
    }
}
