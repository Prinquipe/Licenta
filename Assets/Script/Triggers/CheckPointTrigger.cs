using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointTrigger : MonoBehaviour
{
    public CheckPointState state;
    public string CheckPointID;
    public GameObject player;
    public GameObject sprite;


    private Animator animator;
    private PlayerMovement mov;
    private bool PlayerEntered;
    private bool smolder;
    private bool AckExpected;

    [Header("Event")]
    [Space]

    public UnityEvent RequestSaveEvent;
    public UnityEvent ResetEnemyEvent;


    void Awake()
    {
        if(RequestSaveEvent == null)
        {
            RequestSaveEvent = new UnityEvent();
        }
        if(ResetEnemyEvent == null)
        {
            ResetEnemyEvent = new UnityEvent();
        }

        animator = (Animator)sprite.GetComponent<Animator>();
        mov = (PlayerMovement)player.GetComponent<PlayerMovement>();
        smolder = false;
        AckExpected = false;
    }


    void Start()
    {
        if(state.m_IsActivated)
        {
            smolder = state.m_IsActivated;
            animator.SetBool("isSmoldering", smolder);
            animator.SetBool("isActive", state.m_IsActivated);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && PlayerEntered)
        {
            Debug.Log("Pressed E");
            if (!state.m_IsActivated)
            {
                state.m_IsActivated = true;
                animator.SetBool("isActive", state.m_IsActivated);
                mov.state.m_CheckPointName = CheckPointID;
                ResetEnemyEvent.Invoke();
                AckExpected = true;
                mov.CheckPointScene = gameObject.scene.name; 
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
            Debug.Log("Enter Campsite");
            PlayerEntered = true;
            smolder = true;
            animator.SetBool("isSmoldering", smolder);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            PlayerEntered = false;
            smolder = false;
            animator.SetBool("isSmoldering", smolder);
        }
    }

    public void OnAckResetEnemyEvent()
    {
        if (AckExpected)
        {
            AckExpected = false;
            RequestSaveEvent.Invoke();
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
