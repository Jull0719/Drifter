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
}
