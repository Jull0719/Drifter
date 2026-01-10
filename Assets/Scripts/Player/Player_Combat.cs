using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [SerializeField] private float counterRecoveryTime = 0.1f;
    public bool PerformCounterAttack()
    {
        bool hasPerformedCounter = false;
        foreach (var target in TargetDetected())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null) continue;

            if (counterable.CanBeCountered)
            {
                hasPerformedCounter = true;
                counterable.HandleCountered();
            }
        }

        return hasPerformedCounter;
    }

    // 获取从counterAttack恢复到idle的时间
    public float GetCounterRecoveryTime() => counterRecoveryTime;
}
