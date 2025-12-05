using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    public void UpdateSlot(Inventory_Item item)
    {
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
}
