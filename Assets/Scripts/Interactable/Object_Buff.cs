using System;
using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    [Header("Buff")]
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5;
    [SerializeField] private BuffData[] buffs;
    [Space]
    [Header("漂浮效果")]
    [SerializeField] private float floatSpeed = 2;
    [SerializeField] private float floatRange = 0.2f;
    private Vector3 startPosition;

    private Player_Stats stats;

    private void OnValidate()
    {
        gameObject.name = "增益 - " + buffName;
    }

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        FloatEffect();
    }

    // 漂浮效果
    private void FloatEffect()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stats = collision.GetComponent<Player_Stats>();

        if (stats.CanApplyBuff(buffName))
        {
            stats.ApplyBuffs(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }
}
