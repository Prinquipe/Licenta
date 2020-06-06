using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameMenuMgr : MonoBehaviour
{
    public Button[] slot;
    public Button[] deleteButton;
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
                    deleteButton[i].interactable = false;
                    deleteButton[i].GetComponent<Image>().enabled = false;
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
        if (!GameMgr.CheckIfEmpty(1))
        {
            StartGame(1);
        }
        else
        {
            StartNewGame(1);
        }
    }

    public void OnSlot2Clicked()
    {
        if (!GameMgr.CheckIfEmpty(2))
        {
            StartGame(2);
        }
        else
        {
            StartNewGame(2);
        }
    }

    public void OnSlot3Clicked()
    {
        if (!GameMgr.CheckIfEmpty(3))
        {
            StartGame(3);
        }
        else
        {
            StartNewGame(3);
        }
    }


    void StartGame(int i)
    {
        GameMgr.SetCurrentSlot(i);
        string loadScene = GameMgr.GetLastSlotScene(GameMgr.currentSlot);
        GameMgr.NewGame = true;
        SceneManager.LoadScene(loadScene);
    }

    void StartNewGame(int i)
    {
        GameMgr.SetCurrentSlot(i);
        SceneManager.LoadScene(GameMgr.newGameScene);
    }
}
