using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject parent;
    public const float I_FRAME_TIME = 2f;

    private InventoryController m_Inventory;
    private GameObject m_CurrentInterObj;
    private PlayerMovement playerMov;
    private SpriteRenderer Renderer;
    private Door m_DoorScript;
    private Item m_ItemScript;
    private float IFrameTimer;
    private const float BLINK_TIMER = 0.2f;
    private float BlinkTimer;
    private bool blink;
    private bool damaged;


    void Awake()
    {
        IFrameTimer = I_FRAME_TIME;
        BlinkTimer = 0;
        damaged = false;
        m_Inventory = (InventoryController)gameObject.GetComponent<InventoryController>();
        playerMov = (PlayerMovement)parent.GetComponent<PlayerMovement>();
        Renderer = (SpriteRenderer)parent.GetComponent<SpriteRenderer>();
        blink = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(damaged)
        {
            if(IFrameTimer >= 0f)
            {
                IFrameTimer -= Time.deltaTime;
                
                if(BlinkTimer < BLINK_TIMER)
                {
                    BlinkTimer += Time.deltaTime;
                }
                else
                {
                    BlinkTimer = 0f;
                    blink = !blink;
                    Renderer.enabled = blink;
                }
            }
            else
            {
                IFrameTimer = I_FRAME_TIME;
                damaged = false;
                Renderer.enabled = true;
                blink = true;
            }
        }
    }

    public void TakeDamage(int i)
    {
        if (!damaged)
        {
            playerMov.TakeDamage(i);
            damaged = true;
        }
    }
}
