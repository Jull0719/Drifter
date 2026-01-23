using TMPro;
using UnityEngine;

public class UI_InGame : HealthBar_Base
{
    [Header("生命值")]
    [SerializeField] private TextMeshProUGUI healthBarText;

    [Header("金币")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private Player player;
    private Inventory_Shop shop;

    protected override void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<Player>();
        health = FindAnyObjectByType<Player>().GetComponent<Player_Health>();

        shop = FindAnyObjectByType<Inventory_Shop>();
        shop.OnUpdateUI += UpdateUI;

        UpdateUI();
    }

    public override void UpdateHealthBarValue()
    {
        base.UpdateHealthBarValue();

        healthBarText.text = Mathf.RoundToInt(health.GetCurrentHealth()) + "/" + player.stats.GetMaxHealth();
    }

    public void UpdateUI()
    {
        moneyText.text = player.inventory.money.ToString();
    }
}
