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
    

    public void LoadGame()
    {
        StartCoroutine(LoadGameCorut(LevelIndex));
    } 

    IEnumerator LoadGameCorut(int levelIndex)
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(1);

    }
}
