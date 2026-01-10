using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Material Item", fileName = "Material Item - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    [TextArea] public string itemInfo;
    public int itemPrice;
    public int maxStackSize = 1; // 最大堆叠数

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
