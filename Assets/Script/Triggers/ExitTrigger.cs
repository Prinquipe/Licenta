using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public GameObject asset;
    public string nextScene;

    private AssetManager assetMgr;

    private bool called;

    void Awake()
    {
        assetMgr = (AssetManager)asset.GetComponent<AssetManager>();
        called = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject interObj;
        if(!called)
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter Exit Trigger");
            called = true;
            interObj = other.gameObject;
            assetMgr.SaveObject();
            DontDestroyOnLoad(interObj);
            SceneManager.LoadScene(nextScene);
        }
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
