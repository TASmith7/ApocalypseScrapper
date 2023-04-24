using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    

    [Header("----- Player/Boss -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    //public GameObject Boss;
    //public BossAI bossScript;

    [Header("----- Menu's -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject RSGSplash;
    public GameObject checkpointMenu;
    [Header("----- HP Bar -----")]
    public Image HPBar;
    [Header("----- Salvage & Grade Bar -----")]
    public TextMeshProUGUI salvageValueText;
    public TextMeshProUGUI salvageCollected;
    public TextMeshProUGUI scoreText;
    public int amtSalvaged;
    public TextMeshProUGUI grade;
    public char playerGrade;
    [Header("----- Boss Health Bar -----")]
    public Image bossHealthBar;
    public GameObject bossHealthBarParent;
    [Header("----- Jetpack Bar -----")]
    public Image jetpackFuelBar;
    public GameObject jetpackFuelBarParent;
    [Header("----- Stmaina Bar -----")]
    public Image staminaFillBar;
    public GameObject staminaFillBarParent;
    [Header("----- Shield Bar -----")]
    public Image shieldFillBar;
    public GameObject shieldFillBarParent;
    [Header("----- Reticle Bar -----")]
    public GameObject mainReticle;
    public GameObject salvageableItemReticle;
    public Image salvagingObjectReticle;
    [Header("----- Score Text Bar -----")]
    public TextMeshProUGUI playerSalvageScoreText;
    
    public GameObject salvagingObjectParent;

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
        bossHealthBarParent.SetActive(false);
        
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

        // stopping all game audio that might be playing
        playerAudioManager.instance.PauseAllAudio();
        levelAudioManager.instance.PauseAllAudio();

    }

    public void UnpauseState()
    {
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
        
        // unpausing level audio
        levelAudioManager.instance.UnpauseAllAudio();
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
    public void TurnOffStaminaUI()
    {
        jetpackFuelBarParent.SetActive(false);
    }

    public void TurnOnStaminaUI()
    {
        jetpackFuelBarParent.SetActive(true);
    }
    public void TurnOffBossHPUI()
    {
        jetpackFuelBarParent.SetActive(false);
    }

    public void TurnOnBossHPUI()
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
        string pText = SetGradeText(playerGrade);
        scoreText.text = pText;
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
    public  string SetGradeText(char grade)
    {
        switch (grade)
        {
            case 'S':
                return "Good Job. \nYou Collected a lot of Scrap. \nHave fun Spending it.";
            case 'A':
                return "You understood what to do. \nGood Job.";
            case 'B':
                return "So close, yet so far.";
            case 'C':
                return "Getting there. \nThe odds increase";
            case 'D':
                return "Salvage=You live. It's not hard";
            case 'F':
                return "Maybe try collecting some salvage.\n Might make the game easier";
            default:
                return "Negative salvage. You owe us.";
        }

       
    }
}
