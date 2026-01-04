using System;

[Serializable]
public class StatGroup_Level
{
    public Stat strength; // 力量 -> 物理攻击、暴击倍率
    public Stat dexterity; // 敏捷 -> 闪避、暴击概率
    public Stat intelligence; // 智力 -> 魔法攻击、元素抗性
    public Stat vitality; // 体力 -> 生命值上限、防御
}
