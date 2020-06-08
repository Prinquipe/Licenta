using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InventoryController: MonoBehaviour
{

    public InventoryState state;

    public int tempPouch;

    public int Potions;

    public const int pouchTreshold = 100;

    public const float startTempTimer = 2f;

    private float tempTimer;

    private const int MAXPOTIONS = 3;

    private const int MINPOTIONS = 0;

    private bool reset;

    private bool countDown;

    void Awake()
    {
        reset = false;
        countDown = false;
        tempPouch = 0;
        Potions = MAXPOTIONS;
    }

    void Update()
    {
        if (reset)
        {
            reset = false;
            tempTimer = startTempTimer;
            countDown = true;
        }

        if (countDown)
        {
            if (tempTimer > 0)
            {
                tempTimer -= Time.deltaTime;
            }
            else
            {
                tempTimer = startTempTimer;
                state.Pouch += tempPouch;
                tempPouch = 0;
                countDown = false;
            }
        }
    }

    public void consumeKey(GameEnums.KEY_TYPE type)
    {
        switch(type)
        {
            case GameEnums.KEY_TYPE.KEY_BRONZE:
                if(state.BronzeKey > 0)
                {
                    state.BronzeKey--;
                }
                else
                {
                    //something something don't have key
                }
                break;
            case GameEnums.KEY_TYPE.KEY_SILVER:
                if (state.BronzeKey > 0)
                {
                    state.SilverKey--;
                }
                else
                {
                    //something something don't have key
                }
                break;
            case GameEnums.KEY_TYPE.KEY_GOLD:
                if (state.BronzeKey > 0)
                {
                    state.GoldKey--;
                }
                else
                {
                    //something something don't have key
                }
                break;
        }
    }

    public bool hasKey(GameEnums.KEY_TYPE type)
    {
        switch (type)
        {
            case GameEnums.KEY_TYPE.KEY_BRONZE:
                if (state.BronzeKey > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case GameEnums.KEY_TYPE.KEY_SILVER:
                if (state.SilverKey > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case GameEnums.KEY_TYPE.KEY_GOLD:
                if (state.GoldKey > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }


    public void addKey(GameEnums.KEY_TYPE type)
    {
        switch (type)
        {
            case GameEnums.KEY_TYPE.KEY_BRONZE:
                Debug.Log("Before:"+state.BronzeKey);
                state.BronzeKey++;
                Debug.Log("After:" + state.BronzeKey);
                break;
            case GameEnums.KEY_TYPE.KEY_SILVER:
                state.SilverKey++;
                break;
            case GameEnums.KEY_TYPE.KEY_GOLD:
                state.GoldKey++;
                break;
        }
    }

    public bool addPotion()
    {
        if(Potions < MAXPOTIONS)
        {
            Potions++;
            return true;
        }
        else
        {
            //something something potions full
            return false;
        }
    }

    public void removePotion()
    {
        if (Potions > MINPOTIONS )
        {
            Potions--;
        }
        else
        {
            //something something potions empty
        }
    }

    public void AddCoin(int amount)
    {
        Debug.Log("Add Coin "+ amount);
        reset = true;
        tempPouch += amount;
    }
}
