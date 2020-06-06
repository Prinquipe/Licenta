using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using System.Security.Cryptography;
using System;

public class PlayerSave: MonoBehaviour,Saveable
{
    public PlayerWrapper wrap;

    public GameObject player;

    public GameObject playerInv;

    private PlayerState playerState;

    private InventoryState inState;

    private PlayerMovement playerMov;

    private InventoryController inv;

    private string path;

    private readonly object playerSaveLock = new object();

    [Header("Player Event")]
    [Space]

    public UnityEvent AckPlayerSaveEvent;

    void Awake()
    {
        if(AckPlayerSaveEvent == null)
        {
            AckPlayerSaveEvent = new UnityEvent();
        }
        GameMgr.setSavePath();
        path = GameMgr.savePath;
        playerMov =(PlayerMovement)player.GetComponent<PlayerMovement>();
        inv = (InventoryController)playerInv.GetComponent<InventoryController>();
        if (!GameMgr.NewGame)
        {
            LoadObject();
        }
        else
        {
            SaveObject();
            GameMgr.NewGame = false;
        }
        playerState = playerMov.state;
        inState = inv.state;
        wrap = new PlayerWrapper(playerState, inState);
    }

    public void SaveObject()
    {
        lock (playerSaveLock)
        {
            string destination = path + "/player.dat";
            FileStream file;
            if (File.Exists(destination))
            {
                File.Delete(destination);
                file = File.OpenWrite(destination);
            }
            else
            {
                file = File.OpenWrite(destination);
            }

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, wrap);

            DateTime now = DateTime.Now;
            GameMgr.SetSlotDateTime(now);
            GameMgr.SetLastSlot();
            GameMgr.SetLastSlotScene(gameObject.scene.name);
        }
    }

    public void LoadObject()
    {
        lock (playerSaveLock)
        {
            string destination = path + "/player.dat";
            FileStream file;
            if (File.Exists(destination))
            {
                file = File.OpenRead(destination);
            }
            else
            {
                Debug.LogError("File not found");
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();

            PlayerWrapper ass = (PlayerWrapper)bf.Deserialize(file);

            playerMov.state = ass.playerState;
            inv.state = ass.inState;
        }
    }

    public void OnRequestSaveEvent()
    {
        SaveObject();
        AckPlayerSaveEvent.Invoke();
    }
}
