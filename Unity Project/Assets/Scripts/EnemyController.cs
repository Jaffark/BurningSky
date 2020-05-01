using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    //Holds Max Health Point
    float maxHealthPoint;
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
    //Store rotation information as Every Fighter has different behaviour
    Vector3 defaultRotation;


    //For PowerUp
    public ePowerUpType powerUpToGive;
    void Start()
    {
        if (!isReady)
        {
            healthBg.transform.parent.gameObject.SetActive(false);
        }
        if (transform.parent &&  !transform.parent.GetComponent<BossController>())
        transform.SetParent(null);
        //Setting Max value from Health Point
        maxHealthPoint = healthPoint;
        //Setting rotation value
        defaultRotation = transform.eulerAngles;
        //This will Set the Default material using linked renderer
        SetDefaultMaterial();        
    }
    //Added Material from Renderer to Default Material
    void SetDefaultMaterial()
    {
        for (int i = 0; i < renderer.Count; i++)
        {
            defaultMaterial.Add(renderer[i].material);
        }
    }

    //To Reuse this AI we reset its healtPoint and Health Bar
    public void ResetData()
    {
        healthPoint = maxHealthPoint;
        transform.eulerAngles = defaultRotation;
        ResetFlash();
        UpdateHealthBar();
        
    }


   

    // Update is called once per frame
    //Speed with which it move
    public float forwardSpeed;
    void Update()
    {
        //We will use simple Translate function with forward direction and multiple with delta for smooth movement
        //transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
        //We don't want AI to go less then -5 because if it goes to -5 means it out of the screen
        if (transform.position.z < -5 || transform.position.z>400 || transform.position.x>25 || transform.position.x<-25) // defining few boundary to disable the plane
            gameObject.SetActive(false);

        //Handling Cannon
        if (cannon)
        {
            Vector3 pointToMove = GameManager.instance.player.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(pointToMove - cannon.transform.position);
            cannon.transform.rotation = Quaternion.RotateTowards(cannon.transform.rotation, targetRotation, cannonRotationSpeed * Time.deltaTime);
        }
        //This will handle Fire Function
        HandleFire();
    }
    //Delay between fire
    public float delayBetweenFire;
    //Last Fire Time
    public float lastFireTime;
    //public float relaxFire
    public float delayBetweenNFire;
    public int nFireAtATime;
    int currentFireCount;
    //Fire Speed
    public float fireSpeed=10;
    //AI may have single, multiple  or maynot have Fire Point
    public List<Transform> firePoint;
    public Vector3 firePointOffset;
    //Bullet Prefab
    public PlayerFire bulletPrefab;
    public ParticleSystem particleFirePrefab;
    public Vector3 particleScale = new Vector3(2, 2, 2);
    //making to true bullet will target the player
    public GameObject cannon;
    public float cannonRotationSpeed=25;
    public bool fireToPlayer;
    
    void HandleFire()
    {
        if (!isReady)
            return;
        //We will compare the last fire time with current time in order to give delay between firing
        if (currentFireCount>=nFireAtATime && nFireAtATime>0)
        {
            if(Time.time>=lastFireTime+delayBetweenNFire)
            {
                currentFireCount = 0;
            }
            return;
        }
        if (Time.time >= lastFireTime + delayBetweenFire && transform.position.z>-5f) //If the AI goes below will will stop firing
        {
            //Setting the fire time to current time
            lastFireTime = Time.time;
           
            currentFireCount++;
            for (int i = 0; i < firePoint.Count; i++)
            {
                //Trying to get Fire From Pool list               
                if (particleFirePrefab)
                {
                    ParticleSystem firesParticle = GetParticles();
                    if (firesParticle == null)
                    {
                        firesParticle = Instantiate(particleFirePrefab);
                        particlesFires.Add(firesParticle);
                    }
                    firesParticle.gameObject.transform.SetParent(firePoint[i].transform);
                    firesParticle.transform.position = firePoint[i].transform.position;
                    firesParticle.transform.localScale = particleScale;
                    firesParticle.gameObject.SetActive(true);
                    firesParticle.Play();                    
                    StartCoroutine(FireByDelay(i,0.35f));
                }
                else
                {
                    StartCoroutine(FireByDelay(i,0));
                    
                }

                //fire.gameObject.SetActive(true);
                //fire.Fire(fireSpeed);
            }
        }
    }
    IEnumerator FireByDelay(int i,float delay)
    {
       
        yield return new WaitForSeconds(delay);
        PlayerFire fire = GetFires();
        //If fire is null we will spawn  from prefab
        if (fire == null)
        {
            fire = Instantiate(bulletPrefab);
            fires.Add(fire);
        }
        fire.transform.position = firePoint[i].transform.position+ firePoint[i].transform.forward*firePointOffset.z;
        if (fireToPlayer && cannon==null)
            firePoint[i].transform.LookAt(GameManager.instance.player.transform);
        fire.transform.rotation = firePoint[i].transform.rotation;
        fire.gameObject.SetActive(true);
        fire.Fire(fireSpeed);
        
    }
    public List<ParticleSystem> particlesFires = new List<ParticleSystem>();
    public ParticleSystem GetParticles()
    {
        for(int i=0;i< particlesFires.Count;i++)
        {
            if (!particlesFires[i].gameObject.activeInHierarchy)
                return particlesFires[i];
        }
        return null;
    }
    public List<PlayerFire> fires;
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

  

    public GameObject healthBg;
    public Vector3 currentHealthScale;
    void UpdateHealthBar()
    {
        currentHealthScale.x = healthPoint / maxHealthPoint;
        healthBg.transform.localScale = currentHealthScale;
    }
    //When a player bullet hit we reduce the health point by 1
    public bool isReady=true;
    //If Its not ready it will not get damage and even can't shoot
    public void MakeItReady()
    {
        isReady = true;
        healthBg.transform.parent.gameObject.SetActive(true);

    }
    public void OnHit(float damageAmount=1)
    {
        if (!isReady)
            return;
        if (healthPoint <= 0)
            return;
        //Debug.Log("Hit by "+damageAmount);
        healthPoint-=damageAmount;
        //InOrder to Give Flash effect we loop with renderer list and assign the flash material
        for(int i = 0; i < renderer.Count; i++)
        {
            renderer[i].material = flash;
        }
        //Update the Health Bar
        UpdateHealthBar();
        //Call the function to reset renderer material
        Invoke("ResetFlash", flashDelay);
        //if Health Point is less or zero we will do some function by GameManager
        //Like score
        if(healthPoint<=0)
        {           
            //Calling Function to do some work like score
            GameManager.instance.FighterPlanDestroy();
            if (powerUpToGive!=ePowerUpType.none)
            {
                GameManager.instance.SpawnPowerUpAt(gameObject,powerUpToGive);
            }
            //Seting active to false
            LoadExplosion();
            gameObject.SetActive(false);
            if (transform.parent && transform.parent.GetComponent<BossController>())
                transform.parent.GetComponent<BossController>().CallBackWeaponDestoryed();
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
    //Effect 
    #region Effect
    public Vector3 explosionEffectScale=new Vector3(1,1,1);
     void LoadExplosion()
    {
        GameObject explosionEffect = GameManager.instance.GetExplosionEffect();
        if (explosionEffect == null)
        {
            explosionEffect = Instantiate(GameManager.instance.explosionEffect);
            explosionEffect.transform.position = transform.position;
            GameManager.instance.DisableGameObjWithDelay(explosionEffect, 5);
            explosionEffect.transform.localScale = explosionEffectScale;
            explosionEffect.SetActive(true);
        }
    }

    #endregion
}
