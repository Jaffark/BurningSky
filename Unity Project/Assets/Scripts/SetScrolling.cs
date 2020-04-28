using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScrolling : MonoBehaviour
{
    // Start is called before the first frame update    
    // Update is called once per frame
  //Temp variable to hold position information
    Vector3 nextPoint;    
    private void Update()    {
       
       
        nextPoint = transform.position;
        //As every set has common speed we take the speed from SetScrollController
        nextPoint.z += SetScrollController.instance.forwardSpeed;
        //Once it reached to value -50 or lower
        //We add the No of  Set * Set Size which is 75
        if (transform.position.z <= -50)
        {
            nextPoint.z+= 75;          
        }
        transform.position = nextPoint;
    }
   
}
