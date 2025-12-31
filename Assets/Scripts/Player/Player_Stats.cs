using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> ActiveBuffs = new List<string>();
    private Inventory_Base inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory_Base>();
    }

    public bool CanApplyBuff(string source) => ActiveBuffs.Contains(source) == false;

    public void ApplyBuffs(BuffData[] buffs, float duration, string source)
    {
        StartCoroutine(BuffCo(buffs, duration, source));
    }

    private IEnumerator BuffCo(BuffData[] buffs, float duration, string source)
    {
        ActiveBuffs.Add(source);

        foreach (var buff in buffs)
            GetStatByType(buff.type).AddModifiers(buff.value, source);

        inventory.TriggerUpdateUI();

        yield return new WaitForSeconds(duration);

        foreach (var buff in buffs)
            GetStatByType(buff.type).RemoveModifiers(source);

        ActiveBuffs.Remove(source);

        inventory.TriggerUpdateUI();
    }
}
