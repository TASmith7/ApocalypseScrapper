using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void Resume()
    {
        gameManager.instance.UnpauseState();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }
    public void Respawn()
    {
        gameManager.instance.UnpauseState();
        gameManager.instance.playerScript.RespawnPlayer();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;

    }
    public void Restart()
    {
        gameManager.instance.UnpauseState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Options()
    {

    }
    public void Exit()
    {
        Application.Quit();
    }

}
