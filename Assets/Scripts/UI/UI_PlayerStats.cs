using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerStats : UI_Base
{
    private UI_StatSlot[] statSlots;
    private Inventory_Base inventory;

    protected override void Awake()
    {
        base.Awake();
        statSlots = GetComponentsInChildren<UI_StatSlot>();
        inventory = FindFirstObjectByType<Inventory_Base>();

        inventory.OnUpdateUI += UpdateStatSlots;
    }

    private void Start()
    {
        UpdateStatSlots();
    }

    private void UpdateStatSlots()
    {
        foreach (var slot in statSlots)
            slot.UpdateStatSlot();
    }
}
