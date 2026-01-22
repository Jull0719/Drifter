using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item/Material Item", fileName = "Material Item - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    [TextArea] public string itemInfo;
    public int maxStackSize = 1; // 最大堆叠数
    [Space]
    [Header("商店")]
    [Range(100, 10000)] public int itemPrice = 10; // 价格
    public int minStackSizeInShop = 1; // 商店里的最小堆叠数
    public int maxStackSizeInShop = 1; // 商店里的最大堆叠数

    [Header("物品效果")]
    public ItemEffectDataSO effectDataSO;

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
}
