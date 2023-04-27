
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

        // destroying the bullet once blown up
        Destroy(gameObject);
        
    }
    

   
}