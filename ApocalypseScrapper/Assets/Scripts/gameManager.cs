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
    public GameObject ControlsSplash;
    public GameObject ApocSplash;
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
    public GameObject totalScoreLabel;
    public GameObject playerBonusLabel;
    public GameObject floorScoreLabel;

    [Header("----- Boss Health Bar -----")]
    public Image bossHealthBar;
    public GameObject bossHealthBarParent;

    [Header("----- Jetpack Bar -----")]
    public Image jetpackFuelBar;
    public GameObject jetpackFuelBarParent;

    [Header("----- Stamina Bar -----")]
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


    [Header("----- Incoming Transmission -----")]
    public GameObject incomingTransmissionText;
    public GameObject skipTransmissionText;

    //public GameObject salvagingObjectParent;
    

    //[Header("-----Turret Stuff-----")]
    
    //public GameObject turret;
    //[Header("-----Rat Stuff-----")]
    //public GameObject rat;
    //public int enemiesRemaining;

    public bool isPaused;
    float timeScaleOriginal;
    public Scene currentScene;
    bool voWasPlaying;

    AudioClip currentVOPlaying;

    void Awake()
    {
        instance = this;

        // setting our current scene
        currentScene = SceneManager.GetActiveScene();

        if (currentScene == SceneManager.GetSceneByName("Lvl 1"))
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
        // turning off all UI while splash screen is displayed
        TurnOffJetpackUI();
        TurnOffShieldUI();
        TurnOffStaminaUI();
        totalScoreLabel.SetActive(false);
        playerBonusLabel.SetActive(false);
        floorScoreLabel.SetActive(false);

        activeMenu = RSGSplash;
        RSGSplash.SetActive(true);
        yield return new WaitForSeconds(3);
        RSGSplash.SetActive(false);
        activeMenu = ApocSplash;
        ApocSplash.SetActive(true);
        yield return new WaitForSeconds(3);
        ApocSplash.SetActive(false);
        activeMenu = ControlsSplash;
        ControlsSplash.SetActive(true);
        yield return new WaitForSeconds(3);
        ControlsSplash.SetActive(false);

        activeMenu = null;

        if(!levelAudioManager.instance.voiceOverAudioSource.isPlaying)
        {
            // playing intro voice over
            levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOIntro);
        }

        // turning back on salvage UI (all other UI is cued to turn back on elsewhere)
        totalScoreLabel.SetActive(true);
        playerBonusLabel.SetActive(true);
        floorScoreLabel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            levelAudioManager.instance.pauseMenuAudioSource.Play();

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

        // if we are hearing a voice over, cue incoming transmission text
        if(levelAudioManager.instance.voiceOverAudioSource.isPlaying)
        {
            incomingTransmissionText.SetActive(true);
            skipTransmissionText.SetActive(true);
        }
        // else turn it off
        else
        {
            incomingTransmissionText.SetActive(false);
            skipTransmissionText.SetActive(false);
        }

        // if we press T while listening to a VO, stop the VO
        if(Input.GetButtonDown("Skip") && levelAudioManager.instance.voiceOverAudioSource.isPlaying)
        {
            levelAudioManager.instance.voiceOverAudioSource.Stop();
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

        // if we are playing a voice over, pause when in pause state and set flag to true
        if(levelAudioManager.instance.voiceOverAudioSource.isPlaying)
        {
            levelAudioManager.instance.voiceOverAudioSource.Pause();
            voWasPlaying = true;
        }
        // else set flag to false
        else 
        {
            voWasPlaying = false;
        }

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

        // if we were playing a vo when we paused, resume that vo
        if(voWasPlaying)
        {
            levelAudioManager.instance.voiceOverAudioSource.UnPause();
        }
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

        // stop any voice over audio that might already be playing
        levelAudioManager.instance.voiceOverAudioSource.Stop();

        levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOPlayerDead);
        activeMenu = loseMenu;
        activeMenu.SetActive(true); 
    }

    public void TurnOffJetpackUI()
    {
        jetpackFuelBarParent.SetActive(false);
    }

    public void TurnOnShieldUI()
    {
        shieldFillBarParent.SetActive(true);
    }

    public void TurnOffShieldUI()
    {
        shieldFillBarParent.SetActive(false);
    }

    public void TurnOnJetpackUI()
    {
        jetpackFuelBarParent.SetActive(true);
    }
    public void TurnOffStaminaUI()
    {
        staminaFillBarParent.SetActive(false);
    }

    public void TurnOnStaminaUI()
    {
        staminaFillBarParent.SetActive(true);
    }
    public void TurnOffBossHPUI()
    {
        // jetpackFuelBarParent.SetActive(false);
    }

    public void TurnOnBossHPUI()
    {
        // jetpackFuelBarParent.SetActive(true);
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
        if (playerScript.salvDetector == true)
        {
            salvageCollected.text = score.ToString() + " of " + playerScript.totalLevelSalvage;
        }
        else salvageCollected.text = score.ToString();
        
        
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

    public void NextLevel()
    {
        instance.playerScript.SavePlayerStats();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Lvl 1":
                SceneManager.LoadScene("Lvl 2");
                break;

            case "Lvl 2":
                SceneManager.LoadScene("Lvl 3");
                break;

            case "Lvl 3":
                SceneManager.LoadScene("Boss Lvl");
                break;

            case "Boss Lvl":
                WinGame();
                break;

            default:
                Debug.Log("Level failed to load");
                break;
        }
    }

    public void WinGame()
    {

    }


}


