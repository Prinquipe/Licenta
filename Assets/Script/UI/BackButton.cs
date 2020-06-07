using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        string Scene = GameMgr.mainMenu;
        SceneManager.LoadScene(Scene);
    }
}
