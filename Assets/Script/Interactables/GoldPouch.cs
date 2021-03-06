﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class GoldPouch : MonoBehaviour
{
    public int CoinNumber;
    public GameObject Coin;
    public GameObject TenCoin;

    private int spentCoins;

    public void Empty()
    {
        spentCoins = CoinNumber;
        while (spentCoins > 0)
        {
            if (spentCoins >= 10)
            {
                Instantiate(TenCoin, gameObject.transform.position, Quaternion.identity);
                spentCoins -= 10;
            }
            else
            {
                Instantiate(Coin, gameObject.transform.position, Quaternion.identity);
                spentCoins--;
            }
        }
    }
}
