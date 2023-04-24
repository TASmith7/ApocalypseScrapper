using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject crabPrefab;
    [SerializeField]
    private  GameObject dronePrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(crabPrefab));
        if(dronePrefab)
        {
            StartCoroutine(spawnEnemy(dronePrefab));
        }    
        
    }

    // Update is called once per frame
    private IEnumerator spawnEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(1);
        GameObject newEnemy=Instantiate(enemy,new Vector3(transform.position.x,transform.position.y,transform.position.y),Quaternion.identity);
    }
}
