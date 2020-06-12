using System;
using System.Collections;
using UnityEngine;

public class Wizard : Enemy
{
    public enum WizardState
    {
        STATE_CALM,
        STATE_ANGRY
    }

    public enum WizardAttackState
    {
        STATE_RECOVERY,
        STATE_FIREBALL,
        STATE_FLY,
        STATE_GROUNDPOUND
    }

    public int HP;
    public float movSpeed;//to be tweeked
    public float fallSpeed;
    public float Xoffset;
    public float Yoffset;
    public float height;
    public Transform Player;
    public BoxCollider2D solidBox;
    public BoxCollider2D triggerBox;
    public GameObject fireBall;
    public GameObject sandWall;
    public Transform middlePoint;
    public Transform enterPoint;
    public Transform exitPoint;
    public const int MaxHP = 50;
    public const int PLAYER_DAMAGE = 1;
    public const float FIREBALLTIMER = 0.5f;
    public const float SHOOTCOOLDOWN = 0.08f;
    public const float FLYTIMER = 1f;
    public const float GROUNDPOUNDTIMER = 0.83f;
    public const float CLIMBTIME = 0.58f;
    public const float FALLTIME = 0.415f;
    public const float IMPACTMOMENT = 0.16f;
    public const float STARTSHOOTING = 0.34f;
    public const float FIREBALLRECOVER = 0.75f;
    public const float FLYRECOVER = 0.5f;
    public const float GROUNDPOUNDRECOVER = 1.5f;


    private Transform initPosition;
    private float RecoverTimer;
    private float ShootCoolDownInterval;
    private float AttackTimer;
    private WizardState currentState;
    private WizardState nextState;
    private WizardAttackState currentAttackState;
    private WizardAttackState nextAttackState;
    private bool recoveryDone;
    private const float IFRAME_TIME = 0.05f;
    private float IFrameTime;
    private bool damaged;
    private bool calledOnce;
    private bool pouchEmpty;
    private GoldPouch pouch;
    private Rigidbody2D m_RigidBody2D;
    private Animator animator;
    private int fireBalls;
    private int GroundPounds;
    private bool facingRight;
    private bool prevFace;
    private bool doOnce;
    private float initHeight;


    void Awake()
    {
        calledOnce = true;
        doOnce = false;
        animator = (Animator)gameObject.GetComponent<Animator>();
        m_RigidBody2D = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
        pouch = (GoldPouch)gameObject.GetComponent<GoldPouch>();
        RecoverTimer = 0;
        recoveryDone = true;
        AttackTimer = 0;
        ShootCoolDownInterval = 0;
        fireBalls = 0;
        GroundPounds = 0;
        facingRight = false;
        prevFace = false;
        initPosition = gameObject.transform;
        currentState = WizardState.STATE_CALM;
        nextState = currentState;
        currentAttackState = WizardAttackState.STATE_RECOVERY;
        nextAttackState = WizardAttackState.STATE_RECOVERY;
    }

    void Start()
    {
        initHeight = gameObject.transform.position.y;
        startPoint = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.m_IsDead)
        {
            SwitchState();
            SwitchAttack();
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
        EnemyActive();
    }

    void SwitchState()
    {
        switch (currentState)
        {
            case WizardState.STATE_CALM:
                if (HP * 2 == MaxHP)
                {
                    nextState = WizardState.STATE_ANGRY;
                    animator.SetBool("Angry", true);
                }
                break;
            case WizardState.STATE_ANGRY:
                break;
        }
        currentState = nextState;
    }


    void SwitchAttack()
    {
        System.Random random = new System.Random();
        int nextAttack = random.Next(0, 3);
        switch (currentAttackState)
        {
            case WizardAttackState.STATE_RECOVERY:
                Recover();
                if (recoveryDone)
                {
                    facePlayer();
                    switch (nextAttack)
                    {
                        case 0:
                            nextAttackState = WizardAttackState.STATE_FIREBALL;
                            AttackTimer = FIREBALLTIMER;
                            ShootCoolDownInterval = SHOOTCOOLDOWN;
                            if(currentState == WizardState.STATE_CALM)
                            {
                                fireBalls = 1;
                            }
                            else
                            {
                                fireBalls = 3;
                            }
                            break;
                        case 1:
                            nextAttackState = WizardAttackState.STATE_FLY;
                            AttackTimer = FLYTIMER;
                            if (currentState == WizardState.STATE_CALM)
                            {
                                movSpeed = 4;
                            }
                            else
                            {
                                fireBalls = 6;
                            }
                            break;
                        case 2:
                            nextAttackState = WizardAttackState.STATE_GROUNDPOUND;
                            AttackTimer = GROUNDPOUNDTIMER;
                            if (currentState == WizardState.STATE_CALM)
                            {
                                GroundPounds = 0;
                            }
                            else
                            {
                                GroundPounds = 2;
                            }
                            break;
                    }
                }
                break;
            case WizardAttackState.STATE_FIREBALL:
                ShootFireBall();
                break;
            case WizardAttackState.STATE_FLY:
                Fly();
                break;
            case WizardAttackState.STATE_GROUNDPOUND:
                GroundPound();
                break;
        }
        currentAttackState = nextAttackState;
    }

    void Recover()
    {
        RecoverTimer -= Time.deltaTime;
        
        if(RecoverTimer < 0)
        {
            recoveryDone = true;
            animator.SetBool("Recover", false);
            m_RigidBody2D.position = new Vector2(m_RigidBody2D.position.x, initHeight);
        }
    }

    void ShootFireBall()
    {
        GameObject newGameObject;
        FireBall currentFireBall;
        float speed = 5;
        AttackTimer -= Time.deltaTime;
        ShootCoolDownInterval -= Time.deltaTime;
        animator.SetBool("FireBall", true);
        facePlayer();
        if (currentState == WizardState.STATE_ANGRY)
        {
            speed = 5;   
        }
        else if(currentState == WizardState.STATE_CALM)
        {
            speed = 7;
        }

        if(AttackTimer <= STARTSHOOTING && AttackTimer >= 0)
        {

            if(ShootCoolDownInterval < 0 && fireBalls > 0)
            {
                ShootCoolDownInterval = SHOOTCOOLDOWN;
                newGameObject = (GameObject)Instantiate(fireBall, m_RigidBody2D.position, Quaternion.identity);
                currentFireBall = (FireBall)newGameObject.GetComponent<FireBall>();
                currentFireBall.Target = Player.transform;
                currentFireBall.speed = speed;
                fireBalls--;
            }
        }
        else if(AttackTimer <= 0)
        {
            nextAttackState = WizardAttackState.STATE_RECOVERY;
            RecoverTimer = FIREBALLRECOVER;
            animator.SetBool("FireBall", false);
            recoveryDone = false;
        }
    }

    void Fly()
    {
        Transform target;
        animator.SetBool("Fly", true);
        if(m_RigidBody2D.position.x >= middlePoint.position.x)
        {
            target = enterPoint;
        }
        else
        {
            target = exitPoint;
        }    
        AttackTimer -= Time.deltaTime;
        if(AttackTimer <= 0)
        {
            nextAttackState = WizardAttackState.STATE_RECOVERY;
            RecoverTimer = FLYRECOVER;
            animator.SetBool("Fly", false);
            recoveryDone = false;
        }
        else if(Vector2.Distance(m_RigidBody2D.position,target.position) <= 0.2f)
        {
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position,new Vector2(target.position.x,m_RigidBody2D.position.y),movSpeed *Time.deltaTime);
        }
    }

    void GroundPound()
    {
        GameObject newGameObject;
        SandWall sandWall1;
        SandWall sandWall2;
        Debug.Log("GroundPounds:" + GroundPounds);
        animator.SetBool("GroundPound", true);
        AttackTimer -= Time.deltaTime;
        if(AttackTimer > CLIMBTIME)
        {
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position, new Vector2(Player.position.x, initHeight + height), fallSpeed * Time.deltaTime);
        }
        else if(AttackTimer <= CLIMBTIME && AttackTimer > FALLTIME)
        {
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position, new Vector2(Player.position.x, m_RigidBody2D.position.y), movSpeed * Time.deltaTime);
        }
        else if(AttackTimer <= FALLTIME && AttackTimer > IMPACTMOMENT)
        {
            m_RigidBody2D.position = Vector2.MoveTowards(m_RigidBody2D.position, new Vector2(m_RigidBody2D.position.x, initHeight), fallSpeed * Time.deltaTime);
        }
        else if(AttackTimer <= IMPACTMOMENT && AttackTimer > 0)
        {
            if (!doOnce)
            {
                doOnce = true;
                newGameObject = (GameObject)Instantiate(sandWall, new Vector3(m_RigidBody2D.position.x, initHeight, 0), Quaternion.identity);
                sandWall1 = (SandWall)newGameObject.GetComponent<SandWall>();
                newGameObject = (GameObject)Instantiate(sandWall, new Vector3(m_RigidBody2D.position.x, initHeight, 0), Quaternion.identity);
                sandWall2 = (SandWall)newGameObject.GetComponent<SandWall>();
                sandWall1.destroyPoint = enterPoint;
                sandWall1.movesRight = false;
                sandWall2.destroyPoint = exitPoint;
                sandWall2.movesRight = true;
            }

        }
        else if(AttackTimer < 0 && GroundPounds > 0)
        {
            AttackTimer = GROUNDPOUNDTIMER;
            doOnce = false;
            GroundPounds--;
        }
        else if (AttackTimer < 0 && GroundPounds <= 0)
        {
            nextAttackState = WizardAttackState.STATE_RECOVERY;
            animator.SetBool("GroundPound", false);
            animator.SetBool("Recover", true);
            RecoverTimer = GROUNDPOUNDRECOVER;
            recoveryDone = false;
            doOnce = false;
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

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void facePlayer()
    {
        if(Player.position.x - m_RigidBody2D.position.x <= 0)
        {
            facingRight = false;
            Xoffset = 2;
        }
        else
        {
            facingRight = true;
            Xoffset = -2;
        }

        if(prevFace != facingRight)
        {
            prevFace = facingRight;
            Flip();
        }
    }
}
