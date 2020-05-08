using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemState
{
    public string m_ItemID;
    public GameEnums.KEY_TYPE m_Type;
    public bool m_IsKey;
    public bool m_IsPickedUp;

    public ItemState(string _ItemId,bool isKey,bool isPickedUp,GameEnums.KEY_TYPE type = 0)
    {
        m_ItemID = _ItemId;
        m_IsKey = isKey;
        m_IsPickedUp = isPickedUp;
        m_Type = type;
    }
}
