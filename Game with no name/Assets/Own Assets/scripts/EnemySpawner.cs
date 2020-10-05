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

    Vector3 Spawnpos;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnemySpawn", spawntickrate);    
    }

    void SpawnZombie()
    {
        // Zombie Spawn
        if (Random.Range(0, 10) == Random.Range(0, 10))
        {
            Spawnpos = new Vector3(Random.Range(WorldBorderLeft, WorldBorderRight), 7, 0);
            Instantiate(Zombie, Spawnpos, Quaternion.identity);

            Debug.Log("Spawned Zomnie at " + Spawnpos + "and " + DayNightCycle.StateIndex);
        }
    }

    void EnemySpawn()
    {
        if (DayNightCycle.StateIndex == 1)
        {
            SpawnZombie();
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
        } 
        if (DayNightCycle.StateIndex == 10)
        {
            SpawnZombie();
        }
    }
}
