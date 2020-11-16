using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int ShotsFired;
    public float DamageDealed;
    public int DiedCount;
    public int ZombiesKilled;
    public int CrazyEyesKilled;

    void Save()
    {
        ES3.Save<int>("ShotsFired", ShotsFired);
        ES3.Save<float>("DamageDealed", DamageDealed);
        ES3.Save<int>("DiedCount", DiedCount);
        ES3.Save<int>("ZombiesKilled", ZombiesKilled);
        ES3.Save<int>("CrazyEyesKilled", CrazyEyesKilled);

    } 

    void Load()
    {
        ShotsFired = ES3.Load<int>("ShotsFired");
        DamageDealed = ES3.Load<float>("DamageDealed");
        DiedCount = ES3.Load<int>("DiedCount");
        ZombiesKilled = ES3.Load<int>("ZombiesKilled");
        CrazyEyesKilled = ES3.Load<int>("CrazyEyesKilled");
    }

    private void Awake()
    {
        //checking if this is our first startup
        if(ES3.KeyExists("ShotsFired") == true)
        {
            Load();
        }
        
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
