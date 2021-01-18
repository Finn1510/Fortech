using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    string currentTime;
    [SerializeField] string LastTimeSaved;
    [SerializeField] int AutoSaveIntervallMinutes;
    [SerializeField] GameObject[] SaveGameObjects;

    int AutoSaveIntervallSeconds;

    private void Start()
    {
        if (ES3.KeyExists("LastSaved"))
        {
            LastTimeSaved = ES3.Load<string>("LastSaved");
        }

        AutoSaveIntervallSeconds = AutoSaveIntervallMinutes * 60;
        AutoSave(AutoSaveIntervallSeconds);
        
    }


    void AutoSave(int Delay)
    {
        SaveEverything();
        StartCoroutine(AutoSaveDelay(Delay));
    }

    IEnumerator AutoSaveDelay(int Delay)
    {
        yield return new WaitForSeconds(Delay);
        AutoSave(Delay);
    }
    
    void SaveEverything()
    {
        foreach (GameObject GO in SaveGameObjects)
        {
            GO.SendMessage("Save");
        }
    }

    public void Save()
    {
        currentTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        ES3.Save<string>("LastSaved", currentTime);
    }

    private void OnApplicationQuit()
    {
        SaveEverything();
    }


}
