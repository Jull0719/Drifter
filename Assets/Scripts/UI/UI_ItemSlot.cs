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
    protected Inventory_Base inventory;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
        inventory = FindAnyObjectByType<Inventory_Base>();
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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null || itemInSlot.itemDataSO.itemType == ItemType.Material) return;

        if (itemInSlot.itemDataSO.itemType == ItemType.Consumable)
            inventory.TryToUse(itemInSlot);
        else
            inventory.TryToEquipItem(itemInSlot);

        if (itemInSlot == null)
            ui.itemToolTip.ShowToolTip(false, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
