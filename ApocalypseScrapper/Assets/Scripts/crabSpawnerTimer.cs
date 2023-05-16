using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabSpawnerTimer : MonoBehaviour
{
    [SerializeField] crabSpawner[] crabSpawners;
    [SerializeField] public float timeBetweenSpawns;
    [SerializeField] GameObject crabPrefab;
    [SerializeField] public int numOfCrabsToSpawn;

    private float timeAtLastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if we have spawners in our crab spawners list
        if (crabSpawners.Length != 0)
        {
            // if the current time minus the time at last spawn is greater than our set threshold
            if (Time.fixedTime - timeAtLastSpawn > timeBetweenSpawns)
            {
                // set our time at last spawn to the time right now
                timeAtLastSpawn = Time.fixedTime;

                // spawn 5 crabs at random spawners in the level
                for (int i = 0; i < numOfCrabsToSpawn; i++)
                {
                    int spawnerChosen = Random.Range(0, crabSpawners.Length);
                    crabSpawners[spawnerChosen].StartCoroutine(crabSpawners[spawnerChosen].spawnEnemy(crabPrefab));
                }
            }
        }
    }
}
