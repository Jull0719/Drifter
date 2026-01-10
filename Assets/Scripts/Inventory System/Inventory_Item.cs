using System;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemDataSO;
    public int itemStackSize = 1;

    public ItemModifier[] itemModifiers;

    public string itemId;

    public Inventory_Item(ItemDataSO itemDataSO)
    {
        this.itemDataSO = itemDataSO;

        itemModifiers = Equipment()?.modifiers;

        itemId = itemDataSO.itemName + " - " + Guid.NewGuid();
    }

    // 判断是否为装备
    private EquipmentDataSO Equipment() => itemDataSO is EquipmentDataSO equipment ? equipment : null;

    // 添加属性修改器
    public void AddModifiers(Player_Stats playerStats)
    {
        foreach (var modifier in itemModifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.AddModifiers(modifier.value, itemId);
        }
    }

    // 移除属性修改器
    public void RemoveModifiers(Player_Stats playerStats)
    {
        foreach (var modifier in itemModifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.RemoveModifiers(itemId);
        }
    }

    // 物品效果
    public void AddEffect(Player player) => itemDataSO.effectDataSO?.Subscribe(player);
    public void RemoveEffect() => itemDataSO.effectDataSO?.Unsubscribe();

    public bool CanStacked() => itemStackSize < itemDataSO.maxStackSize;
    public void AddStack() => itemStackSize++;
    public void RemoveStack() => itemStackSize--;
}
