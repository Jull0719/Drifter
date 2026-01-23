using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private int maxDropAmount = 1;
    [SerializeField] private int maxDropRarity = 120;
    [SerializeField] private ItemDataBaseSO dropData;

    private void Update()
    {
        // TODO:测试掉落系统
        //if (Input.GetKeyDown(KeyCode.R))
        //    DropItems();
    }

    // 物品最终掉落列表
    public List<Inventory_Item> RollDrops()
    {
        float maxDropRarity = this.maxDropRarity; // 方便反复测试
        List<Inventory_Item> finalDropList = new List<Inventory_Item>();

        // 随机获取可能掉落的物品列表
        List<Inventory_Item> possiableList = new List<Inventory_Item>();
        foreach (var itemData in dropData.itemDataList)
        {
            float dropRate = itemData.dropRate;
            if (Random.Range(0, 100) < dropRate * 100)
            {
                var itemToAdd = new Inventory_Item(itemData);
                possiableList.Add(itemToAdd);
            }
        }

        // 物品根据稀有值降序排列
        possiableList = possiableList.OrderByDescending(item => item.itemDataSO.rarity).ToList<Inventory_Item>();

        // 最终掉落物品列表
        foreach (var item in possiableList)
        {
            if (item.itemDataSO.rarity < maxDropRarity)
            {
                maxDropRarity -= item.itemDataSO.rarity;
                finalDropList.Add(item);
            }
        }

        return finalDropList;
    }

    // 物品掉落
    public virtual void DropItems()
    {
        List<Inventory_Item> dropItems = RollDrops();
        int dropAmount = Mathf.Min(dropItems.Count, maxDropAmount);

        for (int i = 0; i < dropAmount; i++)
            CreateDropItem(dropItems[i]);
    }

    public void CreateDropItem(Inventory_Item itemToCreate)
    {
        GameObject dropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        dropItem.GetComponent<Object_PickupItem>().SetItem(itemToCreate);
    }
}
