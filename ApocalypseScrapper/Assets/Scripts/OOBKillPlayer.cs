
using UnityEngine;

public class OOBKillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.isDead = true;
            gameManager.instance.PlayerDead();
        }
    }
}
