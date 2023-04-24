using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossSpit : MonoBehaviour
{
    [SerializeField] float splashRadius;
    [SerializeField] float damage;
    [SerializeField] float timer;
    [SerializeField] float splashDamage;

    void Start()
    {
        // destroying our bullet after a specified amount of time
        Destroy(gameObject, timer);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ExplosionDamage(transform.position, splashRadius);


        // destroying the bullet if it hits something
        Destroy(gameObject);
    }
    void ExplosionDamage(Vector3 center, float radius)
    {
        

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            
            //checking if the object that we collided with(other) has the IDamage script(i.e. is damageable)
                IDamage damageable = hitCollider.GetComponent<IDamage>();

            // if the object is damageable
            if (damageable != null)
            {
                //finds objects position relative to bullet
                float proximity = (hitCollider.transform.position - center).magnitude;
              //figures the damage applied
                splashDamage = splashDamage-(proximity / radius);
                damageable.TakeDamage(splashDamage);
            }


            
            
            
        }
    }
}