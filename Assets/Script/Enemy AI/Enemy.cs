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
    public Vector2 startPoint;
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

    public void ResetEnemy()
    {
        state.m_IsDead = false;
        gameObject.transform.position = startPoint;
    }

    public abstract void TakeDamage(int damage);
}
