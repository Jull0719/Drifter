using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item", fileName = "Item - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStackSize = 1; // 最大堆叠数
}
