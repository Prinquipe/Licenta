using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    BoxCollider2D attackBox;

    private bool meleeAttack;

    public int PlayerDamage;

    public static int AttackDamage = 1;

    public GameObject player;

    private PlayerMovement playerMov;

    private bool canMeleeAttack = true;

    private bool isAttacking;

    private const float INIT_TIME = 0f;

    public const float MELEE_ATTACK_TIME = 0.3f;

    public const float BOX_ENABLE_START_TIME = 0.2f;

    public const float BOX_ENABLE_END_TIME = 0.25f;

    private float mAttackTime;

    void Awake()
    {
        playerMov = (PlayerMovement)player.GetComponent<PlayerMovement>();
        attackBox = gameObject.GetComponent<BoxCollider2D>();
        mAttackTime = INIT_TIME;
        attackBox.enabled = false;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMeleeAttack)
        {
            if (Input.GetButtonDown("Fire1") && !playerMov.dash && !playerMov.jump)
            {
                canMeleeAttack = false;
                isAttacking = true;
                playerMov.Attack(isAttacking);
            }
        }
        else
        {
            if (mAttackTime < BOX_ENABLE_START_TIME)
            {
                mAttackTime += Time.deltaTime;
            }
            else if (mAttackTime >= BOX_ENABLE_START_TIME && mAttackTime < BOX_ENABLE_END_TIME)
            {
                attackBox.enabled = true;
                mAttackTime += Time.deltaTime;
            }
            else if (mAttackTime >= BOX_ENABLE_END_TIME && mAttackTime < MELEE_ATTACK_TIME)
            {
                attackBox.enabled = false;
                mAttackTime += Time.deltaTime;
            }
            else
            {
                canMeleeAttack = true;
                attackBox.enabled = false;
                isAttacking = false;
                mAttackTime = INIT_TIME;
                playerMov.Attack(isAttacking);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        playerMov.TakeDamage(damage);
    }
}
