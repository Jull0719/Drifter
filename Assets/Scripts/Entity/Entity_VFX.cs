using System;
using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    [Header("Damage VFX")]
    [SerializeField] protected Material damageVfxMaterial;
    [SerializeField] protected float damageVfxDuration = 0.3f;
    private Material defaultMaterial;
    protected Coroutine damageVfxCo;

    [Header("Fade VFX")]
    protected Coroutine fadeCo;
    protected SpriteRenderer sr;

    protected void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = sr.material;
    }

    /// <summary>
    /// 受击效果
    /// </summary>
    public void OnDamageVfx()
    {
        if (damageVfxCo != null)
            StopCoroutine(damageVfxCo);

        damageVfxCo = StartCoroutine(OnDamageVfxCo());
    }

    IEnumerator OnDamageVfxCo()
    {
        sr.material = damageVfxMaterial;
        yield return new WaitForSeconds(damageVfxDuration);
        sr.material = defaultMaterial;
    }

    /// <summary>
    /// 透明效果
    /// </summary>
    /// <param name="fadeDuration">持续时间</param>
    /// <param name="targetAlpha">目标Alpha值，0是透明，1是完全不透明</param>
    public void Fade(float fadeDuration, float targetAlpha)
    {
        if (fadeCo != null)
            StopCoroutine(fadeCo);

        fadeCo = StartCoroutine(FadeCo(fadeDuration, targetAlpha));
    }

    IEnumerator FadeCo(float duration, float targetAlpha)
    {
        float target = targetAlpha;
        Color currentColor = sr.color;

        float speed = Mathf.Abs(targetAlpha - currentColor.a) / duration;
        while (!Mathf.Approximately(currentColor.a, targetAlpha))
        {
            currentColor.a = Mathf.MoveTowards(currentColor.a, targetAlpha, speed * Time.deltaTime);
            sr.color = currentColor;
            yield return null;
        }
        sr.color = currentColor;
    }
}
