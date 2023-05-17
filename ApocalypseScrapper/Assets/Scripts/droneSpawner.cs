using System.Collections;
using UnityEngine;

public class droneSpawner : MonoBehaviour
{
    
    [SerializeField]  GameObject dronePrefab;
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
        yield return new WaitForSeconds(1);
        GameObject newEnemy=Instantiate(enemy,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
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
