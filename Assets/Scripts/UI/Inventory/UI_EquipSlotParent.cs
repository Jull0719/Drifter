using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipSlot[] uiEquipSlots;

    public void UpdateEquipSlot(List<Inventory_EquipSlot> equipList)
    {
        if (uiEquipSlots == null)
            uiEquipSlots = GetComponentsInChildren<UI_EquipSlot>();

        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            if (equipList[i].HasItem())
                uiEquipSlots[i].UpdateSlot(equipList[i].equipedItem);
            else
                uiEquipSlots[i].UpdateSlot(null);
        }
    }
}
