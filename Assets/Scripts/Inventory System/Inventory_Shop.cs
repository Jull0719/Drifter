using System.Collections.Generic;
using UnityEngine;

public class Inventory_Shop : Inventory_Base
{
    [SerializeField] private ItemDataBaseSO shopData;
    [Header("物品的数量")]
    [SerializeField] private int minItemsAmount = 4;
    [SerializeField] private int maxItemsAmount = 18;

    public Inventory_Player inventory { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        FillShopList();
    }

    // 测试商店填装物品
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            FillShopList();
    }

    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;

    // 填装商店货物
    public void FillShopList()
    {
        itemList.Clear();

        // 1.商店可能会出现的物品列表
        List<Inventory_Item> possiableItems = new List<Inventory_Item>();

        foreach (var itemData in shopData.itemDataList)
        {
            int randomStackSize = Random.Range(itemData.minStackSizeInShop, itemData.maxStackSizeInShop + 1);
            int finalStackSize = Mathf.Clamp(randomStackSize, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.itemStackSize = finalStackSize;
            possiableItems.Add(itemToAdd);
        }

        // 2.随机生成物品列表
        int randomItemsAmount = Random.Range(minItemsAmount, maxItemsAmount + 1);
        int finalItemsAmount = Mathf.Clamp(randomItemsAmount, 1, possiableItems.Count);

        for (int i = 0; i < finalItemsAmount; i++)
        {
            int randomIndex = Random.Range(0, possiableItems.Count);
            var itemToAdd = possiableItems[randomIndex];

            if (IsFull(itemToAdd) == false)
            {
                AddItem(itemToAdd);
                possiableItems.Remove(itemToAdd);
            }
        }

        TriggerUpdateUI();
    }

    // 购买物品
    public void TryToBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int buyAmount = buyFullStack ? itemToBuy.itemStackSize : 1;

        for (int i = 0; i < buyAmount; i++)
        {
            // 如果金钱不够
            if (inventory.money < itemToBuy.buyingPrice)
            {
                UI.instance.SetWarningText("金钱不够", true);
                return;
            }

            Inventory_Item itemToAdd = new Inventory_Item(itemToBuy.itemDataSO);

            // 如果购买的是材料直接转移到材料贮藏处
            if (itemToBuy.itemDataSO.itemType == ItemType.Material)
                inventory.storage.AddToStash(itemToAdd);
            else // 否则加入Player背包
            {
                if (inventory.IsFull(itemToBuy)) return; // 需要检查Player背包是否还有空间
                inventory.AddItem(itemToAdd);
            }

            // 商店中移除对应物品，Player金钱相应减少
            inventory.money -= itemToBuy.buyingPrice;
            RemoveOneItem(itemToBuy);
        }

        TriggerUpdateUI();
    }

    // 出售物品
    public void TryToSellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int sellAmount = sellFullStack ? itemToSell.itemStackSize : 1;

        for (int i = 0; i < sellAmount; i++)
        {
            inventory.money += itemToSell.sellingPrice;
            inventory.RemoveOneItem(itemToSell);
        }

        TriggerUpdateUI();
    }
}
