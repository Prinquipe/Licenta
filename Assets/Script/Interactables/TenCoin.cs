﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public const float LIFESPAN_TIMER = 5f;
    public const float START_BLINK_TIMER = 2f;
    public const float BLINK_TIMER = 0.25f;

    private const int coinAmount = 10;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer Renderer;
    private float LifeSpanTimer;
    private float BlinkTimer;
    private bool BlinkOn;


    void Awake()
    {
        if (rigidBody2D == null)
        {
            rigidBody2D = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
        }
        if (Renderer == null)
        {
            Renderer = (SpriteRenderer)gameObject.GetComponent<SpriteRenderer>();
        }
        LifeSpanTimer = LIFESPAN_TIMER;
        BlinkTimer = BLINK_TIMER;
        BlinkOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LifeSpanTimer > START_BLINK_TIMER)
        {
            LifeSpanTimer -= Time.deltaTime;
        }
        else if (LifeSpanTimer > 0)
        {
            LifeSpanTimer -= Time.deltaTime;
            if (BlinkTimer <= 0)
            {
                Renderer.enabled = !BlinkOn;
                BlinkOn = !BlinkOn;
            }
            else
            {
                BlinkTimer -= Time.deltaTime;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        InventoryController inv;
        if(other.CompareTag("PlayerDamage"))
        {
            inv = (InventoryController)gameObject.GetComponent<InventoryController>();
            inv.AddCoin(coinAmount);
            Destroy(gameObject);
        }
    }
}