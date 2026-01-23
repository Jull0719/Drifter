using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamagable
{
    [SerializeField] private Vector2 knockback = new Vector2(0, 3);
    [SerializeField] private float duration = 1f;
    private int attackCount;
    private bool canAttack = true;
    private bool canDrop = true;

    private Animator anim;
    private Rigidbody2D rb;
    private Entity_VFX vfx;
    private Entity_DropManager dropManager;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        vfx = GetComponent<Entity_VFX>();
        dropManager = GetComponent<Entity_DropManager>();
    }

    public bool TakeDamage(float damage, Transform damageDealer)
    {
        if (!canAttack) return false;

        if (canDrop)
        {
            dropManager.DropItems();
            canDrop = false;
        }

        vfx.OnDamageVfx();
        rb.velocity = knockback;
        rb.angularVelocity = Random.Range(-200, 200);

        attackCount++;
        TryChangeState(attackCount);

        return true;
    }

    private void TryChangeState(int attackCount)
    {
        switch (attackCount)
        {
            case 1:
                anim.SetBool("isOpen", true);
                break;

            case 2:
                anim.SetBool("isDamage", true);
                break;
            case 3:
                canAttack = false;
                Destroy(gameObject, duration);
                vfx.Fade(duration, 0);
                break;
        }
    }
}
