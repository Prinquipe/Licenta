using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrightnessButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject SliderObject;
    public Button button;
    private Sprite savedSprite;
    private float currentBright;
    private bool CanModify;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        currentBright = GameMgr.GetBrightness();
        CanModify = false;
        slider = (Slider)SliderObject.GetComponent<Slider>();
        slider.value = currentBright;
        slider.interactable = false;
        savedSprite = button.gameObject.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanModify)
        {
            button.gameObject.GetComponent<Image>().sprite = button.spriteState.pressedSprite; 
        }
        else
        {
            button.gameObject.GetComponent<Image>().sprite = savedSprite;
        }

        if(CanModify && (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            slider.value += 10f;
        }
        else if (CanModify && (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            slider.value -= 10f;
        }
        else if (CanModify && (Input.GetKeyDown(KeyCode.Return)))
        {
            currentBright = slider.value;
            CanModify = false;
            slider.interactable = false;
        }

        GameMgr.SetBrightness(currentBright);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CanModify = true;
        slider.interactable = true;
    }
}
