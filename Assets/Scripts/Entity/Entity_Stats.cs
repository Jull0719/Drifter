using UnityEditor.PackageManager;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [SerializeField] protected StatSetupDataSO defaultStatDataSO;

    public StatGroup_Life life;
    public StatGroup_Offense offense;
    public StatGroup_Defense defense;
    public StatGroup_Level level;

    // 最大生命值
    public float GetMaxHealth()
    {
        float baseHealth = life.maxHealth.GetValue();
        float bonusHealth = level.vitality.GetValue() * 5; // 1点体力 -> 5点生命值上限
        float finalHealth = baseHealth + bonusHealth;
        return finalHealth;
    }

    // 再生生命值
    public float GetRegenerateHealth()
    {
        return life.healthRegen.GetValue();
    }

    // 物理攻击
    public float GetPhysicalDamage(out bool isCrit)
    {
        float basePhysicalDamage = offense.damage.GetValue();
        float bonusPhysicalDamage = level.strength.GetValue(); // 1点力量 -> 1点物理攻击
        float totalPhysicalDamage = basePhysicalDamage + bonusPhysicalDamage;

        isCrit = Random.Range(0, 100) < GetCritChance();

        float finalPhyscialDamage = isCrit ? totalPhysicalDamage * GetCritPower() : totalPhysicalDamage;
        return finalPhyscialDamage;
    }

    // 暴击倍率
    public float GetCritPower()
    {
        float baseCritPower = offense.critPower.GetValue(); // 默认为1
        float bonusCritPower = level.strength.GetValue() * 0.05f; // 1点力量 -> 0.05暴击倍率 （1.05倍基础攻击力）
        return baseCritPower + bonusCritPower;
    }

    // 暴击概率
    public float GetCritChance()
    {
        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = level.dexterity.GetValue() * 0.3f; // 1点敏捷 -> 0.3%暴击概率
        float totalCritChance = baseCritChance + bonusCritChance;

        // 限制暴击概率上限：60%
        float critChanceCap = 60;
        float finalCritChance = Mathf.Clamp(totalCritChance, 0, critChanceCap);

        return finalCritChance;
    }

    // 攻击速度
    public float GetAttackSpeed()
    {
        return offense.attackSpeed.GetValue();
    }

    // 闪避概率
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = level.dexterity.GetValue() * 0.5f; // 1点敏捷 -> 0.5%闪避概率
        float totalEvasion = baseEvasion + bonusEvasion;

        // 限制闪避概率上限：65%
        float evasionCap = 65;
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    // 护甲减伤
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = level.vitality.GetValue(); // 1点体力 -> 1点护甲
        float totalArmor = (baseArmor + bonusArmor);

        float reductionMultiplier = 1 - armorReduction; // 实际的护甲系数 = 1 - 护甲穿透率（减少的护甲）
        float effectiveArmor = totalArmor * reductionMultiplier;

        // mitigation -> 减伤比例
        float mitigation = effectiveArmor / (100 + effectiveArmor);

        // 限制减伤比例上限：65%
        float mitigationCap = 0.65f;
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    // 护甲穿透
    public float GetArmorReduction()
    {
        float armorReduction = offense.armorReduction.GetValue();
        float finalArmorReduction = Mathf.Clamp01(armorReduction);

        return finalArmorReduction;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return level.strength;
            case StatType.Dexterity: return level.dexterity;
            case StatType.Intelligence: return level.intelligence;
            case StatType.Vitality: return level.vitality;

            case StatType.MaxHealth: return life.maxHealth;
            case StatType.HealthRegen: return life.healthRegen;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritPower: return offense.critPower;
            case StatType.CritChance: return offense.critChance;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            default:
                Debug.LogWarning($"StatType {type} is not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Apply Default Stat Setup")]
    public void SetDefaultStatValue()
    {
        if (defaultStatDataSO == null)
        {
            Debug.Log("No default stat setup assigned.");
            return;
        }

        // Life
        life.maxHealth.SetBaseValue(defaultStatDataSO.maxHealth);
        life.healthRegen.SetBaseValue(defaultStatDataSO.healthRegen);

        // Offense
        offense.attackSpeed.SetBaseValue(defaultStatDataSO.attackSpeed);
        offense.damage.SetBaseValue(defaultStatDataSO.damage);
        offense.critChance.SetBaseValue(defaultStatDataSO.critChance);
        offense.critPower.SetBaseValue(defaultStatDataSO.critPower);
        offense.armorReduction.SetBaseValue(defaultStatDataSO.armorReduction);

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
