using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public int HP;
    public Transform m_startPosition;
    public float m_Speed;//to be tweeked
    public const float m_StartWaitTime = 50f;//to be tweeked
    public float m_MaxDistance;// to be tweeked
    public Transform Player;
    public float m_MovementSmoothing;
    public float thrust; //to be tweeked
    public const int PLAYER_DAMAGE = 1;
    public BoxCollider2D solidBox;
    public BoxCollider2D triggerBox;

    private float m_WaitTime;
    private int m_facingRight;
    private int m_isAbove;
    private bool m_lastDirection = true;
    private bool m_AttackMode = false;
    private const float IFRAME_TIME = 0.05f;
    private float IFrameTime;
    private bool damaged;
    private bool calledOnce;
    private GoldPouch pouch;

    public Rigidbody2D m_RigidBody2D;

    void Awake()
    {
        damaged = false;
        IFrameTime = IFRAME_TIME;
        m_WaitTime = m_StartWaitTime;
        Player = GameObject.FindWithTag("Player").transform;
        pouch = (GoldPouch)gameObject.GetComponent<GoldPouch>();
    }

    void Start()
    {
        calledOnce = !state.m_IsDead;
        Debug.Log("Called Once:" + calledOnce);
        startPoint = gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!state.m_IsDead)
        {
            Hover();
            Attack();
            if(damaged)
            {
                if(IFrameTime > 0)
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
        EnemyActive();
    }

    void Hover()
    {
        if (!m_AttackMode)
        {
            if (m_WaitTime >= 0)
            {
                m_WaitTime -= Time.deltaTime;
            }
            else
            {
                m_WaitTime = m_StartWaitTime;
                Flip();
            }
        }
    }

    void EnemyActive()
    {
        if(calledOnce == state.m_IsDead)
        {
            calledOnce = !state.m_IsDead;
            if(!calledOnce)
            {
                pouch.Empty();
                m_RigidBody2D.velocity = Vector2.zero;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = calledOnce;
            solidBox.enabled = calledOnce;
            triggerBox.enabled = calledOnce;
        }
    }

    void Attack()
    {
        if (Vector2.Distance(transform.position, Player.position) <= m_MaxDistance)
        {
            m_AttackMode = true;
            facePlayer();
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position, Player.position, m_Speed * Time.deltaTime);
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    void facePlayer()
    {
        bool right = true;
        if (transform.position.x - Player.position.x > .25f)
        {
            m_facingRight = 1;
            right = true;
        }
        else if (transform.position.x - Player.position.x < .25f)
        {
            m_facingRight = -1;
            right = false;
        }
        else
        {
            m_facingRight = 0;
        }

        if (transform.position.y - Player.position.y > .25f)
        {
            m_isAbove = 1;
        }
        else if (transform.position.y - Player.position.y < .25f)
        {
            m_isAbove = -1;
        }
        else
        {
            m_isAbove = 0;
        }

        if (m_lastDirection != right)
        {
            m_lastDirection = right;
            Flip();
        }
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("TakingDamage");
        Vector2 direction = new Vector2(-m_facingRight,-m_isAbove);
        HP -= damage;
        if (HP > 0)
        {
            Debug.Log("ReduceHP");
            m_RigidBody2D.AddForce(direction * thrust, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Enemy Dead");
            state.m_IsDead = true;
        }
    }

    void OnEnterTrigger2D(Collider2D other)
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
