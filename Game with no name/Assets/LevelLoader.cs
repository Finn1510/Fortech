using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using HutongGames.PlayMaker;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public int LevelIndex = 1;
    
    public string oursaveFile;
    ES3AutoSaveMgr autoSaveMgr;

    private void Start()
    {
        autoSaveMgr = GameObject.Find("Easy Save 3 Manager").GetComponent<ES3AutoSaveMgr>();
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameCorut(LevelIndex));
    } 

    IEnumerator LoadGameCorut(int levelIndex)
    {
        transition.SetTrigger("Start");
        oursaveFile = FsmVariables.GlobalVariables.GetFsmString("world savefile").Value;

        string ourSaveFilePath = Application.persistentDataPath + "/" + oursaveFile;
        Debug.Log("Path to our saveFile: " + ourSaveFilePath);

        if (ES3.FileExists(oursaveFile))
        {
            Debug.Log("Our save file already exists");

            autoSaveMgr.settings.path = oursaveFile;
        }
        else
        {
            File.WriteAllText(ourSaveFilePath, "{}");
            Debug.Log("Created our save File");

            autoSaveMgr.settings.path = oursaveFile;
    
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(1);

    }
}
