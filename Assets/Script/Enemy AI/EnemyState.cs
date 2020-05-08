using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyState
{
    public string m_EnemyID;
    public bool m_IsDead;
    public bool m_IsBoss;

    EnemyState(string _id,bool _dead, bool _boss)
    {
        m_EnemyID = _id;
        m_IsDead = _dead;
        m_IsDead = _boss;
    }
}
