using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item", fileName = "Item - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    [TextArea] public string itemInfo;
    public int itemPrice;
    public int maxStackSize = 1; // 最大堆叠数
}
