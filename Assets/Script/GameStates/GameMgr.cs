using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public static string savePath;

    public static bool loadGame;

    public static bool noLastSlot;

    public static int lastSlot;

    public static int currentSlot;

    public static bool NewGame;

    public static Resolution resolution;

    public static float brightness;

    public static int fullscreenMode;

    public static string newGameScene = "TestScene";


    public static void setSavePath()
    {
        string temp = Application.streamingAssetsPath + "/slot" + currentSlot.ToString();
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

    public static void DeleteSlot(int i)
    {
        string temp = Application.streamingAssetsPath + "/slot" + i.ToString();
        if (Directory.Exists(temp))
        {
            Directory.Delete(temp);
        }
    }

    void Awake()
    {
        loadGame = false;
        NewGame = false;
        string  res;

        if(PlayerPrefs.HasKey("LastSlot"))
        {
            Debug.Log("Got lastslot");
            lastSlot = PlayerPrefs.GetInt("LastSlot");
            Debug.Log(lastSlot);
            noLastSlot = false;
        }
        else
        {
            noLastSlot = true;
        }


        if(PlayerPrefs.HasKey("Resolution"))
        {
            res = PlayerPrefs.GetString("Resolution");
        }
        else
        {
            resolution = Screen.currentResolution;
            res = resolution.ToString();
            PlayerPrefs.SetString("Resolution", resolution.ToString());
        }

        if (PlayerPrefs.HasKey("Brightness"))
        {
            brightness = PlayerPrefs.GetFloat("Brightness");
        }
        else
        {
            brightness = Screen.brightness;
            PlayerPrefs.SetFloat("Brightness", brightness);
        }

        if(PlayerPrefs.HasKey("FullScreenMode"))
        {
            fullscreenMode = PlayerPrefs.GetInt("FullScreenMode");
        }
        else
        {
            if(Screen.fullScreen)
            {
                fullscreenMode = 1;
            }
            else
            {
                fullscreenMode = 0;
            }

            PlayerPrefs.SetInt("FullScreenMode",fullscreenMode);
        }

        ChangeSettings(res);
    }

    private void ChangeSettings(string res)
    { 
        Debug.Log(res);

        string b = string.Empty;
        int[] val =new int[3];
        int j = 0;

        for(int i = 0; i < res.Length; i++)
        {
            if (Char.IsDigit(res[i]))
            {
                b += res[i];
            }
            else
            {
                Debug.Log(b);
                if (b.Length != 0)
                { 
                    val[j] = int.Parse(b);
                    b = string.Empty;
                    j++;
                }
                else
                {
                    continue;
                }
            }
        }

        Resolution newRes = new Resolution();
        newRes.width = val[0];
        newRes.height = val[1];
        newRes.refreshRate = Screen.currentResolution.refreshRate;

        if (brightness != Screen.brightness)
        {
            Screen.brightness = brightness;
        }

        if(Screen.fullScreen)
        {
            if(fullscreenMode == 0)
            {
                Screen.fullScreen = false;
                Screen.fullScreenMode = (FullScreenMode) 3;
            }
        }
        else
        {
            if(fullscreenMode == 1)
            {
                Screen.fullScreen = true;
                Screen.fullScreenMode = (FullScreenMode) 0;
            }
        }

        if (fullscreenMode == 1)
        {
            Screen.SetResolution(newRes.width, newRes.height,true);
        }
        else
        {
            Screen.SetResolution(newRes.width, newRes.height, false);
        }
    }

    public static void SetCurrentResolution(Resolution res)
    {
        resolution = res;
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
        PlayerPrefs.SetString("Resolution", resolution.ToString());
    }

    public static void SetBrightness(float bright)
    {
        brightness = bright;
        Screen.brightness = brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public static Resolution GetCurrentRes()
    {
        return resolution;
    }

    public static float GetBrightness()
    {
        return brightness;
    }

    public static bool GetLoadGame()
    {
        return loadGame;
    }

    public static void SetLoadGame()
    {
        loadGame = true;
    }

    public static void SetLastSlot()
    {
        Debug.Log("SetLastSlot"+currentSlot);
        PlayerPrefs.SetInt("LastSlot", currentSlot);
    }

    public static bool CheckIfEmpty(int i)
    {
        string temp = Application.streamingAssetsPath + "/slot" + i.ToString();
        if (Directory.Exists(temp))
        {
            return false;
        }
        return true;
    }

    public static string GetSlotDateTime(int i)
    {
        string slotTime = "SlotDateTime" + i;
        return PlayerPrefs.GetString(slotTime);
    }

    public static void SetSlotDateTime(DateTime time)
    {
        string slotTime = "SlotDateTime" + currentSlot;
        PlayerPrefs.SetString(slotTime, time.ToString());
    }

    public static string GetLastSlotScene(int i)
    {
        string slotScene = "LastSlotScene" + i;
        return PlayerPrefs.GetString(slotScene);
    }

    public static void SetLastSlotScene(string scene)
    {
        string slotScene = "LastSlotScene" + currentSlot;
        PlayerPrefs.SetString(slotScene,scene);
    }

    public static void SetCurrentSlot(int i)
    {
        currentSlot = i;
    }
}
