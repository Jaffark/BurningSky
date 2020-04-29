using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
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
}
