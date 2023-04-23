using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timer;
    [SerializeField] float splashRad;

    void Start()
    {
        // destroying our bullet after a specified amount of time
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        // checking if the object that we collided with (other) has the IDamage script (i.e. is damageable)
        IDamage damageable = other.GetComponent<IDamage>();

        // if the object is damageable
        if (damageable != null)
        {
            // calculate the distance from the center of the splash attack to the object
            float distance = Vector3.Distance(transform.position, other.transform.position);

            // calculate the splash damage using the distance and splash radius
            float splashDamage = damage * (1 - distance / splashRad);

            // then take the calculated amount of splash damage
            damageable.TakeDamage(Mathf.RoundToInt(splashDamage));
        }

        // destroying the bullet if it hits something
        Destroy(gameObject);
    }
}