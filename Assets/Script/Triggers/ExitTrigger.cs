using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitTrigger : MonoBehaviour
{
    public GameObject asset;
    public Image blackScreen;
    public string nextScene;
    public float step;
    public const float FADETIME = 1f;

    private float FadeTime;
    private float initAlpha;
    private AssetManager assetMgr;
    private GameObject interObj;

    private bool called;

    void Awake()
    {
        initAlpha = 0f;
        blackScreen.enabled = false;
        blackScreen.color = new Color(0f, 0f, 0f, 0f);
        assetMgr = (AssetManager)asset.GetComponent<AssetManager>();
        called = false;
        FadeTime = 0f;
    }

    void Update()
    {
        if (FadeTime <= 0 && called)
        {
            ChangeScene();
        }
        else if (called)
        {
            initAlpha += step;
            Debug.Log("InitAlpha=" + initAlpha);
            if(initAlpha < 1f)
            {
                blackScreen.color = new Color(0f, 0f, 0f, initAlpha);
            }
        }
        FadeTime -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!called)
            {
                Debug.Log("Enter ExitTrigger");
                called = true;
                interObj = other.gameObject;
                FadeTime = FADETIME;
                blackScreen.enabled = true;
                assetMgr.SaveObject();
            }
        }
    }

    void ChangeScene()
    {
        DontDestroyOnLoad(interObj);
        SceneManager.LoadScene(nextScene);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (called)
        {
            if (other.CompareTag("Player"))
            {
                called = false;
            }
        }
    }
}
