using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : FighterPlane
{

    // For Now Both PlayerController and EnemyController will have almost same field and function
    //Later we will make it common
    //V0.2 Dones 
    public List<GameObject> playerDrones;
    //For Shield
    public GameObject shieldEffect;

    void Start()
    {
        SetDefaultValues();
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
    public float touchMovementSensitivity=4;
    public float distanceByMouseMove;
    Vector3 lastMousePosition;
    public bool allowTouch;
    void Update()
    {
        //Dont want to run update when fighter plane is in Menu other than fan
        if (!GameManager.instance)
        {
            fan.transform.Rotate(Vector3.up * Time.deltaTime * fanRotationSpeed);
            return;
        }
        //For now we will only work on Keyboard control later we  can do it with drag or hidden joystic

        float inputRight = Input.GetAxis("Horizontal");
        float inputUp = Input.GetAxis("Vertical");
        //For Phone or Mouse 
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            if(!allowTouch)
            {
               // Debug.Log("Magnitude " + Input.mousePosition.normalized);
                distanceByMouseMove = Vector3.Distance(lastMousePosition, Input.mousePosition);
              //  Debug.Log(Screen.height * 0.02f);
              //  Debug.Log("Distance " + distanceByMouseMove);
                if(distanceByMouseMove>Screen.height*0.03f)
                {
                    allowTouch = true; 
                }
            }
           

            if (allowTouch)
            {
                inputRight = Input.GetAxisRaw("Mouse X") * touchMovementSensitivity;
                inputUp = Input.GetAxisRaw("Mouse Y") * touchMovementSensitivity;
            }
        }

        if (Input.GetMouseButtonUp(0)){
            allowTouch = false;
        }

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
    public Vector3 firePointOffset;//To Manage with particle
    //Bullet Prefab
    public PlayerFire bulletPrefab;
    //If use Bulllet 2x
    public PlayerFire bullet2XPrefab;
    public ParticleSystem particleFirePrefab;
    public Vector3 particleScale = new Vector3(2, 2, 2);

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
              
                if (particleFirePrefab)
                {
                    ParticleSystem firesParticle = GetParticles();
                    if (firesParticle == null)
                    {
                        firesParticle = Instantiate(particleFirePrefab);
                        particlesFires.Add(firesParticle);
                    }
                    firesParticle.gameObject.transform.SetParent(this.transform);
                    firesParticle.transform.position = firePoint[i].transform.position;
                    firesParticle.transform.localScale = particleScale;
                    firesParticle.gameObject.SetActive(true);
                    firesParticle.Play();
                    StartCoroutine(FireByDelay(i,0.35f));
                }
                else
                {
                    StartCoroutine(FireByDelay(i, 0));
                }


               // fire.gameObject.SetActive(true);
                //Calling fire function to add force
               // fire.Fire(fireSpeed);
            }
        }
    }
    public AudioSource fireShootSource;
    IEnumerator FireByDelay(int i,float delay)
    {
        if (StaticData.IsSoundOn() && fireShootSource)
        {
            fireShootSource.Play();
        }
        yield return new WaitForSeconds(delay);
        PlayerFire fire = GetFires();
        //If its null we will spawn and add to the pool list
        if (fire == null)
        {
            if (GameManager.instance.isPowerUp2X)
            {
                fire = Instantiate(bullet2XPrefab);
                fires2X.Add(fire);
            }
            else
            {
                fire = Instantiate(bulletPrefab);
                fires.Add(fire);
            }
        }
        fire.transform.position = firePoint[i].transform.position+firePoint[i].transform.forward * firePointOffset.z;
        fire.transform.rotation = firePoint[i].transform.rotation;

        fire.gameObject.SetActive(true);
        fire.Fire(fireSpeed);

    }
    public List<ParticleSystem> particlesFires = new List<ParticleSystem>();
    public ParticleSystem GetParticles()
    {
        for (int i = 0; i < particlesFires.Count; i++)
        {
            if (!particlesFires[i].gameObject.activeInHierarchy)
                return particlesFires[i];
        }
        return null;
    }
    // Pool list for Fires
    public List<PlayerFire> fires;
    public List<PlayerFire> fires2X;
    //Function will return bullets which can be reuse
    public PlayerFire GetFires()
    {
        if(GameManager.instance.isPowerUp2X)
        {
            for (int i = 0; i < fires2X.Count; i++)
            {
                if (!fires2X[i].gameObject.activeInHierarchy)
                {
                    return fires2X[i];
                }
            }
            return null;
            
        }
        for(int i=0;i<fires.Count;i++)
        {
            if(!fires[i].gameObject.activeInHierarchy)
            {
                return fires[i];
            }
        }
        return null;
    }
   
    //Shield
    public void EnableShield()
    {
        shieldEffect.SetActive(true);
    }



    public override void OnHit(float damageAmount)
    {
        if (healthPoint <= 0)
            return;
    
        if(shieldEffect.activeInHierarchy)
        {
            shieldEffect.SetActive(false);
            return;
        }

        base.OnHit(damageAmount);               
        //if Health Point is less or zero we will say the GameManager to Show Result Page        
        if (healthPoint <= 0)
        {
            gameObject.SetActive(false);
            //Saying GameManager to Show Result Page
            GameManager.instance.ShowLevelFailedByDelay();
            GameManager.instance.PlayExplosionPlayer();
            //Disabling the Drone
            foreach (GameObject obj in playerDrones)
                obj.SetActive(false);
        }
        else
        {
            if(StaticData.IsSoundOn())  GameManager.instance.crashPlayerAS.Play();

        }
        //As Player Get Damage we say GameManager to update health Bar
        GameManager.instance.PlayerGetDamage();

    }
    //
   
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collisiong "+collision.gameObject.name);
        if(collision.gameObject.tag=="Enemy")
        {
            OnHit(1);
            collision.gameObject.GetComponent<EnemyController>().OnHit();
        }
    }
    
}
