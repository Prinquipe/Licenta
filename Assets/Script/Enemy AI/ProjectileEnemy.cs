using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    public int HP;

    private BoxCollider2D box;

    void Awake()
    {
        box = (BoxCollider2D)GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void TakeDamage(int damage)
    {

        HP -= damage;
        if(HP > 0)
        {
        }    
        else
        {
            state.m_IsDead = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            box.enabled = false;
        }
    }


}
