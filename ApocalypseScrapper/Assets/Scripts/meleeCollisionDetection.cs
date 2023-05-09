using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeCollisionDetection : MonoBehaviour
{
    [SerializeField] playerController playerScript;


    void OnTriggerEnter(Collider other)
    {
        // the actual hit box on our enemies is a capsule collider. We need this line because there are multiple colliders on our enemies
        // set up for detection radius
        if (other is CapsuleCollider)
        {
            if (other.tag == "Enemy" && playerScript.isMeleeing)
            {
                Debug.Log("Melee Landed");
                // checking if the object that we collided with (other) has the IDamage script (i.e. is damageable)
                IDamage damageable = other.GetComponent<IDamage>();

                // if the object is damageable
                if (damageable != null)
                {
                    Debug.Log("Melee Dealt Damage");
                    // then take the specified amount of damage
                    damageable.TakeDamage(playerScript.meleeDamage);
                }
            }
        }
    }
}
