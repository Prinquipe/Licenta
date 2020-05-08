using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using System.Security.Cryptography;

public class PlayerSave: MonoBehaviour,Saveable
{
    public PlayerWrapper wrap;

    public GameObject player;

    private PlayerState playerState;

    private InventoryState inState;

    private string path;

    [Header("Player Event")]
    [Space]

    public UnityEvent AckPlayerSaveEvent;

    void Awake()
    {
        if(AckPlayerSaveEvent == null)
        {
            AckPlayerSaveEvent = new UnityEvent();
        }
        GameMgr.setSavePath(1);
        path = GameMgr.savePath;
        LoadObject();
        PlayerMovement pl =(PlayerMovement)player.GetComponent<PlayerMovement>();
        playerState = pl.state;
        inState = pl.invController.state;
        wrap = new PlayerWrapper(playerState,inState);

    }

    public void SaveObject()
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
        Debug.Log(wrap.inState.BronzeKey);
        bf.Serialize(file, wrap);
    }

    public void LoadObject()
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


        PlayerMovement pl = (PlayerMovement)player.GetComponent<PlayerMovement>();
        pl.state = ass.playerState;
        Debug.Log(ass.inState.BronzeKey);
        pl.invController.state = ass.inState;
    }

    public void OnRequestSaveEvent()
    {
        SaveObject();
        AckPlayerSaveEvent.Invoke();
    }
}
