using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Canvas canvas;
    public Image canvasImage;
    public Button[] buttons;
    public Text[] textBoxes;

    private int AckCounter;
    private bool AckExpected;
    // Start is called before the first frame update

    [Header("Player Event")]
    [Space]

    public UnityEvent RequestSaveEvent;

    void Awake()
    {
        AckExpected = false;
        AckCounter = 2;
        Time.timeScale = 1f;
        if (RequestSaveEvent == null)
        {
            RequestSaveEvent = new UnityEvent();
        }
        canvas.enabled = false;
        foreach(Button b in buttons)
        {
            b.interactable = false;
        }
        foreach(Text t in textBoxes)
        {
            t.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            canvas.enabled = true;
            foreach (Button b in buttons)
            {
                b.interactable = true;
            }
            foreach (Text t in textBoxes)
            {
                t.enabled = true;
            }
        }
    }

    public void OnResumeButton()
    {
        canvas.enabled = false;
        Time.timeScale = 1f;
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
        foreach (Text t in textBoxes)
        {
            t.enabled = false;
        }
    }

    public void OnQuitButton()
    {
        canvasImage.color = new Color(0f, 0f, 0f, 1f); 

        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
        foreach (Text t in textBoxes)
        {
            t.enabled = false;
        }

        AckExpected = true;
        RequestSaveEvent.Invoke();
    }

    public void OnAckSaveAssetEvent()
    {
        AckCounter--;
        if (AckCounter <= 0 && AckExpected)
        {

            Debug.Log("Change Scene");
            SceneManager.LoadScene(GameMgr.mainMenu);
        }
    }

    public void OnAckPlayerSaveEvent()
    {
        AckCounter--;
        if (AckCounter <= 0 && AckExpected)
        {
            
            Debug.Log("Change Scene");
            SceneManager.LoadScene(GameMgr.mainMenu);
        }
    }
}
