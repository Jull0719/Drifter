using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [SerializeField] protected StatSetupDataSO defaultStatDataSO;

    [SerializeField] private StatGroup_Life life;
    [SerializeField] private StatGroup_Offense offense;
    [SerializeField] private StatGroup_Defense defense;
    [SerializeField] private StatGroup_Level level;

    // 获取最大生命值
    public float GetMaxHealth()
    {
        float baseHealth = life.maxHealth.GetBaseValue();
        float bonusHealth = level.vitality.GetBaseValue() * 5; // 1点体力 -> 5点生命值上限
        float finalHealth = baseHealth + bonusHealth;
        return finalHealth;
    }

    // 获取物理攻击
    public float GetPhysicalDamage(out bool isCrit)
    {
        float basePhysicalDamage = offense.damage.GetBaseValue();
        float bonusPhysicalDamage = level.strength.GetBaseValue(); // 1点体力 -> 1点物理攻击
        float totalPhysicalDamage = basePhysicalDamage + bonusPhysicalDamage;

        isCrit = Random.Range(0, 100) < GetCritChance();

        float finalPhyscialDamage = isCrit ? totalPhysicalDamage * GetCritPower() : totalPhysicalDamage;
        return finalPhyscialDamage;
    }

    // 获取暴击倍率
    public float GetCritPower()
    {
        float baseCritPower = offense.critPower.GetBaseValue(); // 默认为1
        float bonusCritPower = level.strength.GetBaseValue() * 0.05f; // 1点力量 -> 0.05暴击倍率 （1.05倍基础攻击力）
        return baseCritPower + bonusCritPower;
    }

    // 获取暴击概率
    public float GetCritChance()
    {
        float baseCritChance = offense.critChance.GetBaseValue();
        float bonusCritChance = level.dexterity.GetBaseValue() * 0.3f; // 1点敏捷 -> 0.3%暴击概率
        float totalCritChance = baseCritChance + bonusCritChance;

        // 限制暴击概率上限：60%
        float critChanceCap = 60;
        float finalCritChance = Mathf.Clamp(totalCritChance, 0, critChanceCap);

        return finalCritChance;
    }


    // 获取闪避概率
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetBaseValue();
        float bonusEvasion = level.dexterity.GetBaseValue() * 0.5f; // 1点敏捷 -> 0.5%闪避概率
        float totalEvasion = baseEvasion + bonusEvasion;

        // 限制闪避概率上限：65%
        float evasionCap = 65;
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    [ContextMenu("Apply Default Stat Setup")]
    public void SetStatValue()
    {
        if (defaultStatDataSO == null) return;

        // Life
        life.maxHealth.SetBaseValue(defaultStatDataSO.maxHealth);
        life.healthRegen.SetBaseValue(defaultStatDataSO.healthRegen);

        // Offense
        offense.damage.SetBaseValue(defaultStatDataSO.damage);
        offense.critChance.SetBaseValue(defaultStatDataSO.critChance);
        offense.critPower.SetBaseValue(defaultStatDataSO.critPower);

        // Defense
        defense.armor.SetBaseValue(defaultStatDataSO.armor);
        defense.evasion.SetBaseValue(defaultStatDataSO.evasion);

        // Level
        level.strength.SetBaseValue(defaultStatDataSO.strength);
        level.dexterity.SetBaseValue(defaultStatDataSO.dexterity);
        level.intelligence.SetBaseValue(defaultStatDataSO.intelligence);
        level.vitality.SetBaseValue(defaultStatDataSO.vitality);
    }
}
