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

    void Start()
    {
        //Setting Max value from Health Point
        maxHealthPoint = healthPoint;
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
        ResetFlash();
        UpdateHealthBar();
        
    }


   

    // Update is called once per frame
    //Speed with which it move
    public float forwardSpeed;
    void Update()
    {
        //We will use simple Translate function with forward direction and multiple with delta for smooth movement
        transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
        //We don't want AI to go less then -5 because if it goes to -5 means it out of the screen
        if (transform.position.z < -5)
            gameObject.SetActive(false);
        //This will handle Fire Function
        HandleFire();
    }
    //Delay between fire
    public float delayBetweenFire;
    //Last Fire Time
    public float lastFireTime;
    //Fire Speed
    public float fireSpeed=10;
    //AI may have single, multiple  or maynot have Fire Point
    public List<Transform> firePoint;
    //Bullet Prefab
    public PlayerFire bulletPrefab;
    //making to true bullet will target the player
    public bool fireToPlayer;
    void HandleFire()
    {

        //We will compare the last fire time with current time in order to give delay between firing
        if (Time.time >= lastFireTime + delayBetweenFire && transform.position.z>-5f) //If the AI goes below will will stop firing
        {
            //Setting the fire time to current time
            lastFireTime = Time.time;
            for (int i = 0; i < firePoint.Count; i++)
            {
                //Trying to get Fire From Pool list
                PlayerFire fire = GetFires();
                //If fire is null we will spawn  from prefab
                if (fire == null)
                {
                    fire = Instantiate(bulletPrefab);
                    fires.Add(fire);
                }
                fire.transform.position = firePoint[i].transform.position;
                if (fireToPlayer)
                    firePoint[i].transform.LookAt(GameManager.instance.player.transform);
                fire.transform.rotation = firePoint[i].transform.rotation;
                fire.gameObject.SetActive(true);
                fire.Fire(fireSpeed);
            }
        }
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
    public void OnHit()
    {
        if (healthPoint <= 0)
            return;

        healthPoint--;
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
            //Seting active to false
            gameObject.SetActive(false);

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
}
