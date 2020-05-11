using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine.Events;

//class used to hold all the assets of a scene
//Enemies,player,chests, doors
public class AssetManager : MonoBehaviour, Saveable
{
    public string sceneName;

    private Item[] items;
    private Enemy[] enemies;
    private Door[] doors;

    private readonly object assetLock = new object();

    public string SavePath;

    [Header("Asset Event")]
    [Space]

    public UnityEvent AckAssetSaveEvent;


    void Awake()
    {
        if(AckAssetSaveEvent == null)
        {
            AckAssetSaveEvent = new UnityEvent();
        }
        items = (Item[])GameObject.FindObjectsOfType(typeof(Item));
        enemies = (Enemy[])GameObject.FindObjectsOfType(typeof(Enemy));
        doors = (Door[])GameObject.FindObjectsOfType(typeof(Door));
        GameMgr.setSavePath(1);
        sceneName = gameObject.scene.name;
        SavePath = GameMgr.savePath;
        LoadObject();
    }

    //Override
    public void SaveObject()
    {
        lock (assetLock)
        {
            AssetWrapper wrap = new AssetWrapper();
            string destination = SavePath + "/" + sceneName + ".dat";
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

            foreach (Item item in items)
            {
                wrap.Push(item.state);
            }

            foreach (Enemy item in enemies)
            {
                wrap.Push(item.state);
            }

            foreach (Door item in doors)
            {
                wrap.Push(item.state);
            }

            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(file, wrap);
        }
    }

    public void LoadObject()
    {
        lock (assetLock)
        {
            string destination = SavePath + "/" + sceneName + ".dat";
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenRead(destination);
            }
            else
            {
                Debug.Log("File not found. First Time in area");
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();

            AssetWrapper ass = (AssetWrapper)bf.Deserialize(file);
            UnWrap(ass);
        }
    }

    void UnWrap(AssetWrapper ass)
    {
        for (int i = 0; i <ass.items.Length;i++)
        {
            Debug.Log("ItemState");
            if (ass.items[i] != null)
            {
                foreach (Item item in items)
                {
                    if (item.MatchID(ass.items[i].m_ItemID))
                    {
                        item.ChangeState(ass.items[i]);
                        break;
                    }
                }
            }
            else
            {
                break;
            }
        }
        Debug.Log("EnemyState");

        for (int i = 0; i < ass.enemies.Length; i++)
        {
            if (ass.enemies[i] != null)
            {
                foreach (Enemy e in enemies)
                {
                    if (e.MatchID(ass.enemies[i].m_EnemyID))
                    {
                        e.ChangeState(ass.enemies[i]);
                        break;
                    }
                }
            }
            else
            {
                break;
            }

        }

        Debug.Log("DoorState");

        for (int i = 0; i<ass.doors.Length ; i++)
        {
            if (ass.doors[i] != null)
            {
                foreach (Door d in doors)
                {
                    if (d.MatchID(ass.doors[i].m_DoorID))
                    {
                        d.ChangeState(ass.doors[i]);
                        break;
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    public void OnRequestSaveEvent()
    {
        Debug.Log("Called");
        SaveObject();
        AckAssetSaveEvent.Invoke();
    }
}
