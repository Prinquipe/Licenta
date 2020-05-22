using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : Enemy
{
    public int HP;
    public float m_Speed = 50f;//to be tweeked
    public const float m_StartWaitTime = 50f;//to be tweeked
    public const float m_MaxDistance = 50f;// to be tweeked
    public const float m_MinDistance = 0.2f;//to be tweeked
    public const float m_AttackDistance = 5f;//to be tweeked
    public const float m_DetectDistance = 20f;//to be tweeked
    public Transform Player;
    public bool m_isGuard = false;
    public Transform m_startPosition;
    public Transform m_stopPosition;

    private BoxCollider2D box;
    private float m_WaitTime;
    public bool m_facingRight = true;
    private bool m_AttackMode = false;
    private int m_AttackType;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private bool m_isAttacking;
    [SerializeField] private const int m_MaxAttacks = 3;
    
    private Transform m_currentPosition;
    [SerializeField] private Transform m_Player;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_Rigidbody2D = null)
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        m_Player = GameObject.FindWithTag("Player").transform;
        m_currentPosition = m_startPosition;
        m_WaitTime = m_StartWaitTime;
        box = (BoxCollider2D)GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        Attack();
        Flip();
    }

    void Patrol()
    {
        if(!m_AttackMode)
        {
            if(!m_isGuard)
            {
                if (Vector2.Distance(transform.position, m_currentPosition.position) > m_MinDistance)
                {
                    Vector2.MoveTowards(transform.position, m_currentPosition.position, m_Speed * Time.deltaTime);
                }
                else
                {
                    if (m_WaitTime <= 0)
                    {
                        changePosition();
                        face();
                    }
                    else
                    {
                        m_WaitTime -= Time.deltaTime;
                    }
                }
            }
            else
            {
               //play Idle animation
            }
        }
    }

    void Attack()
    {
        if (Vector2.Distance(transform.position, m_Player.position) <= m_DetectDistance)
        {
            m_AttackMode = true;
            facePlayer();
            if (Vector2.Distance(transform.position, m_Player.position) > m_AttackDistance)
            {
                if (transform.position.x > m_startPosition.position.x && transform.position.x < m_startPosition.position.x)
                {
                    Vector2.MoveTowards(transform.position, Player.position, m_Speed * Time.deltaTime);
                }
            }
            else
            {
                m_AttackType = Random.Range(0,m_MaxAttacks);//choose what attack to do
                AttackPlayer(m_AttackType);
            }
        }
    }

    void AttackPlayer(int attackType)
    {
        //to be used as reference
        //play telegraph attackframes
        //playerAttackAnim.SetTrigger("Attack"); 
        //playerAttackAnim.SetInteger("randomAttack", randomAttack);
        //make a collider to indicate damage
    }

    void changePosition()
    {
        if(m_currentPosition.position == m_startPosition.position)
        {
            m_currentPosition.position = m_stopPosition.position;
        }
        else
        {
            m_currentPosition.position = m_stopPosition.position;
        }
    }

    void face()
    {
        if (transform.position.x < m_currentPosition.position.x)
        {
            m_facingRight = true;
        }
        else
        {
            m_facingRight = false;
        }
    }

    void facePlayer()
    {
        if (transform.position.x < Player.position.x)
        {
            m_facingRight = true;
        }
        else
        {
            m_facingRight = false;
        }
    }

    void Flip()
    {
        if(m_facingRight)
        {
            //face right
        }
        else
        {
            //face left
        }
    }

    public override void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP > 0)
        {
        }
        else
        {
            state.m_IsDead = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            box.enabled = false;
        }
    }
}
