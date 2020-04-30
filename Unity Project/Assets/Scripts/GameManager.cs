using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    //Defining Static  for GameManager
    public static GameManager instance;
    
    private void Awake()
    {

      
        instance = this;
    }
    void Start()
    {
        //Loading Level Base on Selection from Menu
        SetLevel();
    }
    //Level Base not Random
    //List Of Level 
    public List<GameObject> levels;
    void SetLevel()
    {
        //Enabling selected level 
        levels[StaticData.GetLevel()].SetActive(true);
    }
    //Power Up and Coins prefab
    public GameObject shieldPrefab;
    //Linking Player as Common in order to use in some suitation
    public PlayerController player;
    //InOrder to show health bar we use Image with ImageType filled
    public Image healthBar;
    // then Base on Health Point and Max Health Point we will set the Fill Amount
    void UpdateHealth()
    {
        healthBar.fillAmount = player.healthPoint / player.maxHealthPoint;
    }
    //Colors to use for Health Bar (Defaullt color and Flash)
    public Color healthBarFlashColor, healthBarDefaultColor;
    //When a Player get Damage we call this function to do the following
    //Flash the Health bar (Just Flash Effect)
    //Update health bar;
    public void PlayerGetDamage()
    {
        //Setting the Health Bar Image color to white 
        healthBar.color = healthBarFlashColor;
        //Reseting the Health Bar color to Default with some dela
        Invoke("ResetHealthBarColor", 0.1f);
        //Updating the fillAmount of Health Bar
        UpdateHealth();
    }
    //Reset to Color to Deafult Color after 
    //This willl call with delay using Invoke
    void ResetHealthBarColor()
    {
        healthBar.color = healthBarDefaultColor;
    }
    // Update is called once per frame
    public bool startSpawning;
    bool useRandomSpawn;
    void Update()
    {
        if (!startSpawning)
            return;
        if(useRandomSpawn)
        SpawnAI();
    }


    #region SpawnLogic
    /// <summary>
    /// V0.1
    /// For this Version we will first set the basic and Ramdom Spawn System 
    /// We have 4 Fighters AI as aiFighet1...4
    /// Here we pool the fighters planes to use it laters using Function GetFighterPlane with particular Fighter index
    /// 
    /// </summary>
    // Fighter Prefabs
    public GameObject aiFighter1,aiFighter2,aiFighter3,aiFighter4;
    
    // Spawn Style(Enum) to define spawn style
    public spawnStyle currentSpawnSytle;
    //Pooling List of AI Fighters
    public List<EnemyController> listOfFighterPlane1,listOfFighterPlane2,listOfFighterPlane3,listOfFighterPlane4 = new List<EnemyController>();
    //Function that will return the AI Fighter base on passed FighterId
    //if the Fighter AI is Disable it means we can reuse it
   
    public GameObject GetFighterPlane(int fighterId)
    {
     //Base on fighterId we will try to find out the disabled Fighter and return it
        switch (fighterId)
        {
            case 1:
                for (int i = 0; i < listOfFighterPlane1.Count; i++)
                {
                    if (!listOfFighterPlane1[i].gameObject.activeInHierarchy)
                    {
                        listOfFighterPlane1[i].ResetData();
                        return listOfFighterPlane1[i].gameObject;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < listOfFighterPlane2.Count; i++)
                {
                    if (!listOfFighterPlane2[i].gameObject.activeInHierarchy)
                    {
                        listOfFighterPlane2[i].ResetData();
                        return listOfFighterPlane2[i].gameObject;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < listOfFighterPlane3.Count; i++)
                {
                    if (!listOfFighterPlane3[i].gameObject.activeInHierarchy)
                    {
                        listOfFighterPlane3[i].ResetData();
                        return listOfFighterPlane3[i].gameObject;
                    }
                }
                break;
            case 4:
                for (int i = 0; i < listOfFighterPlane4.Count; i++)
                {
                    if (!listOfFighterPlane4[i].gameObject.activeInHierarchy)
                    {
                        listOfFighterPlane4[i].ResetData();
                        return listOfFighterPlane4[i].gameObject;
                    }
                }
                    break;
        }
        return null;
    }
    //Temperary Gameobject to spawn Fighter AI
    GameObject newAi;
    //This will store last spawn fighter gameobject detail
  
    public GameObject lastSpawnFighterPlane;
    public float lastSpawnTime;
    public float delayBetweenSpawn;

    void SpawnAI()
    {
        //If Player is not active we will not spawn the player 
        if (!player.gameObject.activeInHierarchy)
            return;
        //Inorder to give delay between spawn we compare the last spawn time with current time
        if(Time.time>lastSpawnTime+delayBetweenSpawn)
        {
            //Just to Random spawn Style 
            //The Spawn Style will define whether to spawn only 1,multiple and rightleft(We will define rightleft in v0.2).
            int random = Random.Range(1, 101);
            currentSpawnSytle = spawnStyle.single;
            //If random value is greater than 50 we set the spawn style to multiple
            if (random > 50)
                currentSpawnSytle = spawnStyle.multiple;
            //This expectedZ will hold the value of z value to spawn fighter at z 
            float expectedZ = 17;
            //In order to avoid more number of Fighter overlaping we set some offset between the Spawn Fighter 
            float offsetBetweenFighters = 3;
            //We will compare to make it more accurate offset
            if(lastSpawnFighterPlane && lastSpawnFighterPlane.transform.position.z>17- offsetBetweenFighters)
            {
                expectedZ = lastSpawnFighterPlane.transform.position.z + offsetBetweenFighters;
            }
            //If expected z spawn value is more we will ignore the spawn just to avoid more number of unnessary spawn in background
            if (expectedZ > 35)
                return;
            //Set the lastSpawnTime to current Time
            lastSpawnTime = Time.time;
            //Comparing the current Spawn Style and then loading base on that
            switch (currentSpawnSytle)
            {
                case spawnStyle.single:
                    //In Sigle spawn we will randomize the Fighters base on some values
                    int randomFighter = Random.Range(1, 101);
                    //If randomFighter is less then 30 we willl spawn the Fighter AI 1 and so on
                    if (randomFighter <= 30)
                    {
                        //Instead of spawning fighter every time we will check for disabled one using GetFighterPlane
                        newAi = GetFighterPlane(1);
                        //If its null we spawn 
                        if (newAi == null)
                        {
                            //Ai Fighter with Only Forward Be
                            newAi = Instantiate(aiFighter1);
                            listOfFighterPlane1.Add(newAi.GetComponent<EnemyController>());
                        }
                       
                    }
                    else if (randomFighter <= 60)
                    {
                        newAi = GetFighterPlane(2);
                        if (newAi == null)
                        {
                            newAi = Instantiate(aiFighter2);
                            listOfFighterPlane2.Add(newAi.GetComponent<EnemyController>());
                        }
                    }
                    else if (randomFighter <= 80)
                    {
                        newAi = GetFighterPlane(3);
                        if (newAi == null)
                        {
                            newAi = Instantiate(aiFighter3);
                            listOfFighterPlane3.Add(newAi.GetComponent<EnemyController>());
                        }
                    }
                    else
                    {
                        newAi = GetFighterPlane(4);
                        if (newAi == null)
                        {
                            newAi = Instantiate(aiFighter4);
                            listOfFighterPlane3.Add(newAi.GetComponent<EnemyController>());
                        }
                    }
                    //We will set the x position to random between -3.8f to 3.8f as we dont want to spawn out of Screen X
                    newAi.transform.position = new Vector3(Random.Range(-3.8f, 3.8f), 0, expectedZ);
                    
                    //Setting Fighter Ai active  to true
                    newAi.SetActive(true);
                    //Setting lastSpawnFighterPlane as this newAI
                    lastSpawnFighterPlane = newAi;
                    break;
                case spawnStyle.multiple:
                    //In this we will spawn multiple AI Fighter1
                    //Since we will spawn with some distance in z direction
                    float lastZVal = expectedZ;
                    //We define distance between two to make it come in sequence 
                    float distanceBetweenTwo = 3;
                    //We don't want sequence to come in different x value so we will define common random value for all Fighter AI
                    float lastXVal = Random.Range(-3.8f, 3.8f);
                    //V0.2 Added Ramdom Behaviour
                    int randomBehaviour =  Random.Range(1, 5); //1-4
                    for (int i = 0; i < 3; i++)
                    {
                        //We will try to get the Fighter AI from list
                        newAi = GetFighterPlane(1);
                        if(newAi==null) 
                        newAi  = Instantiate(aiFighter1);//V0.2 Giving variation in behaviour
                       
                     
                        FighterBehaviour behaviour = newAi.GetComponent<FighterBehaviour>();
                        switch(randomBehaviour)
                        {
                            case 1:
                                behaviour.fighterBehaviour = eFighterBehaviour.Straight;
                                break;
                            case 2:
                                behaviour.fighterBehaviour = eFighterBehaviour.GoLeft;
                                behaviour.pointToMove = new Vector3(-11, 0, 5);
                                behaviour.speed = 12f;
                                behaviour.behaviourWhenZ = Random.Range(6, 8);
                                break;
                            case 3:
                                behaviour.fighterBehaviour = eFighterBehaviour.GoRight;
                                behaviour.pointToMove = new Vector3(11, 0, 5);
                                behaviour.speed = 12f;
                                behaviour.behaviourWhenZ = Random.Range(6, 8);
                                break;
                            case 4:
                                behaviour.fighterBehaviour = eFighterBehaviour.ZigZag;
                                behaviour.speed = 0.5f;

                                break;
                        }
                        //We multiple z value with i inorder to get the difference in z value
                        newAi.transform.position = new Vector3(lastXVal, 0, lastZVal+(i*distanceBetweenTwo));                        
                        newAi.SetActive(true);
                        lastSpawnFighterPlane = newAi;
                    }
                    //Just to avoid more number of spawn which will make 
                    lastSpawnTime = delayBetweenSpawn * 3;
                    break;
            }
        }
    }
    #endregion

    #region PowerUp
    public void PowerUpPickUp(string powerUpId)
    {
        switch(powerUpId)
        {
            case "Shield":
                player.EnableShield();
                break;
        }
    }
    //If AI fighter Can give any power then we call this function
    public void SpawnPowerUpAt(GameObject atObj,ePowerUpType powerUpType)
    {
        GameObject effectToLoad = null;
        switch(powerUpType)
        {
            case ePowerUpType.shield:
                effectToLoad = shieldPrefab;
                break;
                  
        }
        if (effectToLoad)
        {
            GameObject newPower = Instantiate(effectToLoad);
            newPower.transform.position = atObj.transform.position;
            newPower.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Result
    //Storing data (Best Score) using player pref
    //Static function to Get Best Score
    //Moving this static function to Static Class
    /*public static int GetBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }
    //Static function to Set Best Score in PlayerPref
    public static void SetBestScore(int val)
    {
        PlayerPrefs.SetInt("BestScore", val);
    }*/
    
    public TextMeshProUGUI scoreText, bestScoreTxt;
    int score;
    public void FighterPlanDestroy()
    {
        score++;
    }
    public void BossKilled()
    {
        StartCoroutine(ShowCompletePage());
    }
    //If there is no Boss Fight once we load the all AI Fighter from Level Manager 
    //We start checking whether there is any AI fighter avaialble or not inorder to call level complete page
    public void StartCheckingInLoopForLC()
    {
        StartCoroutine(CheckForLCForEveryDelayOf(1f));
    }
    IEnumerator CheckForLCForEveryDelayOf(float delay)
    {
        bool isAvailable = true;

        while (isAvailable)
        {
            EnemyController[] ai = GameObject.FindObjectsOfType<EnemyController>();
            if (ai.Length <= 0)
            {
                isAvailable = false;
            }
            yield return new WaitForSeconds(delay);
        }
       StartCoroutine(ShowCompletePage());
    }

    IEnumerator ShowCompletePage()
    {
        //Just giving delay for Level Complete page
        yield return new WaitForSeconds(2);
        ShowResultPage("Completed");
    }
    public GameObject failedPageContent, completePageContent;
    public void ShowResultPage(string resultType)
    {
        switch (resultType)
        {
            case "Failed":
                failedPageContent.SetActive(true);
                break;
            case "Completed":
                scoreText.text = score + "";
                if (score > StaticData.GetBestScore())
                {
                    StaticData.SetBestScore(score);
                    
                }
                if(score>StaticData.GetLevelBestScore(StaticData.GetLevel()))
                {
                    StaticData.SetLevelBestScore(score, StaticData.GetLevel());
                }
                bestScoreTxt.text = StaticData.GetBestScore() + "";
                completePageContent.SetActive(true);
                //Storing Unlock information so that we can blink the newly unlock level in Level Selection
                if(!StaticData.IsLevelUnlock(StaticData.GetLevel()+1))
                {
                    StaticData.lastLevelUnlock = StaticData.GetLevel() + 1;
                }
                StaticData.SetLevelUnlockTo("true", StaticData.GetLevel()+1);
                break;
        }
       
      //  resultPage.SetActive(true);
    }
    public void NextAtLevelCompleted()
    {
        //When You press Next button we will show level selection instead of Menu Page
        //So using this static variable we can know frome where the player has come from
        StaticData.comeFromPage = "LevelCompleted";
        failedPageContent.SetActive(false);
        Application.LoadLevel("Menu");
    }
    public void RetryAtFailed()
    {
        failedPageContent.SetActive(false);
        Application.LoadLevel(Application.loadedLevelName);
    }
    public void HomeAtFailed()
    {
        failedPageContent.SetActive(false);
        Application.LoadLevel("Menu");
    }
    #endregion;
    //Region For Pause Page
    #region  PausePage
    public GameObject pausePage;
    public void Paused()
    {
        Time.timeScale = 0;
        pausePage.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pausePage.SetActive(false);
    }
    public void HomeAtPause()
    {
        Time.timeScale = 1;
        pausePage.SetActive(false);
        Application.LoadLevel("Menu");
    }
    public void RetryAtPause()
    {
        Time.timeScale = 1;
        pausePage.SetActive(false);
        Application.LoadLevel(Application.loadedLevelName);
    }
    #endregion


}
public enum spawnStyle
{
    single,multiple,leftRight
}
public enum ePowerUpType
{
    none,shield
}