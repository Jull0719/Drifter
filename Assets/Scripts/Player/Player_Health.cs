using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditorInternal;
using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    // 事件
    public static event Action OnPlayerDeath;

    protected override void Awake()
    {
        base.Awake();

        player = entity as Player;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Die();
        }
    }

    // 死亡
    public override void OnDie()
    {
        base.OnDie();

        OnPlayerDeath?.Invoke();
        player.stateMachine.ChangeState(player.deadState);

        GameManager.instance.SetPlayerDeathPosition(transform.position);
        GameManager.instance.ReStart();
    }
}
