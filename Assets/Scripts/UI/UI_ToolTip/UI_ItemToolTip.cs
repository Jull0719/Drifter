using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    public void ShowToolTip(bool isShow, RectTransform target, Inventory_Item itemToShow)
    {
        base.ShowToolTip(isShow, target);

        ItemDataSO itemDataSO = itemToShow.itemDataSO;

        itemNameText.text = itemDataSO.itemName;
        itemTypeText.text = GetItemType(itemDataSO.itemType);
        itemInfoText.text = GetItemInfo(itemToShow);
        itemPriceText.text = itemDataSO.itemPrice.ToString();
    }

    private string GetItemInfo(Inventory_Item item)
    {
        string itemInfo = item.itemDataSO.itemInfo;

        if (item.itemDataSO.itemType == ItemType.Material)
            return itemInfo;

        StringBuilder sb = new StringBuilder(itemInfo);

        sb.AppendLine("");

        return sb.ToString();
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Consumable => "消耗品",
            ItemType.Material => "材料",
            _ => ""
        };
    }
}
