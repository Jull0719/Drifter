using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthBarTrail;
    [SerializeField] private float healthTrailSpeed = 0.5f;
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

    private void OnDisable()
    {
        health.OnHealthChange -= UpdateHealthBarValue;
        entity.OnFlipped -= HandleHealthBarFlipped;
    }

    private void Start()
    {
        UpdateHealthBarValue();
    }

    // 更新血条血量
    public void UpdateHealthBarValue()
    {
        healthBar.value = health.GetHealthPercent();
        StartHealthBarTrail();
    }

    // 血条缓冲效果
    public void StartHealthBarTrail()
    {
        if (healthTrailCo != null)
            StopCoroutine(healthTrailCo);

        healthTrailCo = StartCoroutine(HealthBarTrailCo());
    }
    IEnumerator HealthBarTrailCo()
    {
        while (healthBarTrail.value > healthBar.value)
        {
            healthBarTrail.value -= healthTrailSpeed * Time.deltaTime;
            yield return null;
        }

        healthBarTrail.value = healthBar.value;
    }

    // 禁止血条随角色翻转
    private void HandleHealthBarFlipped() => transform.rotation = Quaternion.identity;
}
