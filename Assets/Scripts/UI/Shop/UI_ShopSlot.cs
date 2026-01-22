using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ShopSlot : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
        shop.TryToBuyItem(itemInSlot, buyFullStack);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}
