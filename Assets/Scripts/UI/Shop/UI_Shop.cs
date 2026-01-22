using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    private Inventory_Shop shop;
    private Inventory_Player inventory;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private UI_ItemSlotParent itemSlotParent;
    [SerializeField] private UI_ItemSlotParent shopSlotParent;

    public void SetupShop(Inventory_Shop shop)
    {
        this.shop = shop;
        inventory = shop.inventory;

        shop.OnUpdateUI += UpdateShopUI;
        inventory.OnUpdateUI += UpdateShopUI;

        UpdateShopUI();
    }

    private void UpdateShopUI()
    {
        shopSlotParent.UpdateItemSlots(shop.itemList);
        itemSlotParent.UpdateItemSlots(shop.inventory.itemList);

        // 更新金钱的显示
        moneyText.text = inventory.money.ToString();
    }
}
