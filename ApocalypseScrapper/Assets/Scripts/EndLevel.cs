
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 1"))
            {
                PlayerPrefs.SetInt("BeatLevel1", 1);
            }
            else if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 2"))
            {
                PlayerPrefs.SetInt("BeatLevel2", 1);
            }
            else if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 3"))
            {
                PlayerPrefs.SetInt("BeatLevel3", 1);
            }

            gameManager.instance.NextLevel();
        }

        

    }
}
