using UnityEngine;

[CreateAssetMenu(menuName = "Data/Stat", fileName = "Stat Setup - ")]
public class StatSetupDataSO : ScriptableObject
{
    [Header("Life")]
    public float maxHealth = 100; // 生命值上限
    public float healthRegen; // 再生速率

    [Header("Offense")]
    public float damage = 10; // 物理攻击
    public float critPower = 1; // 暴击倍率
    public float critChance; // 暴击概率

    [Header("Defense")]
    public float armor; // 护甲
    public float evasion; // 闪避概率

    [Header("Level")]
    public float strength; // 力量
    public float dexterity; // 敏捷
    public float intelligence; // 智力
    public float vitality; // 体力
}
