using System.Collections;
using UnityEngine;

public class bulletPlayer : MonoBehaviour
{
    [SerializeField] int damage=1;
    [SerializeField] float timer;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] playerController playerScript;

    void Start()
    {
        //pulling damage from playerController, which should have proper damge w/ upgrades
        damage = playerScript.shootDamage;
        // destroying our bullet after a specified amount of time
        switch(damage)
        {
            
            case 1:
                trailRenderer.startColor = Color.white;
                trailRenderer.endColor = Color.white;
                break;
            case 2:
                trailRenderer.startColor = Color.white;
                trailRenderer.endColor = Color.green;
                break;
            case 3:
                trailRenderer.startColor = Color.green;
                trailRenderer.endColor = Color.blue;
                break;
            case 4:
                trailRenderer.startColor = Color.blue;
                trailRenderer.endColor = Color.cyan;
                break;
            case 5:
                trailRenderer.startColor = Color.cyan;
                trailRenderer.endColor = Color.white;
                break;
            case 6:
                trailRenderer.startColor = Color.white;
                trailRenderer.endColor = Color.red;
                break;
            case 7:
                trailRenderer.startColor = Color.red;
                trailRenderer.endColor = Color.blue;
                break;
            case 8:
                trailRenderer.startColor = Color.blue;
                trailRenderer.endColor = Color.green;
                break;
            case 9:
                trailRenderer.startColor = Color.green;
                trailRenderer.endColor = Color.black;
                break;
            case 10:
                trailRenderer.startColor = Color.black;
                trailRenderer.endColor = Color.red;
                break;
            case 11:
                trailRenderer.startColor = Color.red;
                trailRenderer.endColor = Color.black;
                break;
            case 12:
                trailRenderer.startColor = Color.black;
                trailRenderer.endColor = Color.blue;
                break;
            case 13:
                trailRenderer.startColor = Color.blue;
                trailRenderer.endColor = Color.grey;
                break;
            case 14:
                trailRenderer.startColor = Color.grey;
                trailRenderer.endColor = Color.yellow;
                break;
            case 15:
                trailRenderer.startColor = Color.yellow;
                trailRenderer.endColor = Color.blue;
                break;
            case 16:
                trailRenderer.startColor = Color.blue;
                trailRenderer.endColor = Color.grey;
                break;
            case 17:
                trailRenderer.startColor = Color.grey;
                trailRenderer.endColor = Color.red ;
                break;
            default:
                trailRenderer.startColor = Color.black;
                trailRenderer.endColor = Color.black;
                break;
        }
        //if(damage < 3)
        //{
            
        //    trailRenderer.startColor = Color.white;
        //    trailRenderer.endColor = Color.white;

        //}
        //else if(damage >= 3 && damage < 5)
        //{
        //    trailRenderer.startColor = Color.red;
        //    trailRenderer.endColor = Color.blue;
        //}
        //else if(damage >= 5 && damage < 10)
        //{
        //    trailRenderer.startColor = Color.yellow;
        //    trailRenderer.endColor = Color.green;
        //}
        //else if(damage >= 10)
        //{
        //    trailRenderer.startColor = Color.cyan;
        //    trailRenderer.endColor = Color.blue;
        //}

        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Minion") || other.CompareTag("Drone"))
            {
                // checking if the object that we collided with (other) has the IDamage script (i.e. is damageable)
                IDamage damageable = other.GetComponent<IDamage>();

                // if the object is damageable
                if (damageable != null)
                {
                    // then take the specified amount of damage
                    damageable.TakeDamage(damage);
                }
                // destroying the bullet if it hits something
                Destroy(gameObject);
            }
            
        }
        
            
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player") || !collision.collider.CompareTag("Enemy") ||   !collision.collider.CompareTag("Minion"))
        {
            Destroy(gameObject,.05f);
        }
    }
}
