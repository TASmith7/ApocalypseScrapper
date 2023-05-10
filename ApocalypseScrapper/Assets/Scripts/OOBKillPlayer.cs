using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOBKillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            gameManager.instance.PlayerDead();
    }
}
