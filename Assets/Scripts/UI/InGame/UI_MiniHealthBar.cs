using UnityEngine;

public class UI_MiniHealthBar : HealthBar_Base
{
    private Entity entity;

    protected override void Awake()
    {
        base.Awake();
        entity = GetComponentInParent<Entity>();
        health = GetComponentInParent<Entity_Health>();
    }

    private void OnEnable()
    {
        entity.OnFlipped += HandleHealthBarFlipped;
    }

    private void OnDisable()
    {
        entity.OnFlipped -= HandleHealthBarFlipped;
    }

    // 禁止血条随角色翻转
    private void HandleHealthBarFlipped() => transform.rotation = Quaternion.identity;
}
