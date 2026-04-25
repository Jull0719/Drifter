using System;

public class Player_Health : Entity_Health
{
    private UI_MiniHealthBar healthBar;
    private Player player;

    // 事件
    public static event Action OnPlayerDeath;

    protected override void Awake()
    {
        base.Awake();

        player = entity as Player;
    }

<<<<<<< HEAD
    protected override void Start()
    {
        base.Start();
        healthBar = GetComponentInChildren<UI_MiniHealthBar>(true);
=======
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Die();
        }
>>>>>>> feature/场景管理
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

    internal void EnabledMiniHealthBar(bool enabled)
    {
        healthBar.gameObject.SetActive(enabled);
    }
}
