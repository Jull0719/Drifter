using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat combat;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void Start()
    {
        combat = entity.combat;
    }

    private void CurrentAnimationTrigger() => entity.CurrentStateAnimationTrigger();

    private void SetAttackTrigger()
    {
        combat.PerformAttack();
    }
}
