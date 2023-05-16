using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneSpawnTimer : MonoBehaviour
{
    [SerializeField] droneSpawner[] droneSpawners;
    [SerializeField] public float timeBetweenSpawns;
    [SerializeField] GameObject dronePrefab;
    [SerializeField] public int numOfDronesToSpawn;

    private float timeAtLastSpawn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if we have spawners in our crab spawners list
        if (droneSpawners.Length != 0)
        {
            // if the current time minus the time at last spawn is greater than our set threshold
            if (Time.fixedTime - timeAtLastSpawn > timeBetweenSpawns)
            {
                // set our time at last spawn to the time right now
                timeAtLastSpawn = Time.fixedTime;

                // spawn 5 crabs at random spawners in the level
                for (int i = 0; i < numOfDronesToSpawn; i++)
                {
                    int spawnerChosen = Random.Range(0, droneSpawners.Length);
                    droneSpawners[spawnerChosen].StartCoroutine(droneSpawners[spawnerChosen].spawnEnemy(dronePrefab));
                }
            }
        }
    }
}
