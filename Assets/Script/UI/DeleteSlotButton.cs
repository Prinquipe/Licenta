using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSlotButton : MonoBehaviour
{
    public Button confirmButton;
    // Start is called before the first frame update
    void Awake()
    {
        confirmButton.interactable = false;
        confirmButton.GetComponent<Image>().enabled = false;
    }

    public void OnDeleteButtonClicked()
    {
        confirmButton.interactable = true;
        confirmButton.GetComponent<Image>().enabled = true;
    }
}
