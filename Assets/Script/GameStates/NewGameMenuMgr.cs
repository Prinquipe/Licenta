using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenuMgr : MonoBehaviour
{
    public Button[] slot;
    public Text[] slotText;
    
    // Start is called before the first frame update
    void Awake()
    {
       if(!GameMgr.GetLoadGame())
       {
            for(int i =0; i < slot.Length; i++)
            {
                if(GameMgr.CheckIfEmpty(i+1))
                {
                    slotText[i].text = "Slot" + (i+1) + " Empty";
                }
                else
                {
                    slotText[i].text = "Slot" + (i+1) + ": " + GameMgr.GetSlotDateTime(i);
                }
            }
       }
       else
        {
            for (int i = 0; i < slot.Length; i++)
            {
                if (GameMgr.CheckIfEmpty(i+1))
                {
                    slot[i].enabled = false;
                    slotText[i].text = "Slot" + (i+1)+ " Empty";
                }
                else
                {
                    slotText[i].text = "Slot" + (i+1) + ":" + GameMgr.GetSlotDateTime(i);
                }
            }
        }
    }


    public void OnSlot1Clicked()
    {

    }

    public void OnSlot2Clicked()
    {

    }

    public void OnSlot3Clicked()
    {

    }

}
