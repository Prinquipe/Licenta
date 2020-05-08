﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public enum DashState
    {
        DASHREADY,
        DASHACTIVE,
        DASHCOOLDOWN
    }
    public PlayerController controller;

    public PlayerState state;

    public InventoryController invController;

    public Transform startPosition;

    public Animator animator;

    public float dashTimer;

    public float maxDashTime = 1f;

    private float horizontalMove = 0f;

    public DashState dashState;

    private bool jump = false;

    private bool doubleJump = false;

    private float doubleJumpTime;

    public const float maxDoubleDashTime = .5f;

    private bool dash = false;

    [SerializeField] public float runSpeed;
    // Update is called once per frame

    void Update()
    {
        if (state.hasDashAbility)
        {
            Dash();
        }
        if (dashState != DashState.DASHACTIVE)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
        animator.SetFloat("runSpeed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            if (!jump)
            {
                Debug.Log("Jumping");
                jump = true;
                animator.SetBool("isJumping", true);
            }
            else if(!doubleJump && state.hasDoubleJump)
            {
                Debug.Log("DoubleJumping");
                doubleJumpTime = 0;
                doubleJump = true;
                animator.SetBool("isDoubleJumping", true);
            }
        }

        if(doubleJump && state.hasDoubleJump)
        {
            if(doubleJumpTime >=maxDashTime)
            {
                animator.SetBool("isDoubleJumping", false);
            }
            else
            {
                doubleJumpTime += Time.deltaTime;
            }
        }
    }

    public void OnLanding()
    {
        Debug.Log("OnLanding called");
        animator.SetBool("isJumping", false);
        jump = false;
        doubleJump = false;
    }

    public void Dash()
    {
        switch(dashState)
        {
            case DashState.DASHREADY:
                bool isDashKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if(isDashKeyDown)
                {
                    animator.SetBool("isDashing", true);
                    dash = true;
                    dashState = DashState.DASHACTIVE;
                }
                break;

            case DashState.DASHACTIVE:
                dashTimer += Time.deltaTime;
                
                if(dashTimer >=maxDashTime)
                {
                    dashTimer = maxDashTime;
                    dash = false;
                    animator.SetBool("isDashing", false);
                    dashState = DashState.DASHCOOLDOWN;
                }
                break;
            case DashState.DASHCOOLDOWN:
                dashTimer -= Time.deltaTime;
                if(dashTimer <=0)
                {
                    dashTimer = 0;
                    dashState = DashState.DASHREADY;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        //move character
        controller.Move(horizontalMove * Time.fixedDeltaTime,dash, jump,doubleJump);
    }
}
