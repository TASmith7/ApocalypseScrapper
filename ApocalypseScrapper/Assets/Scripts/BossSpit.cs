//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class BossSpit : MonoBehaviour
//{

//    [SerializeField] float timer;
//    [SerializeField] float splashDamage;

//    void Start()
//    {
//        // destroying our bullet after a specified amount of time
//        Destroy(gameObject, timer);

//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Spit hit+", other.gameObject);
//        if (other.CompareTag("Player"))
//        {

//            // checking if the object that we collided with (other) has the IDamage script (i.e. is damageable)
//            IDamage damageable = other.GetComponent<IDamage>();

//            // if the object is damageable
//            // then take the specified amount of damage
//            damageable?.TakeDamage(splashDamage);
//        }
//        // destroying the bullet if it hits something
//        Destroy(gameObject);

//    }

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] float splashDamage;
    [SerializeField] float splashRadius;

    void Start()
    {
        // destroying our bullet after a specified amount of time
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        // destroying the bullet if it hits something
        Destroy(gameObject);

        Debug.Log("Spit hit: " + other.gameObject.name);

        // check if the object that we collided with is damageable
        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null)
        {
            // apply damage to the target
            damageable.TakeDamage(splashDamage);

            // get all the colliders in the splash radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, splashRadius);

            foreach (Collider collider in colliders)
            {
                // check if the collider has the IDamage script and it is not the boss or its children
                if (collider.CompareTag("Player") || (collider.transform.root.CompareTag("Enemy") && collider.transform.root != transform.root))
                {
                    IDamage damageableCollider = collider.GetComponent<IDamage>();
                    if (damageableCollider != null)
                    {
                        // calculate the damage to apply based on the distance from the boss's attack
                        float distance = Vector3.Distance(transform.position, collider.transform.position);
                        float damage = splashDamage * (1 - distance / splashRadius);

                        // apply damage to the collider
                        damageableCollider.TakeDamage(damage);
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // draw a wire sphere to represent the splash radius in the Unity Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}