using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : FighterPlane
{
   
 
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
        SetDefaultValues();
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
        if (!isReady || transform.position.z > 14f) //Dont want to fire when AI is out of the screen
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
    List<ParticleSystem> particlesFires = new List<ParticleSystem>();
    public ParticleSystem GetParticles()
    {
        for(int i=0;i< particlesFires.Count;i++)
        {
            if (!particlesFires[i].gameObject.activeInHierarchy)
                return particlesFires[i];
        }
        return null;
    }
    List<PlayerFire> fires=new List<PlayerFire>();
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
        if(GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = true;
        }

    }
    public override void OnHit(float damageAmount=1)
    {
       
        if (!isReady)
            return;
        if (healthPoint <= 0)
            return;
        base.OnHit(damageAmount);
        //Update the Health Bar
        UpdateHealthBar();
        //Call the function to reset renderer material
       
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
   
    //Effect 
    #region Effect
    public Vector3 explosionEffectScale=new Vector3(1,1,1);
    //public AudioSource explosionAS;
     void LoadExplosion()
    {
        GameObject explosionEffect = GameManager.instance.GetExplosionEffect();
        if (explosionEffect == null)
        {
            explosionEffect = Instantiate(GameManager.instance.explosionEffect);
          
        }
        else
        {
            GameManager.instance.explosionList.Add(explosionEffect);
        }
        if(StaticData.IsSoundOn())
        {

          //  Debug.Log("Explosion Play");
            //The better way to do without calling the GetComponent 
            if (explosionEffect.GetComponent<AudioSource>())
                explosionEffect.GetComponent<AudioSource>().Play();
            
        }
            explosionEffect.transform.position = transform.position;
            GameManager.instance.DisableGameObjWithDelay(explosionEffect, 5);
            explosionEffect.transform.localScale = explosionEffectScale;
            explosionEffect.SetActive(true);
       
    }

    #endregion
}
