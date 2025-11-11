using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    public override void PerformAttack()
    {
        foreach (var target in TargetDetected())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable?.TakeDamage(damage, entity);
        }
    }
}
