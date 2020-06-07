using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public GameObject player;

    private PlayerSave playerSave;
    private AssetManager asset;

    private bool called;
    // Start is called before the first frame update
    void Awake()
    {
        playerSave = (PlayerSave)player.GetComponent<PlayerSave>();
        asset = (AssetManager)GameObject.FindObjectOfType<AssetManager>();
        called = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!called)
        {
            if (other.CompareTag("Player"))
            {
                called = true;
                Debug.Log("Enter Trigger");
                playerSave.SaveObject();
                asset.LoadObject();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(called)
        {
            if (other.CompareTag("Player"))
            {
                called = false;
            }
        }
    }
}
