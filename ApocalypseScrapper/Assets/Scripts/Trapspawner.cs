using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trapspawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int intervalTime;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int prefabMaxNum;
    [SerializeField] GameObject levelTrigger;
    [SerializeField] GameObject lockedOutMessage;
    [SerializeField] GameObject enemies;
    [SerializeField] TextMeshProUGUI enemiesRemaining;

    public List<GameObject> prefabList = new List<GameObject>();

    int prefabsSpawnCount;
    bool playerInRange;
    bool isSpawning;
    bool lockedDownMessageSent;

    public int testNumber;


    // Start is called before the first frame update
    void Start()
    {
        // gameManager.instance.updatGameGoal(prefabMaxNum);
        //levelTrigger.GetComponent<Collider>().enabled = false;

        //enemyCount.GetComponent<Text>().text = "Enemies Remaining: " + prefabList.Count;
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
            UpdateEnemiesRemaining();
            levelTrigger.SetActive(true);
            enemies.SetActive(false);
        }

        for (int i = 0; i < prefabList.Count; i++)
        {
            if (prefabList[i] == null)
            {
                prefabList.RemoveAt(i);
                // Debug.Log("Removed drone");
                // Debug.Log(prefabList.Count);
                UpdateEnemiesRemaining();
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
            StartCoroutine(LockDownMessage());
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

    IEnumerator LockDownMessage()
    {
        if (!lockedDownMessageSent)
        {
            lockedDownMessageSent = true;
            lockedOutMessage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            lockedOutMessage.SetActive(false);
            Destroy(lockedOutMessage);
        }
    }

    void UpdateEnemiesRemaining()
    {
        //int enemiesRemaining = totalEnemies - prefabList.Count;
        enemies.SetActive(true);
        enemiesRemaining.GetComponent<TextMeshProUGUI>().text = "Enemies Remaining: " + prefabList.Count;
    }
}
