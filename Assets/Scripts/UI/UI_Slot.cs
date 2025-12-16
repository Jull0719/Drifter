using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    private UI ui;

    private RectTransform rect;
    private Inventory_Item itemInSlot;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(Inventory_Item item)
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

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("鼠标退出");
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
