using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    string currentTime;
    [SerializeField] string LastTimeSaved;

    private void Start()
    {
        if (ES3.KeyExists("LastSaved"))
        {
            LastTimeSaved = ES3.Load<string>("LastSaved");
        }
        
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    void Save()
    {
        currentTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        ES3.Save<string>("LastSaved", currentTime);
    }
}
