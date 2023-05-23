using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretDetectionRange : MonoBehaviour
{
    public bool playerInRange;
    [SerializeField] SphereCollider turretCollWake;
    [Range(10, 1000)][SerializeField] float radiusActive;
    [Range(10, 200)][SerializeField] float radiusSleep;
    public float activeRadius;

    private void Start()
    {
        activeRadius = radiusSleep;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            turretCollWake.radius = radiusActive;
            activeRadius = turretCollWake.radius;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
