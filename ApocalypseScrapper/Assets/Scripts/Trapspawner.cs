using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trapspawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float intervalTime;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int prefabMaxNum;
    [SerializeField] GameObject levelTrigger;
    [SerializeField] GameObject lockedOutMessage;
    [SerializeField] GameObject enemies;
    [SerializeField] TextMeshProUGUI enemiesRemaining;
    [SerializeField] GameObject particleEffectPrefab;

    public List<GameObject> prefabList = new List<GameObject>();

    int prefabsSpawnCount;
    bool playerInRange;
    bool isSpawning;
    bool lockedDownMessageSent;
    bool inLockdown;
    bool lockdownHasOccurred;

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

        if (prefabList.Count == 0 && inLockdown)
        {
            UpdateEnemiesRemaining();
            levelTrigger.SetActive(true);
            enemies.SetActive(false);

            inLockdown = false;
            levelAudioManager.instance.lockdownSirenAudioSource.Stop();
            levelAudioManager.instance.lockdownAudioSource.Stop();
            levelAudioManager.instance.lockdownSirenAudioSource.PlayOneShot(levelAudioManager.instance.lockdownShutdownSiren);
            levelAudioManager.instance.lockdownAudioSource.PlayOneShot(levelAudioManager.instance.lockdownDisengagedVoice);
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

        if (playerInRange && !levelAudioManager.instance.lockdownAudioSource.isPlaying && !levelAudioManager.instance.lockdownSirenAudioSource.isPlaying && inLockdown && !gameManager.instance.isPaused)
        {
            levelAudioManager.instance.lockdownAudioSource.Play();
            levelAudioManager.instance.lockdownSirenAudioSource.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !lockdownHasOccurred)
        {
            playerInRange = true;
            inLockdown = true;
            lockdownHasOccurred = true;

            //Debug.Log("Player Enter Spawner");
            levelTrigger.SetActive(false);
            StartCoroutine(LockDownMessage());
            //Debug.Log("Exit Level OFF!!!");
        }
    }
    IEnumerator spawnStall()
    {
        yield return new WaitForSeconds(.8f);
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        // will need to instatiate the particle effect first than spawn the drones or enemies to the location of the smoke
        GameObject particleClone = Instantiate(particleEffectPrefab, spawnPos[Random.Range(0, spawnPos.Length)].position, particleEffectPrefab.transform.rotation);
        StartCoroutine(spawnStall());

        GameObject prefabClone = Instantiate(prefab, particleClone.transform.position, prefab.transform.rotation);
        prefabList.Add(prefabClone);

        prefabsSpawnCount++;
        yield return new WaitForSeconds(intervalTime);
        isSpawning = false;
        Destroy(particleClone);
    }

    IEnumerator LockDownMessage()
    {
        if (!lockedDownMessageSent)
        {
            lockedDownMessageSent = true;
            lockedOutMessage.SetActive(true);
            yield return new WaitForSeconds(3.5f);
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
