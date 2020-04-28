using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rig;
    Vector3 startDistance;
    void Start()
    {
        
    }
    //This willl reset the velocity and angular velocity if any
    //and then add force to this
    public void Fire(float speed)
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        startDistance = transform.position;
        rig.AddForce(transform.forward * Time.deltaTime * 10000*speed, ForceMode.Force);
    }
    // Update is called once per frame
    float dis;
    void Update()
    {

       // if the bulllet goes off the screen we will disable it
        if(transform.position.z>13.5f || transform.position.z<-10)
        {
            gameObject.SetActive(false);
        }
    }
    //Since we are using same script for both Enemy and player
    //We will compare it and do the function call which is almost same
    //Later we will use it differenly but for basic setup we will go with this
    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.tag =="Enemy")
        {
            collision.transform.root.GetComponent<EnemyController>().OnHit();
            gameObject.SetActive(false);
        }
       if (collision.gameObject.tag == "Player")
        {
            collision.transform.root.GetComponent<PlayerController>().OnHit();
            gameObject.SetActive(false);
        }
    }
}
