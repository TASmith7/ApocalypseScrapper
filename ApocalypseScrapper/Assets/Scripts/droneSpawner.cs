using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneSpawner : MonoBehaviour
{
    
    [SerializeField]  GameObject dronePrefab;
    // Start is called before the first frame update
    void Start()
    {
        if(dronePrefab)
        {
            StartCoroutine(spawnEnemy(dronePrefab));
        }    
        
    }

    // Update is called once per frame
    private IEnumerator spawnEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(1);
        GameObject newEnemy=Instantiate(enemy,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
    }
}
