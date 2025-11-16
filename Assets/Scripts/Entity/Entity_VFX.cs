using System;
using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    [Header("击中")]
    [SerializeField] protected GameObject onHitVfxPrefab;
    [SerializeField] protected GameObject onHeavyHitVfxPrefab;
    [SerializeField] protected Color hitVfxColor = Color.white;

    [Header("受击")]
    [SerializeField] protected Material damageVfxMaterial;
    [SerializeField] protected float damageVfxDuration = 0.3f;
    private Material defaultMaterial;
    protected Coroutine damageVfxCo;

    [Header("消隐")]
    protected Coroutine fadeCo;
    protected SpriteRenderer sr;

    private Entity entity;

    protected void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = sr.material;

        entity = GetComponent<Entity>();
    }

    /// <summary>
    /// 击中效果
    /// </summary>
    /// <param name="target">击中目标</param>
    /// <param name="damage">击中伤害</param>
    public void CreateOnHitVfx(Transform target, bool isHeavyHit, float damage)
    {
        GameObject hitVfxPrefab = isHeavyHit ? onHeavyHitVfxPrefab : onHitVfxPrefab;
        hitVfxPrefab.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor; // 击中效果的颜色

        float yRotation = entity.facingDir == 1 ? 0 : 180; // 调整朝向，和发出攻击对象朝向保持一致
        Instantiate(hitVfxPrefab, target.position, Quaternion.Euler(0, yRotation, 0));
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
