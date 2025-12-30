using TMPro;
using UnityEngine;

public class UI_InGame : HealthBar_Base
{
    [SerializeField] private TextMeshProUGUI healthBarText;

    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<Player>();
        health = FindAnyObjectByType<Player>().GetComponent<Player_Health>();
    }

    public override void UpdateHealthBarValue()
    {
        base.UpdateHealthBarValue();

        healthBarText.text = Mathf.RoundToInt(health.GetCurrentHealth()) + "/" + player.stats.GetMaxHealth();
    }
}
