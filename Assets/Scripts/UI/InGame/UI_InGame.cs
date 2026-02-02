using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [Header("血条设置")]
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image healthBarTrail;
    [SerializeField] protected float healthTrailSpeed = 0.5f;
    private Coroutine healthTrailCo;
    [SerializeField] private TextMeshProUGUI healthBarText;

    [Header("金币")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private Player player;
    private Inventory_Shop shop;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        player.health.OnHealthChange += UpdateHealthBarValue;

        shop = FindAnyObjectByType<Inventory_Shop>();
        shop.OnUpdateUI += UpdateUI;

        UpdateUI();
    }

    // 更新血条血量
    public void UpdateHealthBarValue()
    {
        healthBarText.text = Mathf.RoundToInt(player.health.GetCurrentHealth()) + "/" + player.stats.GetMaxHealth();
        healthBar.fillAmount = player.health.GetHealthPercent();
        StartHealthBarTrail();
    }

    public void UpdateUI()
    {
        moneyText.text = player.inventory.money.ToString();
    }

    // 血条缓冲效果
    public void StartHealthBarTrail()
    {
        if (healthTrailCo != null)
            StopCoroutine(healthTrailCo);

        healthTrailCo = StartCoroutine(HealthBarTrailCo());
    }

    protected IEnumerator HealthBarTrailCo()
    {
        while (healthBarTrail.fillAmount > healthBar.fillAmount)
        {
            healthBarTrail.fillAmount -= healthTrailSpeed * Time.deltaTime;
            yield return null;
        }

        healthBarTrail.fillAmount = healthBar.fillAmount;
    }
}
