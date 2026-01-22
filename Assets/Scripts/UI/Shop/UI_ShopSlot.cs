using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ShopSlot : UI_ItemSlot
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot, true);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;
        bool rightButton = eventData.button == PointerEventData.InputButton.Right;

        if (leftButton || itemInSlot == null) return;

        if (rightButton)
        {
            bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
            shop.TryToBuyItem(itemInSlot, buyFullStack);
        }

        ui.itemToolTip.ShowToolTip(false, null);
    }
}
