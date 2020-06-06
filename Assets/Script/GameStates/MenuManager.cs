using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private string[] ScenePath;
    public Button ContinueButton;
    public Button LoadButton;

    void Awake()
    {
        ScenePath = new string[2];
        ScenePath[0] = "NewGameMenuScene";
        ScenePath[1] = "OptionsMenu";
    }

    void Start()
    {
        Debug.Log(GameMgr.lastSlot);
        if (GameMgr.lastSlot == 0)
        {
            Image temp;
            ContinueButton.interactable = false;
            temp =(Image) ContinueButton.GetComponent<Image>();
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 0.3f);
            LoadButton.interactable = false;
            temp = (Image)LoadButton.GetComponent<Image>();
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 0.3f);
        }
    }

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(ScenePath[0]);
    }

    public void OnLoadGameButtonClicked()
    {
        SceneManager.LoadScene(ScenePath[0]);
        GameMgr.SetLoadGame();
    }

    public void OnOptionsButtonClicked()
    {
        SceneManager.LoadScene(ScenePath[1]);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void OnContinueButtonClicked()
    {
        GameMgr.SetCurrentSlot(GameMgr.lastSlot);
        string loadScene = GameMgr.GetLastSlotScene(GameMgr.currentSlot);
        GameMgr.NewGame = true;
        SceneManager.LoadScene(loadScene);
    }
}
