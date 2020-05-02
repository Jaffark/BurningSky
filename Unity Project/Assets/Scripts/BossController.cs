using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public List<EnemyController> weapons1 = new List<EnemyController>();
    public List<EnemyController> weapons2 = new List<EnemyController>();
    public void CallBackWeaponDestoryed()
    {
        bool isAnyWeaponAvailable = false;
        for(int i=0;i<weapons1.Count;i++)
        {
            if(weapons1[i].gameObject.activeInHierarchy)
            {
                isAnyWeaponAvailable = true;
                return;
            }
        }
        if(!isAnyWeaponAvailable)
        {
            for(int i=0;i<weapons2.Count;i++)
            {
                if(weapons2[i].healthPoint>0)
                {
                    if (!weapons2[i].isReady)
                    {
                        weapons2[i].MakeItReady();
                    }
                    isAnyWeaponAvailable = true;
                }
            }
        }
        if(!isAnyWeaponAvailable)
        {
            gameObject.SetActive(false);
            GameManager.instance.BossKilled();
            //Debug.Log("Level Cleared");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
