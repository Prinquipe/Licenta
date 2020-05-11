using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    public int HP;
    public float m_Speed = 50f;//to be tweeked
    public const float m_StartWaitTime = 50f;//to be tweeked
    public const float m_MinDistence = 0.2f;//to be tweeked
    public Transform m_startPosition;
    public Transform m_stopPosition;

    private BoxCollider2D box;
    private float m_WaitTime;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;

    private Transform m_targetPosition;
    private bool m_facingRight = true;
    // Start is called before the first frame update

    void Awake()
    {
        m_WaitTime = m_StartWaitTime;
        m_targetPosition = m_startPosition;
        if (m_Rigidbody2D == null)
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        box = (BoxCollider2D)GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!state.m_IsDead)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (transform.position.x < m_targetPosition.position.x)
        {
            Vector2.MoveTowards(transform.position, m_targetPosition.position, m_Speed * Time.deltaTime);
        }
        else if(transform.position.x > m_targetPosition.position.x)
        {
            Vector2.MoveTowards(transform.position, m_targetPosition.position, m_Speed * Time.deltaTime);
        }
        else if(Vector2.Distance(transform.position,m_targetPosition.position)< m_MinDistence) //needs tweeking
        {
            if(m_WaitTime <= 0)
            {
                SwitchTargetPosition();
                Flip();
                m_WaitTime = m_StartWaitTime;
            } else
            {
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void SwitchTargetPosition()
    {
        if(m_targetPosition == m_startPosition)
        {
            m_facingRight = true;
            m_targetPosition = m_stopPosition;
        }
        else
        {
            m_facingRight = false;
            m_targetPosition = m_startPosition;
        }
    }

   void Flip()
   {
        if(m_facingRight)
        {
            //scale one
        }
        else
        {
            //scale -1
        }
   }

   public override void TakeDamage()
   {
        if(HP > 0)
        {
            --HP;
        }
        else
        {
            state.m_IsDead = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            box.enabled = false;
        }
   }
}
