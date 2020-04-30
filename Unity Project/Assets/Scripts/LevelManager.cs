using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Levem Manager will manage the level
/// Instead of Loading random AI Fighter planes 
/// We will load a define AI fighter planes which will be enable base on time
/// </summary>

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLoadingContent());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //List of AI Fighters or any other content we want to set in the level
    [SerializeField]//In order to show in Inspector
    public List<LevelContent> listOfContent = new List<LevelContent>();
    //After finishing loading content we will check to clear the level
    public bool finishedLoadingContent;
    IEnumerator StartLoadingContent()
    {
        for(int i=0;i<listOfContent.Count;i++)
        {
            yield return new WaitForSeconds(listOfContent[i].delayToLoad);
            for (int j=0;j<listOfContent[i].contents.Count;j++)
            {
                listOfContent[i].contents[j].gameObject.SetActive(true);
            }
           
        }
        finishedLoadingContent = true;
        GameManager.instance.StartCheckingInLoopForLC();
    }


}
[System.Serializable]
public class LevelContent
{
    public List<GameObject> contents = new List<GameObject>();
    public int delayToLoad;
}
