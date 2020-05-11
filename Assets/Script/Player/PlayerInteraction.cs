using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private InventoryController m_Inventory;
    private GameObject m_CurrentInterObj;
    private Door m_DoorScript;
    private Item m_ItemScript;

    void Awake()
    {
        m_Inventory = (InventoryController)gameObject.GetComponent<InventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && m_CurrentInterObj)
        {
            if(m_CurrentInterObj.CompareTag("Door"))
            {
                m_DoorScript = (Door)m_CurrentInterObj.GetComponent<Door>();
                if(m_DoorScript.state.m_IsLocked)
                {
                    bool res;
                    res = m_Inventory.hasKey(m_DoorScript.state.m_Type);
                    if(res)
                    {
                        m_Inventory.consumeKey(m_DoorScript.state.m_Type);
                        m_DoorScript.Open();
                    }
                }
            }
            if(m_CurrentInterObj.CompareTag("Item"))
            {
                m_ItemScript = (Item)m_CurrentInterObj.GetComponent<Item>();
                if (m_ItemScript.state.m_IsKey)
                {
                    m_Inventory.addKey(m_ItemScript.state.m_Type);
                }
                else
                {
                    bool res;
                    res = m_Inventory.addPotion();
                }
            }
        }
    }

    void OnEnterTrigger2D(Collider2D other)
    {
        m_CurrentInterObj = other.gameObject;
    }
}
