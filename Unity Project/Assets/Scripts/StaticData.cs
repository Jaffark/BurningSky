using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static string comeFromPage;
    public static int lastLevelUnlock = 0;
    //Will return Best Score
    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }
    //Static function to Set Best Score in PlayerPref
    public static void SetBestScore(int val)
    {
        PlayerPrefs.SetInt("BestScore", val);
    }

    public static int GetLevelBestScore(int levelNo)
    {
        return PlayerPrefs.GetInt("BestScore" + levelNo);
    }
    //Static function to Set Best Score in PlayerPref
    public static void SetLevelBestScore(int val,int levelNo)
    {
        PlayerPrefs.SetInt("BestScore"+levelNo, val);
    }


    public static bool IsLevelUnlock(int levelNo)
    {
        if (PlayerPrefs.GetString("IsLevel"+levelNo+"Unlock") == "true" || levelNo==1)
            return true;
        return false;
    }
    public static void SetLevelUnlockTo(string val,int levelNo)
    {
        PlayerPrefs.SetString("IsLevel" + levelNo + "Unlock", val);
    }

    public static int GetLevel()
    {
        return PlayerPrefs.GetInt("Level", 1);
    }
    public static void SetLevel(int levelNo)
    {
        PlayerPrefs.SetInt("Level", levelNo);
    }

#if UNITY_EDITOR
    [MenuItem("GLU/Delete Pref")]
    public static void DeletePref()
    {
        PlayerPrefs.DeleteAll();
    }   
#endif
}
