using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Base : MonoBehaviour
{
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image healthBarTrail;
    [SerializeField] protected float healthTrailSpeed = 0.5f;
    private Coroutine healthTrailCo;

    protected Entity_Health health;

    protected virtual void Awake()
    {
        healthBar.fillAmount = 1;
        healthBarTrail.fillAmount = 1;
    }

    protected virtual void Start()
    {
        health.OnHealthChange += UpdateHealthBarValue;
    }

    // 更新血条血量
    public virtual void UpdateHealthBarValue()
    {
        healthBar.fillAmount = health.GetHealthPercent();
        StartHealthBarTrail();
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
