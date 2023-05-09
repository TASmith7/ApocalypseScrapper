
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    
    [SerializeField] GameObject splashBall;


   

    private void OnCollisionEnter(Collision other)
    {
        
        //instantiate SPlash ball
        Instantiate(splashBall, transform.position, transform.rotation);
        splashBall.transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 2, Time.deltaTime * 10);
        // destroying the bullet once blown up
        Destroy(gameObject);
        
    }
    

   
}