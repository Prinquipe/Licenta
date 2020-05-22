using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, Interactable
{
    public ItemState state;

    public string ItemID;

    private int eventCalls = 2;

    private bool PlayerEntered;

    private InventoryController ctrl;

    [Header("Event")]
    [Space]

    public UnityEvent RequestSaveEvent;

    void Awake()
    {
        if (RequestSaveEvent == null)
            RequestSaveEvent = new UnityEvent();
        PlayerEntered = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && PlayerEntered)
        {
            RemoveItem();
            ctrl.addKey(state.m_Type);
        }
    }

    void Start()
    {
        if (state.m_IsPickedUp)
        {
            Debug.Log("Key already PickedUp");
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        //
    }


    public bool MatchID(string ID)
    {
        if (ItemID.Equals(ID))
            return true;
        return false;
    }


    public void ChangeState(ItemState _state)
    {
        state = _state;
    }

    public void RemoveItem()
    {
        state.m_IsPickedUp = true;
        RequestSaveEvent.Invoke();
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Area Entered");
        if(other.CompareTag("PlayerDamage"))
        {
            ctrl = (InventoryController)other.gameObject.GetComponent<InventoryController>();
            PlayerEntered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Area Exited");
        if (other.GetComponent("PlayerDamage"))
        {
            PlayerEntered = false;
        }
    }

    public void OnAckSaveAssetEvent()
    {
        Debug.Log("Called "+ gameObject.name);
        eventCalls--;
        if (eventCalls <= 0)
        {
            Debug.Log("Safe to Destroy. Saved in Both Locations");
            if (state.m_IsPickedUp)
            {
                Destroy(gameObject);
            }
            else
            {
                eventCalls = 2;
            }
        }
    }

    public void OnAckSavePlayerEvent()
    {
        Debug.Log("Called " + gameObject.name);
        eventCalls--;
        if (eventCalls <= 0)
        {
            Debug.Log("Safe to Destroy. Saved in Both Locations");
            if (state.m_IsPickedUp)
            {
                Destroy(gameObject);
            }
            else
            {
                eventCalls = 2;
            }
        }
    }
}
