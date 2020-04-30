using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public static Menu instance;
    //Camera Animation we are using Two different animation 
    //One for Menu and One for Level Selection (One more for Transition if it goes back from level selection to Menu)
    public Animator cameraAnim;
    string currentPage;
    void Start()
    {
        //To Set Resolution in PC to Portriat
        Screen.SetResolution(450, 750, true);
        if(StaticData.comeFromPage == "LevelCompleted")
        {
            PlayAtMenu();
        }
        StaticData.comeFromPage = "";
        instance = this;
        CheckHighScore();
    }

    public TextMeshProUGUI highScoreText;
    public void CheckHighScore()
    {
        highScoreText.text = "" + StaticData.GetBestScore();
    }
    public GameObject menuPage;
    void TweenMenu()
    {
        currentPage = "Menu";        
        menuPage.SetActive(true);
    }
    void DeTweenMenu()
    {
        menuPage.SetActive(false);
    }
    //Menu Play Button Press
    public void PlayAtMenu()
    {
        DeTweenMenu();
       Invoke("TweenLevelSelection",0.8f);
        SetCameraTo(1);
    }
    public void BackAtLevelSelection()
    {
        Invoke("TweenMenu",0.5f);
        DeTweenLevlSelection();
        SetCameraTo(0);


    }
    public GameObject levelSelectionPage;
    void TweenLevelSelection()
    {
        currentPage = "LevelSelection";       
        levelSelectionPage.SetActive(true);
    }
    void DeTweenLevlSelection()
    {
        levelSelectionPage.SetActive(false);
    }
    void SetCameraTo(int id)
    {
        cameraAnim.SetInteger("Id", id);
    }
    public void OnLevelClick(int level)
    {
        //Checking if level is lock or not
        //V0.2 Only have one level as we are not define Unlocking any level
        if(!StaticData.IsLevelUnlock(level))
        {
            Debug.Log("Level is Lock");
            return;
        }
        StaticData.SetLevel(level);
        DeTweenLevlSelection();
        StartCoroutine(LoadGame());
    }
    public GameObject loadingPage;
    //Showing loading page to load the game
    IEnumerator LoadGame()
    {
        SetCameraTo(0);
        yield return new WaitForSeconds(1f);
        loadingPage.SetActive(true);
        yield return new WaitForSeconds(1f);
        Application.LoadLevel("InGame");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
