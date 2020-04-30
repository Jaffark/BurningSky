using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // For Now Both PlayerController and EnemyController will have almost same field and function
    //Later we will make it common
    //V0.2 Dones 
    public List<GameObject> playerDrones;
    //Holds Max Health Point
    public  float maxHealthPoint;
    //Holds Current Health Point
    public float healthPoint;

    //We will link the renderer in order to give flash effect if player bullet hit
    public List<Renderer> renderer;
    //we will store the Materials information in order to reset back to the default material once the flash is done
    public List<Material> defaultMaterial;
    //Flash material , We will set this material when it get hit by player 
    public Material flash;
    //Delay till the flash Should show after this delay default Materials will assign
    public float flashDelay = 0.1f;
    //For Shield
    public GameObject shieldEffect;

    void Start()
    {
        //Setting Max value from Health Point
        maxHealthPoint = healthPoint;
        //This will Set the Default material using linked renderer
        SetDefaultMaterial();
    }

    //In order to give Player movement a feel we just tilt the Player base on Horizontal
    public float tiltAngle;
    //Smooth by which it will tilt
    public float smooth = 2;
    //Instead of tilting the main object we will tilt the body
    public GameObject fighterBody;
    //Speed with with it will move
    public float movementsSpeed = 0.25f;
    //We will define the boundary in order to avoid going off the screen
    public Vector2 boundaryX;
    public Vector2 boundaryZ;
    //We will rotate the fan (Later we can add the animation instead of doing with code)
    public Transform fan;
    //Rotation speed with which a fan should rotate
    public float fanRotationSpeed;
    // Update is called once per frame
    void Update()
    {
        //For now we will only work on Keyboard control later we  can do it with drag or hidden joystic
        float inputRight = Input.GetAxis("Horizontal");
        float inputUp = Input.GetAxis("Vertical");

        float tiltAroundZ = inputRight * tiltAngle;      

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, 0, 0);

        // Dampen towards the target rotation
        //We will only tilt and move the Player if its within boundary
        if (transform.position.x > boundaryX.x && inputRight < 0 || transform.position.x < boundaryX.y && inputRight>0)
        {
            transform.Translate(Vector3.right * inputRight * movementsSpeed);
            target = Quaternion.Euler(0, 0, tiltAroundZ);
        }        
        if(transform.position.z>boundaryZ.x && inputUp<0 || transform.position.z<boundaryZ.y && inputUp > 0)
        {
            transform.Translate(Vector3.forward * inputUp * movementsSpeed);
            
        }
        //Rotating Player Body with Slerp
        fighterBody.transform.rotation = Quaternion.Slerp(fighterBody.transform.rotation, target, Time.deltaTime * smooth);
        //Rotating Fan using Rotate
        fan.transform.Rotate(Vector3.up * Time.deltaTime * fanRotationSpeed);
        //Handling Fire
        HandleFire();

    }
    //Hold Delay Value between Fire
    public float delayBetweenFire;
    //Last Fire Time
    public float lastFireTime;
    //Fire Speed
    public float fireSpeed=10;
    //A player can have multiple fires 
    public List<Transform> firePoint;
    //Bullet Prefab
    public PlayerFire bulletPrefab;

    void HandleFire()
    {
        //We will compare the last fire time with current time to give delay in bullet spawn
        if(Time.time>=lastFireTime+delayBetweenFire)
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
                fire.Fire(fireSpeed);
            }
        }
    }
    // Pool list for Fires
    public List<PlayerFire> fires;
    //Function will return bullets which can be reuse
    public PlayerFire GetFires()
    {
        for(int i=0;i<fires.Count;i++)
        {
            if(!fires[i].gameObject.activeInHierarchy)
            {
                return fires[i];
            }
        }
        return null;
    }

    //Added Material from Renderer to Default Material
    void SetDefaultMaterial()
    {
        for (int i = 0; i < renderer.Count; i++)
        {
            defaultMaterial.Add(renderer[i].material);
        }
    }
    //Reset function to change the material to defaults
    public void ResetFlash()
    {
        for (int i = 0; i < renderer.Count; i++)
        {
            renderer[i].material = defaultMaterial[i];
        }
    }
    //Shield
    public void EnableShield()
    {
        shieldEffect.SetActive(true);
    }



    public void OnHit()
    {
        if (healthPoint <= 0)
            return;
        if(shieldEffect.activeInHierarchy)
        {
            shieldEffect.SetActive(false);
            return;
        }
        healthPoint--;
        //InOrder to Give Flash effect we loop with renderer list and assign the flash material
        for (int i = 0; i < renderer.Count; i++)
        {
            renderer[i].material = flash;
        }

        //Call the function to reset renderer material
        Invoke("ResetFlash", flashDelay);
        //if Health Point is less or zero we will say the GameManager to Show Result Page
        
        if (healthPoint <= 0)
        {
            gameObject.SetActive(false);
            //Saying GameManager to Show Result Page
            GameManager.instance.ShowResultPage("Failed");
            //Disabling the Drone
            foreach (GameObject obj in playerDrones)
                obj.SetActive(false);


        }
        //As Player Get Damage we say GameManager to update health Bar
        GameManager.instance.PlayerGetDamage();

    }
    //
   
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collisiong "+collision.gameObject.name);
        if(collision.gameObject.tag=="Enemy")
        {
            OnHit();
            collision.gameObject.GetComponent<EnemyController>().OnHit();
        }
    }
    
}
