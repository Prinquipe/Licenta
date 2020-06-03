using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMgr : MonoBehaviour
{
    public static string savePath;

    public static bool loadGame = false;

    private static GameMgr _instance = null;

    private GameMgr()
    {
        
    }

    public static void setSavePath(int slot)
    {
        string temp = Application.streamingAssetsPath + "/slot" + slot.ToString();
        Debug.Log(temp);
        if(!Directory.Exists(temp))
        {
            Directory.CreateDirectory(temp);
            savePath = temp;
        }
        else
        {
            savePath = temp;
        }
    }

    public static GameMgr getInstance()
    {
        if(_instance == null)
        {
            _instance = new GameMgr();
        }
        return _instance;
    }
}
