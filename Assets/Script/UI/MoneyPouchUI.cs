using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MoneyPouchUI : MonoBehaviour
{
    public GameObject player;
    public Image moneyPouch;
    public Text goldInPouch;
    public Text tempGold;
    public const float WAITONSCREEN = 5f;

    private float waitOnScreen2;
    private float waitOnScreen;
    private int previousAmount;
    private bool tempGoldNotGone;
    private bool goldInPouchNotGone;

    private InventoryController ctrl;
    // Start is called before the first frame update
    void Awake()
    {
        ctrl = (InventoryController)player.GetComponent<InventoryController>();
        waitOnScreen = WAITONSCREEN;
        waitOnScreen2 = WAITONSCREEN;
        previousAmount = ctrl.tempPouch;
        moneyPouch.enabled = false;
        goldInPouch.enabled = false;
        tempGold.enabled = false;
        tempGoldNotGone = false;
        goldInPouchNotGone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ctrl.tempPouch != previousAmount && ctrl.tempPouch != 0)
        {
            previousAmount = ctrl.tempPouch;
            tempGold.text = previousAmount.ToString();
            goldInPouch.text = ctrl.state.Pouch.ToString();
            tempGold.enabled = true;
            goldInPouch.enabled = true;
            moneyPouch.enabled = true;
            waitOnScreen = WAITONSCREEN;
            waitOnScreen2 = WAITONSCREEN;
            tempGoldNotGone = true;
        }

        if(waitOnScreen > 0 && tempGoldNotGone)
        {
            waitOnScreen -= Time.deltaTime;
        }
        else
        {
            if (tempGoldNotGone)
            {
                tempGold.enabled = false;
                tempGoldNotGone = false;
                goldInPouch.text = ctrl.state.Pouch.ToString();
                goldInPouchNotGone = true;
            }
        }    

        if(waitOnScreen2 > 0 && goldInPouchNotGone )
        {
            waitOnScreen2 -= Time.deltaTime;
        }
        else
        {
            if (goldInPouchNotGone)
            {
                goldInPouch.enabled = false;
                goldInPouchNotGone = false;
                moneyPouch.enabled = false;
            }
        }
    }
}
