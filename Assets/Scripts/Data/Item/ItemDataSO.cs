using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item/Material Item", fileName = "Material Item - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("基本信息")]
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    [TextArea] public string itemInfo;
    public int maxStackSize = 1; // 最大堆叠数
    [Space]
    [Header("商店信息")]
    [Range(100, 10000)] public int itemPrice = 10; // 价格
    public int minStackSizeInShop = 1; // 商店里的最小堆叠数
    public int maxStackSizeInShop = 1; // 商店里的最大堆叠数
    [Space]
    [Header("掉落设置")]
    [Range(0, 100)] public int rarity = 1;
    [Range(0, 1)] public float dropRate;
    [Range(0, 1)] public float maxDropRate = 0.7f;
    [Space]
    [Header("物品效果")]
    public ItemEffectDataSO effectDataSO;

    private void OnValidate()
    {
        dropRate = GetDropRate();
    }

    /// <summary>
    /// 获取物品类型对应显示的文字
    /// </summary>
    /// <param name="itemType">物品类型</param>
    /// <returns></returns>
    public string GetItemType(ItemType itemType)
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

    /// <summary>
    /// 计算掉落概率
    /// </summary>
    /// <returns>掉落概率</returns>
    public float GetDropRate()
    {
        int maxRarity = 100;
        dropRate = Mathf.Abs(maxRarity - rarity + 1) / (maxRarity * 1.0f);
        return Mathf.Min(dropRate, maxDropRate);
    }
}
