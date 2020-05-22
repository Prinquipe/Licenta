using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, Interactable
{
    public DoorState state;
    public string DoorID;
    public Animator animator;
    public BoxCollider2D solidBox;
    public BoxCollider2D triggerBox;

    private bool PlayerEntered;
    private InventoryController ctrl;
    private bool open;

    [Header("Event")]
    [Space]

    public UnityEvent RequestSaveEvent;

    void Awake()
    {
        if (RequestSaveEvent == null)
        {
            RequestSaveEvent = new UnityEvent();
        }
        PlayerEntered = false;
        open = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && PlayerEntered)
        {
            if(ctrl.hasKey(state.m_Type))
            {
                ctrl.consumeKey(state.m_Type);
                Interact();
            }
        }

        if(open == state.m_IsClosed)
        {
            open = !state.m_IsClosed;
            animator.SetBool("isOpen",open);
            solidBox.enabled = state.m_IsClosed;
            triggerBox.enabled = state.m_IsClosed;
        }
    }

    public void Open()
    {
        state.m_IsLocked = false;
        state.m_IsClosed = false;
        RequestSaveEvent.Invoke();
    }

    public void Interact()
    {
        Open();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerDamage"))
        {
            ctrl = (InventoryController)other.gameObject.GetComponent<InventoryController>();
            PlayerEntered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            PlayerEntered = false;
        }
    }

    public bool MatchID(string ID)
    {
        return DoorID.Equals(ID);
    }

    public void ChangeState(DoorState _state)
    {
        state = _state;
    }
}
