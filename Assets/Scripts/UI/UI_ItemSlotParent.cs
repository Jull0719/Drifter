using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlotParent : MonoBehaviour
{
    private UI_ItemSlot[] itemSlots;

    public void UpdateItemSlots(List<Inventory_Item> itemList)
    {
        if (itemSlots == null)
            itemSlots = GetComponentsInChildren<UI_ItemSlot>();

        // 更新物品栏
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < itemList.Count)
                itemSlots[i].UpdateSlot(itemList[i]);
            else
                itemSlots[i].UpdateSlot(null);
        }
    }
}
