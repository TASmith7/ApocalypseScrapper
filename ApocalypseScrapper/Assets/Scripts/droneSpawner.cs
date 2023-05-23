using System.Collections;
using UnityEngine;

public class droneSpawner : MonoBehaviour
{
    
    [SerializeField]  GameObject dronePrefab;
    [SerializeField] GameObject particleEffectPrefab;
    public bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        if(dronePrefab)
        {
            StartCoroutine(spawnEnemy(dronePrefab));
        }    
        
    }

    // Update is called once per frame
    public IEnumerator spawnEnemy(GameObject enemy)
    {
        GameObject particleClone = Instantiate(particleEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        GameObject newEnemy=Instantiate(enemy,particleClone.transform.position,Quaternion.identity);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
