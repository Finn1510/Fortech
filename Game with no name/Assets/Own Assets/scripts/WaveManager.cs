using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] WaveScriptableObject[] Waves;
    [Space]
    [Header("References")]
    [SerializeField] GameObject CrazyEye;
    [SerializeField] GameObject Zombie;
    [SerializeField] TMP_Text WaveNumberText;
    [SerializeField] TMP_Text EnemyRemainingText;
    [Space]
    
    [Header("Parameters")]
    [SerializeField] float WorldBorderLeft = -130;
    [SerializeField] float WorldBorderRight = 130;
    
    [Header("Zombie")]
    [SerializeField] float ZombieSpawnHeight = 7;

    [Header("CrazyEye")]
    [SerializeField] float CrazyEyeSpawnHeight = 9;

    int currentWaveNumber;

    int WaveRemainingCrazyEyes;
    int WaveRemainingZombies;
    int WaveRemainingEnemies;
    public int lastCompletedWavenumber;

    public bool currentWaveFinished = false;
    public List<GameObject> EnemyPool = new List<GameObject>();

    Vector3 Spawnpos;
    // Start is called before the first frame update
    void Start()
    {
        Load();
        initWave();
    }

    public void NextWave()
    {
        lastCompletedWavenumber = currentWaveNumber;
        if(currentWaveFinished == true)
        {
            if (currentWaveNumber + 1 == Waves.Length)
            {
                Debug.Log("Player completed all Waves");
                return;
            }

            currentWaveNumber = currentWaveNumber + 1;
            currentWaveFinished = false;
            initWave();
        }
    }

    void initWave()
    {
        if(currentWaveFinished == false)
        {
            WaveNumberText.text = "Wave " + currentWaveNumber;

            WaveRemainingCrazyEyes = Waves[currentWaveNumber].CrazyEyeCount;
            WaveRemainingZombies = Waves[currentWaveNumber].ZombieCount;
            Debug.Log("Wave " + currentWaveNumber + " started");
        }
        else
        {
            WaveNumberText.text = "Wave " + currentWaveNumber;
            WaveRemainingCrazyEyes = 0;
            WaveRemainingZombies = 0;
        }

    }

    private void Update()
    {
        WaveRemainingEnemies = WaveRemainingCrazyEyes + WaveRemainingZombies + EnemyPool.Count;

        if (WaveRemainingEnemies != 1 && WaveRemainingEnemies != 0)
        {
            EnemyRemainingText.text = WaveRemainingEnemies.ToString() + " enemies left";
        }
        else if(WaveRemainingEnemies == 0)
        {
            EnemyRemainingText.text = "wave completed";
            currentWaveFinished = true;
        }
        else
        {
            EnemyRemainingText.text = WaveRemainingEnemies.ToString() + " enemy left";
        }

        if(WaveRemainingCrazyEyes > 0)
        {
            if(Random.Range(1,100) == 1)
            {
                //Spawn a CrazyEye and add it to the EnemyPool
                WaveRemainingCrazyEyes = WaveRemainingCrazyEyes - 1;
                EnemyPool.Add(SpawnCrazyEye());
                
            }
        } 
        
        if(WaveRemainingZombies > 0)
        {
            if(Random.Range(1,100) == 1)
            {
                //Spawn a Zombie and add it to the EnemyPool
                WaveRemainingZombies = WaveRemainingZombies - 1;
                EnemyPool.Add(SpawnZombie());
                
            }
        }

        //Check if the enemy in the list is dead and delete him from the list if thats the case
        foreach (GameObject go in EnemyPool)
        {
            if(go == null || go.tag == "Dead")
            {
                EnemyPool.Remove(go);
            }
        }



    }

    GameObject SpawnZombie()
    {
        // Zombie Spawn
        Spawnpos = new Vector3(Random.Range(WorldBorderLeft, WorldBorderRight), ZombieSpawnHeight, 0);
        GameObject ZombieGO = Instantiate(Zombie, Spawnpos, Quaternion.Euler(0, 0, -270));
        Debug.Log("Spawned Zombie");
        return ZombieGO;
    }

    GameObject SpawnCrazyEye()
    {
        // CrazyEye Spawn
        Spawnpos = new Vector3(Random.Range(WorldBorderLeft, WorldBorderRight), CrazyEyeSpawnHeight, 0);
        GameObject CrazyEyeGO = Instantiate(CrazyEye, Spawnpos, Quaternion.identity);
        Debug.Log("Spawned CrazyEye");
        return CrazyEyeGO;
    }

    public void Save()
    {
        ES3.Save<int>("currentWaveNumber", currentWaveNumber);
        ES3.Save<bool>("currentWaveFinished", currentWaveFinished);
        Debug.Log("WaveSaved");
    }

    void Load()
    {
        if (ES3.KeyExists("currentWaveNumber"))
        {
            currentWaveNumber = ES3.Load<int>("currentWaveNumber");
            currentWaveFinished = ES3.Load<bool>("currentWaveFinished");
        }
        else
        {
            currentWaveNumber = 0;
        }
    }
}
