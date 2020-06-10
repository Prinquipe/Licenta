using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy: Enemy
{
    public enum AttackEnemyState
    {
        ENEMY_WAIT,
        ENEMY_CLOSEDISTANCE,
        ENEMY_ATTACK,
        ENEMY_RECOVER
    }

    public int HP;
    public float m_Speed;//to be tweeked
    public float m_MaxDistance;// to be tweeked
    public float m_MinDistance;
    public const float RECOVERYTIMER = 0.5f;
    public const float ATTACKTIMER = 1f;
    public float BeginAttackFrames;
    public float EndAttackFrames;

    public const int PLAYER_DAMAGE = 1;
    public BoxCollider2D attackBox;
    public BoxCollider2D solidBox;
    public BoxCollider2D triggerBox;
    public GameObject Player;

    private AttackEnemyState enemyState;
    private AttackEnemyState nextEnemyState;
    private float AttackTimer;
    private float RecoveryTime;
    private Animator animator;
    private Transform target;
    private bool m_facingRight;
    private const float IFRAME_TIME = 0.05f;
    private float IFrameTime;
    private bool damaged;
    private bool calledOnce;
    private GoldPouch pouch;
    private bool pouchEmpty;
    private Rigidbody2D m_RigidBody2D;

    void Awake()
    {
        attackBox.enabled = false;
        AttackTimer = ATTACKTIMER;
        RecoveryTime = RECOVERYTIMER;
        enemyState = AttackEnemyState.ENEMY_WAIT;
        nextEnemyState = AttackEnemyState.ENEMY_WAIT;
        damaged = false;
        IFrameTime = IFRAME_TIME;
        target = Player.transform;
        pouch = (GoldPouch)gameObject.GetComponent<GoldPouch>();
        m_RigidBody2D = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
        animator = (Animator)gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        calledOnce = state.m_IsDead;
        pouchEmpty = true;
        Debug.Log("Called Once:" + calledOnce);
        startPoint = gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!state.m_IsDead)
        {
            switch(enemyState)
            {
                case AttackEnemyState.ENEMY_WAIT: Wait();
                    break;
                case AttackEnemyState.ENEMY_CLOSEDISTANCE: CloseDistance();
                    break;
                case AttackEnemyState.ENEMY_ATTACK: Attack();
                    break;
                case AttackEnemyState.ENEMY_RECOVER: Recover();
                    break;
            }

            enemyState = nextEnemyState;
            if (damaged)
            {
                if (IFrameTime > 0)
                {
                    IFrameTime -= Time.deltaTime;
                }
                else
                {
                    IFrameTime = IFRAME_TIME;
                    damaged = false;
                }
            }
        }
        target = Player.transform;
        EnemyActive();
    }

    void Wait()
    {
        if (Vector2.Distance(gameObject.transform.position,target.position) <= m_MaxDistance)
        {
            animator.SetBool("isMoving", true);
            nextEnemyState = AttackEnemyState.ENEMY_CLOSEDISTANCE;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void CloseDistance()
    {
        if(Vector2.Distance(gameObject.transform.position, target.position) > m_MinDistance && Vector2.Distance(gameObject.transform.position, target.position) <= m_MaxDistance)
        {
            FacePlayer();
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position, new Vector2(target.position.x, m_RigidBody2D.position.y), m_Speed * Time.deltaTime);
        }
        else if(Vector2.Distance(gameObject.transform.position, target.position) <= m_MinDistance)
        {
            nextEnemyState = AttackEnemyState.ENEMY_ATTACK;
        }
        else if(Vector2.Distance(gameObject.transform.position, target.position) > m_MaxDistance)
        {
            nextEnemyState = AttackEnemyState.ENEMY_WAIT;
        }
    }

    void Attack()
    {
        animator.SetBool("isAttacking", true);
        AttackTimer -= Time.deltaTime;
        if(AttackTimer <= BeginAttackFrames && AttackTimer > EndAttackFrames)
        {
            attackBox.enabled = true;
        }
        else if(AttackTimer <= EndAttackFrames && AttackTimer > 0)
        {
            attackBox.enabled = false;
        }
        else if(AttackTimer <= 0)
        {
            AttackTimer = ATTACKTIMER;
            animator.SetBool("isAttacking", false);
            nextEnemyState = AttackEnemyState.ENEMY_RECOVER;
        }
    }

    void Recover()
    {
        RecoveryTime -= Time.deltaTime;
        animator.SetBool("isRecovering", true);
        if(RecoveryTime <= 0)
        {
            RecoveryTime = RECOVERYTIMER;
            animator.SetBool("isRecovering", false);
            nextEnemyState = AttackEnemyState.ENEMY_WAIT;
        }
    }

    void EnemyActive()
    {
        if (calledOnce == state.m_IsDead)
        {
            calledOnce = !state.m_IsDead;
            if (!pouchEmpty)
            {
                pouchEmpty = true;
                pouch.Empty();
                m_RigidBody2D.velocity = Vector2.zero;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = calledOnce;
            solidBox.enabled = calledOnce;
            triggerBox.enabled = calledOnce;
        }
    }

    void FacePlayer()
    {
        if(target.position.x - transform.position.x > 0)
        {
            if (!m_facingRight)
            {
                Flip();
            }
            m_facingRight = true;
            
        }
        else
        {
            if (m_facingRight)
            {
                Flip();
            }
            m_facingRight = false;
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public override void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            state.m_IsDead = true;
            pouchEmpty = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("PlayerDamage"))
        {
            PlayerInteraction inter;
            inter = (PlayerInteraction)other.gameObject.GetComponent<PlayerInteraction>();
            inter.TakeDamage(PLAYER_DAMAGE);
        }
        else if (other.CompareTag("PlayerAttack"))
        {
            if (!damaged)
            {
                damaged = true;
                TakeDamage(PlayerAttack.AttackDamage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            PlayerInteraction inter;
            inter = (PlayerInteraction)other.gameObject.GetComponent<PlayerInteraction>();
            inter.TakeDamage(PLAYER_DAMAGE);
        }
        else if (other.CompareTag("PlayerAttack"))
        {
            if (!damaged)
            {
                damaged = true;
                TakeDamage(PlayerAttack.AttackDamage);
            }
        }
    }
}
