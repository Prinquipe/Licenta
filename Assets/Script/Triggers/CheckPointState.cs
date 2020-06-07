using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckPointState
{
    public string m_CheckID;
    public bool m_IsActivated;

    public CheckPointState(string _check, bool _active)
    {
        m_CheckID = _check;
        m_IsActivated = _active;
    }
}
