using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    public int HP;
    public float m_Speed;//to be tweeked
    public const float WAITTIME = 2f;//to be tweeked
    public float m_MaxDistance;// to be tweeked
    public float m_MinDistance;
    public const int PLAYER_DAMAGE = 1;
    public BoxCollider2D solidBox;
    public BoxCollider2D triggerBox;
    public Transform startPosition;
    public Transform endPosition;

    private Animator animator;
    private Transform currentGoal;
    private bool reachedGoal;
    private float m_WaitTime;
    private int m_facingRight;
    private int m_isAbove;
    private const float IFRAME_TIME = 0.05f;
    private float IFrameTime;
    private bool damaged;
    private bool calledOnce;
    private GoldPouch pouch;
    private bool pouchEmpty;
    private Rigidbody2D m_RigidBody2D;

    void Awake()
    {
        startPosition.SetParent(null, true);
        endPosition.SetParent(null, true);
        currentGoal = endPosition;
        reachedGoal = false;
        damaged = false;
        IFrameTime = IFRAME_TIME;
        m_WaitTime = WAITTIME;
        pouch = (GoldPouch)gameObject.GetComponent<GoldPouch>();
        m_RigidBody2D = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
        animator = (Animator)gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        calledOnce = state.m_IsDead;
        pouchEmpty = true;
        Debug.Log("calledOnce:" + calledOnce + " ID:" + EnemyID);
        startPoint = gameObject.transform.position;
        animator.SetBool("isWalking", true);
    }
    // Update is called once per frame
    void Update()
    {
        EnemyActive();
        if (!state.m_IsDead)
        {
            Patrol();
            Wait();
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
    }

    void Wait()
    {
        if (reachedGoal)
        {
            animator.SetBool("isWalking", !reachedGoal);
            if (m_WaitTime >= 0)
            {
                m_WaitTime -= Time.deltaTime;
            }
            else
            {
                m_WaitTime = WAITTIME;
                reachedGoal = false;
                animator.SetBool("isWalking", !reachedGoal);
                Flip();
            }
            
        }
    }

    void EnemyActive()
    {
        if (calledOnce == state.m_IsDead)
        {
            calledOnce = !state.m_IsDead;
            Debug.Log("EnemyActive calledOnce:" + calledOnce + " ID:" + EnemyID);
            if (!pouchEmpty)
            {
                pouch.Empty();
                pouchEmpty = true;
                m_RigidBody2D.velocity = Vector2.zero;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = calledOnce;
            solidBox.enabled = calledOnce;
            triggerBox.enabled = calledOnce;
        }
    }

    void Patrol()
    {
        if (Mathf.Abs(transform.position.x - currentGoal.position.x) <= m_MinDistance)
        {
            reachedGoal = true;

            if (currentGoal == startPosition)
            {
                currentGoal = endPosition;
            }
            else
            {
                currentGoal = startPosition;
            }
        }
        else if(!reachedGoal)
        {
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position, currentGoal.position, m_Speed * Time.deltaTime);
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
