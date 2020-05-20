using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public int HP;
    public Transform m_startPosition;
    public float m_Speed = 50f;//to be tweeked
    public const float m_StartWaitTime = 50f;//to be tweeked
    public float m_MaxDistance;// to be tweeked
    public Transform Player;
    public float m_MovementSmoothing;
    public float thrust; //to be tweeked
    public const int PLAYER_DAMAGE = 1;

    private BoxCollider2D box;
    private float m_WaitTime;
    private int m_facingRight;
    private int m_isAbove;
    private bool m_lastDirection = true;
    private bool m_AttackMode = false;

    public Rigidbody2D m_RigidBody2D;

    void Awake()
    {
        m_WaitTime = m_StartWaitTime;
        Player = GameObject.FindWithTag("Player").transform;

        box = (BoxCollider2D)GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.m_IsDead)
        {
            Hover();
            Attack();
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

    public override void TakeDamage()
    {
        Vector2 direction = new Vector2(-m_facingRight,-m_isAbove);
        if (HP > 0)
        {
            --HP;
            m_RigidBody2D.AddForce(direction * thrust);
        }
        else
        {
            state.m_IsDead = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            box.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player;
        PlayerInteraction inter;
        GameObject collideObject;
        Debug.Log("Attacked");
        if (other.CompareTag("PlayerAttack"))
        {
            collideObject = other.gameObject;
            inter = (PlayerInteraction)collideObject.GetComponent<PlayerInteraction>();
            player = inter.GetPlayerMovement();
            player.TakeDamage(PLAYER_DAMAGE);
        }
    }
}
