using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class returnToMainMenu : MonoBehaviour
{
    public static void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
