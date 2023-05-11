using UnityEngine;

public class bulletPlayer : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timer;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] playerController playerScript;

    void Start()
    {
        //pulling damage from playerController, which should have proper damge w/ upgrades
       
        // destroying our bullet after a specified amount of time
        if(damage < 3)
        {
            damage = playerScript.shootDamage;
            trailRenderer.startColor = Color.white;
            trailRenderer.endColor = Color.white;

        }
        else if(damage >= 3 && damage < 5)
        {
            trailRenderer.startColor = Color.red;
            trailRenderer.endColor = Color.blue;
        }
        else if(damage >= 5 && damage < 10)
        {
            trailRenderer.startColor = Color.yellow;
            trailRenderer.endColor = Color.green;
        }
        else if(damage >= 10)
        {
            trailRenderer.startColor = Color.cyan;
            trailRenderer.endColor = Color.blue;
        }

        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
        {
            if (other.CompareTag("Enemy"))
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
}
