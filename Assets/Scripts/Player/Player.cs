using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [Header("移动")]
    public float moveSpeed = 10;
    public float airMultiplier = 0.8f;
    public Vector2 moveInput { get; private set; }

    [Header("跳跃")]
    public float jumpForce = 12;
    public bool canDoubleJump = true;

    [Header("攻击")]
    public float comboLimitedTime = 4; // 组合攻击限制时间
    public float attackVelocityDuration = 0.2f; // 攻击反馈持续时间
    public Vector2[] attackVelocity; //攻击反馈
    private Coroutine queueAttackCo;

    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_AttackState attackState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }

    public Player_Health health { get; private set; }
    public Player_Combat combat { get; private set; }
    public Entity_VFX vfx { get; private set; }

    private Scene m_scene;
    private GameObject[] dialogs;

    [HideInInspector] public int Health;

    public AudioClip[] audios;

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        vfx = GetComponent<Entity_VFX>();
        health = GetComponent<Player_Health>();
        combat = GetComponent<Player_Combat>();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jump");
        fallState = new Player_FallState(this, stateMachine, "jump");
        attackState = new Player_AttackState(this, stateMachine, "attack");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");

        deadState = new Player_DeadState(this, stateMachine, "dead");

        dialogs = GameObject.FindGameObjectsWithTag("dialog");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitializedState(idleState);

        Health = 100;
        m_scene = SceneManager.GetActiveScene();
    }

    protected override void Update()
    {
        base.Update();
        /*
        //Death
        if (Health == 0)
        {
            anim.SetTrigger("Death");
            Destroy(gameObject, 2f);
        }

        //控制攻击组的计时器
        m_timeSinceAttack += Time.deltaTime;

        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.5f && !ShowDailog() && m_scene.name != "Country")
        {
            m_currentAttack++;

            // 两种攻击之前循环切换
            if (m_currentAttack > 2)
                m_currentAttack = 1;

            //设置第一种攻击到第二种攻击的间隔时间
            if (m_timeSinceAttack > 1.5f)
                m_currentAttack = 1;

            if (m_currentAttack == 1)
            {
                //播放普攻音效
                this.GetComponent<AudioSource>().clip = audios[0];
                this.GetComponent<AudioSource>().Play();
            }
            else if (m_currentAttack == 2)
            {
                //播放重击音效
                this.GetComponent<AudioSource>().clip = audios[1];
                this.GetComponent<AudioSource>().Play();
            }
            anim.SetTrigger("Attack" + m_currentAttack);
            m_timeSinceAttack = 0.0f;                           //重置攻击冷却时间
        }

        // Jump
        else if (Input.GetKeyDown("space"))
        {
            if (groundSensor.grounded)
            {
                m_currentJump = 2;
                anim.SetTrigger("Jump");
                SetVelocity(rb.velocity.x, jumpForce);
            }
            else if (m_currentJump == 1 && doubleJump)
            {
                anim.SetTrigger("Jump");
                SetVelocity(rb.velocity.x, jumpForce);
            }
            m_currentJump--;
        }
        */
    }

    bool ShowDailog()
    {
        bool temp = false;
        foreach (var dialog in dialogs)
        {
            if (dialog.activeSelf == true)
            {
                temp = true;
            }
        }
        return temp;
    }

    // 再次进入攻击状态
    public void EnterComboAttack()
    {
        if (queueAttackCo != null)
            StopCoroutine(queueAttackCo);

        queueAttackCo = StartCoroutine(QueueAttackCo());
    }

    IEnumerator QueueAttackCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(attackState);
    }

    // 二段跳
    public void EnabledDoubleJump(bool enable) => canDoubleJump = enable;
}
