using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/**
 * SimpleEenemy este un inamic usor care tot ce face ii sa se plimbe dintr-o parte in alta
 */
public abstract class Enemy: MonoBehaviour
{
    public EnemyState state;

    public string EnemyID;

    void Start()
    {
        if(state.m_IsDead)
        {
            gameObject.SetActive(false);
        }
    }

    public bool MatchID(string ID)
    {
        return EnemyID.Equals(ID);
    }

    public void ChangeState(EnemyState _state)
    {
        state = _state;
    }

    public void OnEnemyResetEvent()
    {
        state.m_IsDead = false;
    }

    public abstract void TakeDamage(int damage);
}
