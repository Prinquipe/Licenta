using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InventoryController m_Inventory;
    
    public GameObject m_CurrentInterObj;
    public Door m_DoorScript;
    public Item m_ItemScript;

    void Awake()
    {
        if (m_Inventory == null)
        {
            m_Inventory = new InventoryController();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && m_CurrentInterObj)
        {
            if(m_CurrentInterObj.CompareTag("Door"))
            {
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
                if (m_ItemScript.state.m_IsKey)
                {
                    m_Inventory.addKey(m_ItemScript.state.m_Type);
                    Destroy(m_CurrentInterObj);
                }
                else
                {
                    bool res;
                    res = m_Inventory.addPotion();
                    if(res)
                    {
                        Destroy(m_CurrentInterObj);
                    }
                    else
                    {
                        //show potions is full
                    }
                }
            }
        }
    }
}
