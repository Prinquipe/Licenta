using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController: MonoBehaviour
{

    public InventoryState state;

    private int tempPouch;

    public const int pouchTreshold = 100;

    public const float startTempTimer = 2f;

    private float tempTimer;

    private const int MAXPOTIONS = 10;

    private const int MINPOTIONS = 0;

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
        if(state.Potions < MAXPOTIONS)
        {
            state.Potions++;
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
        if (state.Potions > MINPOTIONS )
        {
            state.Potions--;
        }
        else
        {
            //something something potions empty
        }
    }

    void addCoin()
    {
        
    }
}
