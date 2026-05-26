using UnityEngine;

public class Enemy_Health : Entity_Health, IDamagable
{
    [Header("死亡")]
    [SerializeField] protected float deadTime = 3;

    private Enemy enemy;
    private StateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        enemy = entity as Enemy;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine = enemy.stateMachine;
    }

    // 获取敌人死亡时间
    public float GetEnemyDeadTime() => deadTime;

    public override void OnDie()
    {
        base.OnDie();
        stateMachine.ChangeState(enemy.deadState);
        Destroy(gameObject, deadTime);
    }

    // 受击
    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        if (base.TakeDamage(damage, damageDealer) == false)
            return false;

        //Debug.Log("Enemy - " + gameObject.name + "受到攻击");
        if (damageDealer.GetComponent<Player>() != null)
            enemy.TryToEnterBattleState(damageDealer);

        return true;
    }
}
