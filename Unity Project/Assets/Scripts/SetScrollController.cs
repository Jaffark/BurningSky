using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScrollController : MonoBehaviour
{
    // Start is called before the first frame update
    public static SetScrollController instance;     
    //Speed with which the set should scroll
    public float forwardSpeed = 2;
    void Start()
    {
        instance = this;   
    }
   
    // Update is called once per frame
    void Update()
    {
       
        //From speed to slow effect at starting for the set scrolling 
        if(forwardSpeed<-0.1f)
        {
            forwardSpeed += 0.01f;
        }
        if(!GameManager.instance.startSpawning)
        {
            if (forwardSpeed >= -0.1f)
                GameManager.instance.startSpawning = true;
        }

        
    }
}
