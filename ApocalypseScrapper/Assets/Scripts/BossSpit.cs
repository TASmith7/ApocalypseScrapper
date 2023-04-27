
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] GameObject splashBall;
    

    

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(explodeDelay());
        //instantiate SPlash ball
        Instantiate(splashBall,transform.position,transform.rotation);

        // destroying the bulletonce blown up
        Destroy(gameObject);
        
    }
    IEnumerator explodeDelay()
    {
        yield return new WaitForSeconds(3);
    }

   
}