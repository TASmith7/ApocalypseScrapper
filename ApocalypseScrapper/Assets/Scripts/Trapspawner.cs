using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapspawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int intervalTime;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int prefabMaxNum;
    [SerializeField] GameObject levelTrigger;

    public List<GameObject> prefabList = new List<GameObject>();

    int prefabsSpawnCount;
    bool playerInRange;
    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
       // gameManager.instance.updatGameGoal(prefabMaxNum);
       //levelTrigger.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && prefabsSpawnCount < prefabMaxNum)
        {
            StartCoroutine(spawn());
        }

        if (prefabList.Count == 0)
        {
            levelTrigger.SetActive(true);
        }

        for (int i = 0; i < prefabList.Count; i++)
        {
            if (prefabList[i] == null)
            {
                prefabList.RemoveAt(i);
               // Debug.Log("Removed drone");
               // Debug.Log(prefabList.Count);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            //Debug.Log("Player Enter Spawner");
            levelTrigger.SetActive(false);
            //Debug.Log("Exit Level OFF!!!");
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        GameObject prefabClone = Instantiate(prefab, spawnPos[Random.Range(0, spawnPos.Length)].position, prefab.transform.rotation);

        prefabList.Add(prefabClone);

        prefabsSpawnCount++;
        yield return new WaitForSeconds(intervalTime);
        isSpawning = false;
    }
}
