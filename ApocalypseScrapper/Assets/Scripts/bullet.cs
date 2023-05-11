
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timer;

    void Start()
    {
        // destroying our bullet after a specified amount of time
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
            // Debug.Log("Bullet hit+", other.gameObject);
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

    private void OnCollisionEnter(Collision collision)
    { 
        // destroying the bullet if it hits something
        Destroy(gameObject);
        Debug.Log("Destroyed Bullet!");
    }
}
