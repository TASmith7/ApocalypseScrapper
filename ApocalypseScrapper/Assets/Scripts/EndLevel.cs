using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // stop any voice overs from playing
            if (levelAudioManager.instance.voiceOverAudioSource.isPlaying)
            {
                levelAudioManager.instance.voiceOverAudioSource.Stop();
            }
            gameManager.instance.NextLevel();
        }
        
    }
}
