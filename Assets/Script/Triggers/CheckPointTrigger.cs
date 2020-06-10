using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckPointTrigger : MonoBehaviour
{
    public CheckPointState state;
    public string CheckPointID;
    public GameObject player;
    public GameObject sprite;
    public Image blackScreen;
    public const float FADETIME = 1f;
    public const float HEALTIMER = 0.5f;
    public static bool PlayerEntered;
    public float step;
    public AssetManager asset;

    private float initAlpha;
    private float FadeTime;
    private float HealTimer;
    private Animator animator;
    private PlayerMovement mov;
    private bool smolder;
    private bool AckExpected;

    [Header("Event")]
    [Space]

    public UnityEvent RequestSaveEvent;
    public UnityEvent ResetEnemyEvent;


    void Awake()
    {
        initAlpha = 1f;
        PlayerEntered = false;
        if (RequestSaveEvent == null)
        {
            RequestSaveEvent = new UnityEvent();
        }
        if(ResetEnemyEvent == null)
        {
            ResetEnemyEvent = new UnityEvent();
        }

        FadeTime = FADETIME;
        HealTimer = HEALTIMER;
        animator = (Animator)sprite.GetComponent<Animator>();
        mov = (PlayerMovement)player.GetComponent<PlayerMovement>();
        smolder = false;
        AckExpected = false;
    }


    void Start()
    {
        if(state.m_IsActivated)
        {
            animator.SetBool("isSmoldering", state.m_IsActivated);
            animator.SetBool("isActive", state.m_IsActivated);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && smolder)
        {
            Debug.Log("Pressed E");
            if (!state.m_IsActivated)
            {
                state.m_IsActivated = true;
                animator.SetBool("isActive", state.m_IsActivated);
            }
            mov.state.m_CheckPointName = CheckPointID;
            AckExpected = true;
            mov.CheckPointScene = gameObject.scene.name;
            ResetEnemyEvent.Invoke();
        }

        if(smolder && state.m_IsActivated)
        {
            if(HealTimer > 0)
            {
                HealTimer -= Time.deltaTime;
            }
            else if(mov.state.HP < mov.state.currentMaxHP)
            {
                mov.HealOneBar();
                HealTimer = HEALTIMER;
            }
        }

        FadeTime -= Time.deltaTime;
        if (FadeTime > 0 && PlayerEntered)
        {
            initAlpha -= step;
            if (initAlpha < 1f)
            {
                blackScreen.color = new Color(0f, 0f, 0f, initAlpha);
            }
        }
    }

    public bool isCheckPoint(string name)
    {
        bool res = false;
        if(name.Equals(CheckPointID))
        {
            res = true;
        }
        return res;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerDamage"))
        {
            if(mov.PlayerDied)
            {
                mov.PlayerDied = false;
                ResetEnemyEvent.Invoke();
            }
            else if(!PlayerEntered)
            {
                ResetEnemyEvent.Invoke();
            }
            
            HealTimer = HEALTIMER;
            smolder = true;
            animator.SetBool("isSmoldering", smolder);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            smolder = false;
            animator.SetBool("isSmoldering", smolder);
        }
    }

    public void OnAckResetEnemyEvent()
    {
        Debug.Log("Reset Done");
        if (AckExpected)
        {
            Debug.Log("Save Enemy");
            AckExpected = false;
            RequestSaveEvent.Invoke();
        }
    }

    public void OnAckSaveAssetEvent()
    {
        if(!PlayerEntered)
        {
            PlayerEntered = true;
            asset.LoadObject();
        }
    }

    public Transform GetPosition()
    {
        return gameObject.transform;
    }

    public bool MatchID(string ID)
    {
        return CheckPointID.Equals(ID);
    }

    public void ChangeState(CheckPointState _state)
    {
        state = _state;
    }
}
