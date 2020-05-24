using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{

    public const float HEAL_WAIT_TIME = 1f;
    public GameObject Player;

    private float HealTime;
    private bool CanHeal;
    private bool Save;
    private PlayerMovement PlayerMov;

    [Header("Event")]
    [Space]

    public UnityEvent RequestSaveEvent;
    [Space]

    public UnityEvent RequestResetEnemy;

    void Awake()
    {
        if(RequestSaveEvent == null)
        {
            RequestSaveEvent = new UnityEvent();
        }
        if(RequestResetEnemy == null)
        {
            RequestResetEnemy = new UnityEvent();
        }

        PlayerMov = (PlayerMovement)Player.gameObject.GetComponent<PlayerMovement>();
        HealTime = HEAL_WAIT_TIME;
        CanHeal = false;
        Save = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanHeal && PlayerMov.state.HP < PlayerMov.state.currentMaxHP)
        {
            if(HealTime > 0)
            {
                HealTime -= Time.deltaTime;
            }
            else
            {
                HealTime = HEAL_WAIT_TIME;
                PlayerMov.state.HP++;
            }
        }
        else if(PlayerMov.state.HP == PlayerMov.state.currentMaxHP)
        {
            if(Save)
            {
                Save = false;
                RequestSaveEvent.Invoke();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerDamage"))
        {
            CanHeal = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            CanHeal = false;
            RequestResetEnemy.Invoke();
        }
    }
}
