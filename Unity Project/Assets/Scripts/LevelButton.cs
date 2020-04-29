using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CheckLock();
    }
    public GameObject lockObj, unlockObj;
    void CheckLock()
    {
        lockObj.SetActive(false);
        unlockObj.SetActive(false);
        if(StaticData.IsLevelUnlock(levelNo))
        {
            unlockObj.SetActive(true);
        }
        else
        {
            lockObj.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int levelNo;
    public void OnClick()
    {
        Menu.instance.OnLevelClick(levelNo);
    }
}
