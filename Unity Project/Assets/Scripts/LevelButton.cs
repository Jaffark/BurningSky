using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CheckLock();
    }
    public GameObject lockObj, unlockObj;
    public TextMeshProUGUI levelBestText;
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
        if(StaticData.GetLevelBestScore(levelNo)>0)
        {
            levelBestText.text = "" + StaticData.GetLevelBestScore(levelNo);
            levelBestText.transform.parent.gameObject.SetActive(true);
        }
        if(StaticData.lastLevelUnlock==levelNo)
        {
            StaticData.lastLevelUnlock = -1;
            GetComponent<Animator>().enabled = true;
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
