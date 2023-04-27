
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitSphere : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] float damage;
    


    public void Start()
    {
        Destroy(gameObject, .1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IDamage damageable = other.GetComponent<IDamage>();

            // if the object is damageable
            if (damageable != null)
            {

                // then take the specified amount of damage
                damageable.TakeDamage(damage);
            }

            
            
        }

        // destroying the bullet once blown up
        Destroy(gameObject);

    }
    


}
