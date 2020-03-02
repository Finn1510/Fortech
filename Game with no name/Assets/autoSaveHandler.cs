using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using HutongGames.PlayMaker;

public class autoSaveHandler : MonoBehaviour
{
    public string oursaveFile;
    ES3AutoSaveMgr autoSaveMgr;

    private void Awake()
    {
        autoSaveMgr = GameObject.Find("Easy Save 3 Manager").GetComponent<ES3AutoSaveMgr>();
         
        oursaveFile = FsmVariables.GlobalVariables.GetFsmString("world savefile").Value;

        string ourSaveFilePath = Application.persistentDataPath + "/" + oursaveFile;
        Debug.Log("Path to our saveFile: " + ourSaveFilePath);

        autoSaveMgr.settings.path = oursaveFile;
    }

    
}
