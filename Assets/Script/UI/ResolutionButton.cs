using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionButton : MonoBehaviour, IPointerDownHandler
{
    public Resolution[] resolutions;
    public Text displayResolution;
    public Button button;
    private Resolution currentRes;
    private Sprite savedSprite;
    private int InOrder;
    private bool CanModify;

    void Start()
    {
        resolutions = Screen.resolutions;
        currentRes =Screen.currentResolution;
        for(int i = 0; i < resolutions.Length; i++)
        {
            if((currentRes.width == resolutions[i].width) && (currentRes.height == resolutions[i].height))
            {
                InOrder = i;
            }
        }

        savedSprite = button.gameObject.GetComponent<Image>().sprite;
        Debug.Log(currentRes.ToString());
        string rezText = currentRes.width + "X" + currentRes.height;
        displayResolution.text = rezText;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CanModify = true;
    }

    // Update is called once per frame
    void Update()
    {
        string rezText = string.Empty;

        if (CanModify)
        {
            button.gameObject.GetComponent<Image>().sprite = button.spriteState.pressedSprite;
        }
        else
        {
            button.gameObject.GetComponent<Image>().sprite = savedSprite;
        }

        if (CanModify && (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            if (InOrder < resolutions.Length-1)
            {
                InOrder++;
                rezText = resolutions[InOrder].width + "X" + resolutions[InOrder].height;
                displayResolution.text = rezText;
            }
        }
        else if(CanModify && (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            if (InOrder > 0)
            {
                InOrder--;
                rezText = resolutions[InOrder].width + "X" + resolutions[InOrder].height;
                displayResolution.text = rezText;
            }
        }
        else if(CanModify && (Input.GetKeyDown(KeyCode.Return)))
        {
            CanModify = false;
            currentRes = resolutions[InOrder];
            GameMgr.SetCurrentResolution(currentRes);
        }
    }
}
