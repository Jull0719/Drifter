using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }

    private void OpenCounterableWindow() => enemy.canBeStunned = true;
    private void CloseCounterableWindow() => enemy.canBeStunned = false;
}
