using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI statToolTipText;

    private Player_Stats playerStats;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    public void ShowToolTip(bool isShow, RectTransform taregt, StatType statType)
    {
        base.ShowToolTip(isShow, taregt);

        statToolTipText.text = GetStatTypeInfo(statType);
    }

    /// <summary>
    /// 根据属性类型返回对应的介绍
    /// </summary>
    /// <param name="statType">属性类型</param>
    /// <returns></returns>
    private string GetStatTypeInfo(StatType statType)
    {
        return statType switch
        {
            // Level Stats
            StatType.Strength => $"影响物理攻击和暴击倍率，1点力量提高{Settings.DamageBonus}点物理攻击、提高{Settings.CritPowerBonus}%暴击倍率。",
            StatType.Dexterity => $"影响闪避和暴击概率，1点敏捷提高{Settings.CritChanceBonus}%暴击概率、提高{Settings.EvasionBonus}%闪避概率。",
            StatType.Intelligence => $"影响魔法攻击和元素抗性",
            StatType.Vitality => $"影响生命值上限和防御，1点体力提高{Settings.HealthBonus}点生命值上限，提高{Settings.ArmorBonus}点防御",

            // Life Stats
            StatType.MaxHealth => "决定了你所拥有的生命值上限",
            StatType.HealthRegen => "每秒能够恢复的生命值",

            // Defense Stats
            StatType.AttackSpeed => "决定了攻击时的速度",
            StatType.Damage => "攻击所能造成的物理伤害",
            StatType.CritChance => $"暴击的概率，上限为{Settings.CritChanceCap}",
            StatType.CritPower => $"暴击的倍率",
            StatType.ArmorReduction => $"攻击时能够无视护甲的百分比",

            // Offense Stats
            StatType.Armor => $"减少所受物理伤害，减伤上限为{Settings.ArmorMitigationCap}%，当前减伤为：{playerStats.GetArmorMitigation(0) * 100}%",
            StatType.Evasion => $"完全躲避攻击的概率，上限为{Settings.EvasionCap}%",

            _ => ""
        };
    }
}
