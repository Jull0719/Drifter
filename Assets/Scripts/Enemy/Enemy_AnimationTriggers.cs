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

    private void OpenCounterableWindow()
    {
        //enemy.vfx.EnabledAttackAlert(true);
        enemy.EnabledCounterableWindow(true);
    }
    private void CloseCounterableWindow()
    {
        //enemy.vfx.EnabledAttackAlert(false);
        enemy.EnabledCounterableWindow(false);
    }
}
