using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item/Equipment Item", fileName = "Equipment Item - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("物品属性")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
