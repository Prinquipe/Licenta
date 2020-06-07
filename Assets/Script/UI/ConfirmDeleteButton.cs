using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDeleteButton : MonoBehaviour
{
    public Text deleteText;
    public Button deleteButton;
    public int slot;
    
    public void OnConfirmDeleteButtonClicked()
    {
        deleteText.text = "Slot" + slot +" Empty";
        GameMgr.DeleteSlot(slot);
        deleteButton.interactable = false;
        deleteButton.GetComponent<Image>().enabled = false;
    }
}
