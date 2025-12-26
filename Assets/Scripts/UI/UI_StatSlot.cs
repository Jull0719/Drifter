using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private Player_Stats playerStats;

    private UI ui;
    private RectTransform rect;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();

        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + statType.ToString();
        statNameText.text = GetStatType(statType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect, statType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }

    // 更新数值
    public void UpdateStatSlot()
    {
        float statValue = GetValueByStatType(statType);
        statValueText.text = IsPercentageStat(statType) ? statValue * 100 + "%" : statValue.ToString();
    }

    // 获取数值
    public float GetValueByStatType(StatType statType)
    {
        return statType switch
        {
            // 等级
            StatType.Strength => playerStats.level.strength.GetValue(),
            StatType.Dexterity => playerStats.level.dexterity.GetValue(),
            StatType.Intelligence => playerStats.level.intelligence.GetValue(),
            StatType.Vitality => playerStats.level.vitality.GetValue(),

            // 生命
            StatType.MaxHealth => playerStats.GetMaxHealth(),
            StatType.HealthRegen => playerStats.GetRegenerateHealth(),

            // 攻击
            StatType.AttackSpeed => playerStats.GetAttackSpeed(),
            StatType.Damage => playerStats.GetBaseDamage(),
            StatType.CritChance => playerStats.GetCritChance(),
            StatType.CritPower => playerStats.GetCritPower(),
            StatType.ArmorReduction => playerStats.GetArmorReduction(),

            // 防御
            StatType.Armor => playerStats.GetBaseArmor(),
            StatType.Evasion => playerStats.GetEvasion(),

            _ => 0
        };
    }

    // 属性类型对应的名称
    private string GetStatType(StatType statType)
    {
        return statType switch
        {
            StatType.MaxHealth => "生命值上限",
            StatType.HealthRegen => "生命值再生速率",
            StatType.AttackSpeed => "攻击速度",
            StatType.Damage => "物理攻击",
            StatType.CritPower => "暴击倍率",
            StatType.CritChance => "暴击概率",
            StatType.ArmorReduction => "护甲穿透",
            StatType.Armor => "护甲",
            StatType.Evasion => "闪避概率",
            StatType.Strength => "力量",
            StatType.Dexterity => "敏捷",
            StatType.Intelligence => "智力",
            StatType.Vitality => "体力",
            _ => ""
        };
    }

    // 是否为百分比
    private bool IsPercentageStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.AttackSpeed:
            case StatType.CritPower:
            case StatType.CritChance:
            case StatType.ArmorReduction:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}

