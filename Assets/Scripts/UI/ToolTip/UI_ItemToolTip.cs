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
        itemPriceText.text = "价格 " + itemDataSO.itemPrice.ToString();
    }

    private string GetItemInfo(Inventory_Item item)
    {
        string itemInfo = item.itemDataSO.itemInfo;

        StringBuilder sb = new StringBuilder(itemInfo);

        if (item.itemDataSO.itemType == ItemType.Material)
            return sb.ToString();

        if (item.itemDataSO.effectDataSO != null)
        {
            sb.AppendLine();
            sb.AppendLine(GetColorText("#00FF00", item.itemDataSO.effectDataSO.effectDescription));
        }

        if (item.itemModifiers != null)
        {
            sb.AppendLine();
            foreach (var modifier in item.itemModifiers)
                sb.AppendLine(GetColorText("#FF0000", GetStatType(modifier.statType) + " " + modifier.value));
        }

        return sb.ToString();
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Consumable => "消耗品",
            ItemType.Material => "材料",
            ItemType.Weapon => "武器",
            ItemType.Armor => "防具",
            ItemType.Accessory => "饰品",
            _ => ""
        };
    }

    private string GetStatType(StatType statType)
    {
        return statType switch
        {
            StatType.MaxHealth => "生命值上限",
            StatType.HealthRegen => "生命值再生速率",
            StatType.AttackSpeed => "攻击速度",
            StatType.Damage => "物理攻击",
            StatType.CritPower => "暴击倍率",
            StatType.CritChance => "暴击概率",
            StatType.ArmorReduction => "护甲穿透",
            StatType.Armor => "护甲",
            StatType.Evasion => "闪避概率",
            StatType.Strength => "力量",
            StatType.Dexterity => "敏捷",
            StatType.Intelligence => "智力",
            StatType.Vitality => "体力",
            _ => ""
        };
    }

    // 是否为百分比
    private bool IsPercentageStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.AttackSpeed:
            case StatType.CritPower:
            case StatType.CritChance:
            case StatType.ArmorReduction:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}
