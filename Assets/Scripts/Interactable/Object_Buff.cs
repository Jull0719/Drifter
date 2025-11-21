using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    [Header("Buff")]
    [SerializeField] private bool canBeUsed = true;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5;
    [SerializeField] private Buff[] buffs;
    [Space]
    [Header("漂浮效果")]
    [SerializeField] private float floatSpeed = 2;
    [SerializeField] private float floatRange = 0.2f;
    private Vector3 startPosition;

    private SpriteRenderer sr;

    private Entity_Stats stats;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        startPosition = transform.position;
    }

    private void Update()
    {
        FloatEffect();
    }

    private void FloatEffect()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeUsed == false) return;

        stats = collision.GetComponent<Entity_Stats>();

        StartCoroutine(BuffCo());
    }

    IEnumerator BuffCo()
    {
        canBeUsed = false;

        foreach (var buff in buffs)
            stats.GetStatByType(buff.type).AddModifiers(buff.value, buffName);

        sr.color = Color.clear;

        yield return new WaitForSeconds(buffDuration);

        foreach (var buff in buffs)
            stats.GetStatByType(buff.type).RemoveModifiers(buffName);

        Destroy(gameObject);
    }
}
