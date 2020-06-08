using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class EnterTrigger : MonoBehaviour
{
    public GameObject player;
    public Image blackScreen;
    public const float FADETIME = 1f;
    public float step;

    private float initAlpha;
    private float FadeTime;
    private PlayerSave playerSave;
    private AssetManager asset;
    private bool called;
    // Start is called before the first frame update
    void Awake()
    {
        initAlpha = 1f;
        playerSave = (PlayerSave)player.GetComponent<PlayerSave>();
        asset = (AssetManager)GameObject.FindObjectOfType<AssetManager>();
        called = false;
        FadeTime = 0f;
    }

    void Start()
    {
        blackScreen.enabled = true;
        blackScreen.color = new Color(0f, 0f, 0f, 1f);
    }

    void Update()
    {
        if (FadeTime > 0 && called)
        {
            initAlpha -= step;
            if (initAlpha < 1f)
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
                called = true;
                Debug.Log("Enter Trigger");
                FadeTime = FADETIME;
                asset.LoadObject();
            }

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
