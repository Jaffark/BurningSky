using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rig;
    Vector3 startDistance;
    public bool useTarget;
    public GameObject target;
    public int damageValue = 1;
    void Start()
    {
       
    }
    //This willl reset the velocity and angular velocity if any
    //and then add force to this
    public void Fire(float speed)
    {
        startDistance = transform.position;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
      
       

        //Specially for Drone Bullet //Rocket Like
        if (useTarget)
        {
            startTime = Time.time;
            forwardSpeed = speed;
            //Finding list of AI Fighter and we will randomly pick one
            EnemyController[] fighter= GameObject.FindObjectsOfType<EnemyController>();

            if(fighter.Length>0)
            {
                target = fighter[Random.Range(0, fighter.Length)].gameObject;
            }

        }
        //If there is no target we will add force
          if(target==null)
            rig.AddForce(transform.forward * Time.deltaTime * 10000*speed, ForceMode.Force);
        
    }
    //Done Will have different properties like Delay To Look a Target and Bullet Rotation
    //We can set that using this function
    public void SetDroneProperty(float delayToLookT, float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
        delayToLookTarget = delayToLookT;
    }
    // Update is called once per frame
    float dis;
    public float rotationSpeed = 12;
    public float forwardSpeed = 2;
    public float startTime;
    public float delayToLookTarget = 2;
    void Update()
    {
        //Behaviour for Drone like Bullet
        if (useTarget)
        {
            //We will disable if target is null or Disabled
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }
            //Simple Translate function to move forward
            transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
            //  transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * forwardSpeed);
            //To Give a extra feel we delay the looking target (Note: We spawn the bullet in reverse direction)
            if (Time.time > startTime + delayToLookTarget)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

        }
       

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
            //Debug.Log("Enemy "+collision.gameObject.name);
            if(collision.transform.GetComponent<EnemyController>())
            {
                collision.transform. GetComponent<EnemyController>().OnHit(damageValue);
                gameObject.SetActive(false);
                target = null; // Making to null as we are pooling the list
            }
            else if (collision.transform.parent && collision.transform.parent.GetComponent<EnemyController>())
            {
                collision.transform.parent.GetComponent<EnemyController>().OnHit(damageValue);
                gameObject.SetActive(false);
                target = null; // Making to null as we are pooling the list
            }
            else if (collision.transform.root.GetComponent<EnemyController>())
            {

                collision.transform.root.GetComponent<EnemyController>().OnHit(damageValue);
                gameObject.SetActive(false);
                target = null; // Making to null as we are pooling the list
            }
            
        }
       if (collision.gameObject.tag == "Player")
        {
            collision.transform.root.GetComponent<PlayerController>().OnHit(damageValue);
            gameObject.SetActive(false);
            target = null; // Making to null as we are pooling the list
        }
        if (collision.gameObject.tag == "Drone")
        {
            gameObject.SetActive(false);
            target = null;
        }
    }
}
