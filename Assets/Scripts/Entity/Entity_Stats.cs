using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [SerializeField] protected StatSetupDataSO defaultStatDataSO;

    public StatGroup_Life life;
    public StatGroup_Offense offense;
    public StatGroup_Defense defense;
    public StatGroup_Level level;

    // 最大生命值 1点体力 -> 5点生命值上限
    public float GetMaxHealth() => life.maxHealth.GetValue() + level.vitality.GetValue() * Settings.HealthBonus;
    // 再生生命值
    public float GetRegenerateHealth() => life.healthRegen.GetValue();

    // 基础攻击
    public float GetBaseDamage() => offense.damage.GetValue() + level.strength.GetValue() * Settings.DamageBonus;
    public float GetPhysicalDamage(out bool isCrit)
    {
        float basePhysicalDamage = GetBaseDamage();

        isCrit = Random.Range(0, 100) < GetCritChance() * 100f;

        float finalPhyscialDamage = isCrit ? basePhysicalDamage * GetCritPower() : basePhysicalDamage;
        return finalPhyscialDamage;
    }

    // 暴击倍率 1点力量 -> 0.05暴击倍率 （1.05倍基础攻击力） -> 1 + strength * Settings.CritPowerBonus
    public float GetCritPower() => offense.critPower.GetValue() + level.strength.GetValue() * Settings.CritPowerBonus;

    // 暴击概率 -> [critChance + dexterity * Settings.CritChanceBonus <= Settings.CritChanceCap]%
    public float GetCritChance()
    {
        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = level.dexterity.GetValue() * Settings.CritChanceBonus; // 1点敏捷 -> 0.5%暴击概率
        float totalCritChance = baseCritChance + bonusCritChance;

        // 限制暴击概率上限
        float critChanceCap = Settings.CritChanceCap;
        float finalCritChance = Mathf.Clamp(totalCritChance, 0, critChanceCap);

        return finalCritChance / 100f;
    }

    // 攻击速度 -> 1
    public float GetAttackSpeed() => offense.attackSpeed.GetValue();

    // 闪避概率 -> [evasion + dexterity * Settings.EvasionBonus <= Settings.EvasionCap]%
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = level.dexterity.GetValue() * Settings.EvasionBonus; // 1点敏捷 -> 0.5%闪避概率
        float totalEvasion = baseEvasion + bonusEvasion;

        // 限制闪避概率上限：65%
        float evasionCap = Settings.EvasionCap;
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion / 100f;
    }

    // 基础防御
    public float GetBaseArmor() => defense.armor.GetValue() + level.vitality.GetValue() * Settings.ArmorBonus;

    // 护甲减伤 -> [armorMitigation <= Settings.ArmorMitigationCap]%
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = GetBaseArmor();

        float reductionMultiplier = 1 - armorReduction; // 实际的护甲系数 = 1 - 护甲穿透率（减少的护甲）
        float effectiveArmor = baseArmor * reductionMultiplier;

        // mitigation -> 减伤比例
        float mitigation = effectiveArmor / (100 + effectiveArmor);

        // 限制减伤比例上限
        float mitigationCap = Settings.ArmorMitigationCap;
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    // 护甲穿透 -> armorReduction <= 1
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

    [ContextMenu("设置默认数值")]
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
