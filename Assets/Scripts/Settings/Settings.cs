public class Settings
{
    // ----- Stat System -----
    public const float HealthBonus = 5; // 生命值补偿
    public const float DamageBonus = 1; // 攻击补偿
    public const float ArmorBonus = 1; // 防御补偿

    public const float CritPowerBonus = 0.05f; // 暴击倍率补偿
    public const float CritChanceBonus = 0.5f; // 暴击概率补偿
    public const float EvasionBonus = 0.5f; // 闪避概率补偿

    public const float CritChanceCap = 65; // 暴击概率上限
    public const float ArmorMitigationCap = 70; // 减伤护甲上限
    public const float EvasionCap = 65; // 闪避率上限

    // ----- UI -----
    public const string ShowMiniHealthBarParameter = "showHealthBar"; // 记录Toggle的值以控制血条显示或隐藏

    // ----- Audio System -----
    public const float DefaultVolume = 0.6f;

    public const string ButtonSFXName = "UI_Click";
}
