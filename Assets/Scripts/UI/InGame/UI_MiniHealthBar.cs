using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniHealthBar : MonoBehaviour
{
    [Header("血条设置")]
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image healthBarTrail;
    [SerializeField] protected float healthTrailSpeed = 0.5f;
    private Coroutine healthTrailCo;

    private Entity entity;
    private Entity_Health health;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        health = GetComponentInParent<Entity_Health>();
    }

    private void OnEnable()
    {
        health.OnHealthChange += UpdateHealthBarValue;
        entity.OnFlipped += HandleHealthBarFlipped;
    }

    // 禁止血条随角色翻转
    private void HandleHealthBarFlipped() => transform.rotation = Quaternion.identity;

    // 更新血条血量
    public virtual void UpdateHealthBarValue()
    {
        if (healthBar == null && !healthBar.transform.parent.gameObject.activeSelf) return;

        healthBar.fillAmount = health.GetHealthPercent();

        // 只有HealthBar显示的时候，才有血量缓冲效果
        if (healthBar.transform.parent.gameObject.activeSelf)
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
