using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void SetAnimationTrigger() => entity.CurrentStateAnimationTrigger();

    private void SetAttackTrigger() => entity.PerformAttack();
}
