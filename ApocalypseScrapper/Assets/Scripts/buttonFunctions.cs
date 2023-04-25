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
    public void RestartLevel()
    {
        gameManager.instance.UnpauseState();
        gameManager.instance.playerScript.RestartLevel();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;

    }
    public void RestartMission()
    {
        gameManager.instance.UnpauseState();
        gameManager.instance.playerScript.RestartMission();
    }
    public void Options()
    {

    }
    public void Exit()
    {
        Application.Quit();
    }

}
