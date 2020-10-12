using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] DayNightC DayNightCycle;
    [Space]
    [SerializeField] GameObject Zombie;
    [SerializeField] GameObject CrazyEye;

    [Header("Parameters")]
    [SerializeField] float spawntickrate = 2;
    [SerializeField] float WorldBorderLeft = -130;
    [SerializeField] float WorldBorderRight = 130;

    [Header("Zombie")]
    [SerializeField] float ZombieSpawnChance = 5;
    [SerializeField] float ZombieSpawnHeight = 7;

    [Header("CrazyEye")]
    [SerializeField] float CrazyEyeSpawnChance = 3;
    [SerializeField] float CrazyEyeSpawnHeight = 9;

    Vector3 Spawnpos;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("EnemySpawn",0, spawntickrate);    
    }

    void SpawnZombie()
    {
        // Zombie Spawn
        if (Random.Range(0, ZombieSpawnChance) == 0)
        {
            Spawnpos = new Vector3(Random.Range(WorldBorderLeft, WorldBorderRight), ZombieSpawnHeight, 0);
            Instantiate(Zombie, Spawnpos, Quaternion.Euler(0,0,-270));

            Debug.Log("Spawned Zombie at " + Spawnpos + "and " + DayNightCycle.StateIndex);
        }
    } 

    void SpawnCrazyEye()
    {
        // CrazyEye Spawn
        if (Random.Range(0, CrazyEyeSpawnChance) == 0)
        {
            Spawnpos = new Vector3(Random.Range(WorldBorderLeft, WorldBorderRight), CrazyEyeSpawnHeight, 0);
            Instantiate(Zombie, Spawnpos, Quaternion.identity);

            Debug.Log("Spawned CrazyEye at " + Spawnpos + "and " + DayNightCycle.StateIndex);
        }
    }

    void EnemySpawn()
    {
        if (DayNightCycle.StateIndex == 1)
        {
            SpawnZombie();
            SpawnCrazyEye();
        }
        if (DayNightCycle.StateIndex == 2)
        {

        }
        if (DayNightCycle.StateIndex == 3)
        {

        }
        if (DayNightCycle.StateIndex == 4)
        {

        }
        if (DayNightCycle.StateIndex == 5)
        {

        }
        if (DayNightCycle.StateIndex == 6)
        {

        }
        if (DayNightCycle.StateIndex == 7)
        {

        }
        if (DayNightCycle.StateIndex == 8)
        {

        }
        if (DayNightCycle.StateIndex == 9)
        {
            SpawnZombie();
            SpawnCrazyEye();
        } 
        if (DayNightCycle.StateIndex == 10)
        {
            SpawnZombie();
            SpawnCrazyEye();
        }
    }
}
