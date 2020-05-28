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

    void Attack()
    {
        Vector2 targetVelocity;
        Vector2 m_Velocity = Vector2.zero;
        if (Vector2.Distance(transform.position, Player.position) <= m_MaxDistance)
        {
            m_AttackMode = true;
            facePlayer();
            targetVelocity = new Vector2(-m_facingRight * m_Speed * Time.deltaTime, -m_isAbove * m_Speed * Time.deltaTime);
            m_RigidBody2D.velocity = Vector2.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
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
            m_RigidBody2D.AddForce(direction * thrust);
        }
        else
        {
            pouch.Empty();
            state.m_IsDead = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            solidBox.enabled = false;
            triggerBox.enabled = false;
            m_RigidBody2D.velocity = Vector2.zero;
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
