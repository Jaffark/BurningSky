using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    // Start is called before the first frame update
    //Follow point will be the Player Fighter plane point
    public Transform followPoint;
    //Drone follow with speed of 
    public float followSpeed = 2;
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
        

        //Using Simple Lerp to move the Drone
        transform.position = Vector3.Lerp(transform.position, followPoint.transform.position, Time.deltaTime * followSpeed);
        HandleFire();
    }
    //Hold Delay Value between Fire
    public float delayBetweenFire;
    //Last Fire Time
    public float lastFireTime;
    //Fire Speed
    public float fireSpeed = 10;
    //A player can have multiple fires 
    public List<Transform> firePoint;
    //Bullet Prefab
    public PlayerFire bulletPrefab;


    //Drone
    public float rotationSpeedForBullet;
    public float bulletDelayToLookTarget;
    void HandleFire()
    {
        //We will compare the last fire time with current time to give delay in bullet spawn
        if (Time.time >= lastFireTime + delayBetweenFire)
        {
            //Setting last fire time to current time
            lastFireTime = Time.time;
            for (int i = 0; i < firePoint.Count; i++)
            {
                //Getting Fire Prefab from Pool list
                PlayerFire fire = GetFires();
                //If its null we will spawn and add to the pool list
                if (fire == null)
                {
                    fire = Instantiate(bulletPrefab);
                    fires.Add(fire);
                }
                fire.transform.position = firePoint[i].transform.position;
                fire.transform.rotation = firePoint[i].transform.rotation;
                fire.gameObject.SetActive(true);
                //Calling fire function to add force
                fire.SetDroneProperty(bulletDelayToLookTarget, rotationSpeedForBullet);
                fire.Fire(fireSpeed);

            }
        }
    }
    // Pool list for Fires
    public List<PlayerFire> fires;
    //Function will return bullets which can be reuse
    public PlayerFire GetFires()
    {
        for (int i = 0; i < fires.Count; i++)
        {
            if (!fires[i].gameObject.activeInHierarchy)
            {
                return fires[i];
            }
        }
        return null;
    }
}
