using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private string[] ScenePath;

    void Awake()
    {
        ScenePath = new string[2];
        ScenePath[0] = "NewGameMenuScene";
        ScenePath[1] = "OptionsMenuScene";
    }

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(ScenePath[0]);
    }

    public void OnLoadGameButtonClicked()
    {
        SceneManager.LoadScene(ScenePath[0]);
    }

    public void OnOptionsButtonClicked()
    {
        SceneManager.LoadScene(ScenePath[1]);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
