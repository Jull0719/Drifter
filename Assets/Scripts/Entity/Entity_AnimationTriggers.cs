using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    protected Entity entity;
    protected Entity_Combat combat;
    protected Entity_SFX sfx;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        combat = GetComponentInParent<Entity_Combat>();
        sfx = GetComponentInParent<Entity_SFX>();
    }

    protected void PlayWalk() => sfx.PlayWalk();

    protected void CurrentAnimationTrigger() => entity.CurrentStateAnimationTrigger();

    protected void SetAttackTrigger() => combat.PerformAttack();
}
