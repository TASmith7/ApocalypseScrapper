using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Stuff -----")]
    public GameObject activeMenu;
    
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject RSGSplash;
    public GameObject checkpointMenu;
    public Image HPBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI salvageCollected;
    public int amtSalvaged;
    public TextMeshProUGUI grade;
    public char playerGrade;
    public Image jetpackFuelBar;
    public GameObject jetpackFuelBarParent;
    public GameObject mainReticle;
    public GameObject salvageableItemReticle;
    public TextMeshProUGUI playerSalvageScoreText;

    //[Header("-----Turret Stuff-----")]
    
    //public GameObject turret;
    //[Header("-----Rat Stuff-----")]
    //public GameObject rat;
    //public int enemiesRemaining;

    public bool isPaused;
    float timeScaleOriginal;

    void Awake()
    {
        instance = this;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Lvl 1"))
        {
            StartCoroutine(SplashScreen());
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOriginal = Time.timeScale;
    }
    IEnumerator SplashScreen()
    {
        
        
            activeMenu = RSGSplash;
            RSGSplash.SetActive(true);
            yield return new WaitForSeconds(2);
            /*activeMenu=controlsMenu;
             yield return new WaitForSeconds(2);

             activeMenu=storyMenu;
             yield return new WaitForSeconds(2);

             */
            RSGSplash.SetActive(false);
            activeMenu = null;
        
        

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
            {
                PauseState();
            }
            else
            {
                UnpauseState();
            }
        }


    }
    
    public void PauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        mainReticle.SetActive(false);
        salvageableItemReticle.SetActive(false);
    }

    public void UnpauseState()
    {
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void UpdateGameGoal()
    {



            PlayerWins();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            PauseState();
        
    }

    public void PlayerDead()
    {
        PauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void TurnOffJetpackUI()
    {
        jetpackFuelBarParent.SetActive(false);
    }

    public void TurnOnJetpackUI()
    {
        jetpackFuelBarParent.SetActive(true);
    }

    public void CueSalvageableReticle()
    {
        mainReticle.SetActive(false);
        salvageableItemReticle.SetActive(true);
    }

    public void CueMainReticle()
    {
        salvageableItemReticle.SetActive(false);
        mainReticle.SetActive(true);
    }

    public void UpdateSalvageScore(int score)
    {
        playerSalvageScoreText.text = score.ToString();
        amtSalvaged = score;
    }
    public void PlayerWins()
    {
        salvageCollected.text=amtSalvaged.ToString();
        if(amtSalvaged>4501)
        {
            playerGrade = 'S';
        }
        else if(amtSalvaged > 4001 && amtSalvaged <= 4500)
        {
            playerGrade = 'A';
        }
        else if (amtSalvaged > 3501 && amtSalvaged <= 4000)
        {
            playerGrade = 'B';
        }
        else if (amtSalvaged > 3001 && amtSalvaged <= 3500)
        {
            playerGrade = 'C';

        }
        else if (amtSalvaged > 2501 && amtSalvaged <= 3000)
        {
            playerGrade = 'D';
        }
        else
        {
            playerGrade = 'F';
        }
        grade.text = playerGrade.ToString();

    }
}
