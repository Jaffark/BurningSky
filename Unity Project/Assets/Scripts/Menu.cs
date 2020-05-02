using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public static Menu instance;
    //Camera Animation we are using Two different animation 
    //One for Menu and One for Level Selection (One more for Transition if it goes back from level selection to Menu)
    public Animator cameraAnim;
    string currentPage;
    public AudioSource bgAudioSource;
    void CheckBgSound()
    {
        if (bgAudioSource && StaticData.IsMusicOn())
        {
            bgAudioSource.volume = 0.65f;
            
        }
        else
        {
            bgAudioSource.volume = 0;
        }
    }
    void Start()
    {
        CheckSoundSprite();
        CheckMusicSprite();
        CheckBgSound();
        //To Set Resolution in PC to Portriat
        Screen.SetResolution(450, 750, true);
        if(StaticData.comeFromPage == "LevelCompleted")
        {
            PlayAtMenu();
        }
        StaticData.comeFromPage = "";
        instance = this;
        CheckHighScore();
        movementTSSlider.value = StaticData.GetMovementTouchSensitivity();
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
        ClickSound();
    }
    public void BackAtLevelSelection()
    {
        Invoke("TweenMenu",0.5f);
        DeTweenLevlSelection();
        SetCameraTo(0);
        ClickSound();

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
        ClickSound();
        if (!StaticData.IsLevelUnlock(level))
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
    public GameObject settingPage;
    public void ShowSettingsPage()
    {
        ClickSound();
        settingPage.SetActive(true);
    }
    public void HideSettingsPage()
    {
        ClickSound();
        settingPage.SetActive(false);
    }
    public Slider movementTSSlider;
    public void OnSliderMTSValueChange()
    {
        //Setting PlayerPref data so that it can be save through out the game(Till user delete data or uninstall the game)
        StaticData.SetMovemenetTouchSensitivity(movementTSSlider.value);
      
    }
    #region Sounds
    public Image soundImg, musicImg;
    public Sprite soundOnSprite, soundOffSprite, musicOnSprite, musicOffSprite;
    public void SwitchSound()
    {
        if(StaticData.IsSoundOn())
        {
            StaticData.SetSoundTo("false");
        }
        else
        {
            StaticData.SetSoundTo("true");
        }
        
        CheckSoundSprite();
        ClickSound();
    }
    void CheckSoundSprite()
    {
        if (StaticData.IsSoundOn())
        {
            soundImg.sprite = soundOnSprite;
        }
        else
        {
            soundImg.sprite = soundOffSprite;
        }
    }
    public void SwitchMusic()
    {
        if (StaticData.IsMusicOn())
        {
            StaticData.SetMusicTo("false");
        }
        else
        {
            StaticData.SetMusicTo("true");
        }

        CheckMusicSprite();
        CheckBgSound();
        ClickSound();
    }
    void CheckMusicSprite()
    {
        if(StaticData.IsMusicOn())
        {
            musicImg.sprite = musicOnSprite;
        }
        else
        {
            musicImg.sprite = musicOffSprite;
        }
    }
    public void ClickSound()
    {
        if (StaticData.IsSoundOn())
        {
            GameObject soundClicp = Instantiate(Resources.Load("Click"))as GameObject;
            soundClicp.transform.position = Camera.main.transform.position;
            soundClicp.SetActive(true);
        }
    }
    #endregion
}
