using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorState
{
    public string m_DoorID;
    public bool m_IsLocked;
    public bool m_IsClosed;
    public bool m_NeedsKey;
    public GameEnums.KEY_TYPE m_Type;

    public DoorState(string doorID,bool locked,bool needsKey,bool isClosed,GameEnums.KEY_TYPE type = 0)
    {
        m_DoorID = doorID;
        m_IsLocked = locked;
        m_NeedsKey = needsKey;
        m_IsClosed = isClosed;
        m_Type = type;
    }
}
