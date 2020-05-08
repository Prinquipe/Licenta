﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerState
{
    // Start is called before the first frame update
    public int HP;

    public int currentMaxHP = 5;

    //public Transform lastCheckPoint;

    public bool hasDoubleJump;

    public bool hasDashAbility;

    public string sceneName;

    public int money;

    PlayerState(int _hp, int _curMaxHp, bool _double, bool _dash, string _name)
    {
        HP = _hp;
        currentMaxHP = _curMaxHp;
        hasDashAbility = _dash;
        hasDoubleJump = _double;
        sceneName = _name;

    }
}
