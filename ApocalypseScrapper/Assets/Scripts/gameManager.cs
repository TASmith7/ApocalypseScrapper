using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Gamelog Vars -----")]
    [SerializeField] List<Message> gamelog = new List<Message>();
    public int maxMessages = 30;
    public GameObject gamelogMain;
    public GameObject gamelogPanel;
    public GameObject textObject;
    float timeOfLastMessage;
    float timeToClearGamelog = 5;

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
    public GameObject ControlsSplashFromOptions;
    public GameObject ApocSplash;
    public GameObject checkpointMenu;

    public GameObject craftingMenu;
    public GameObject playerStatsScreen;
    public GameObject optionsMenu;

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
    public TextMeshProUGUI totalScoreData;
    public TextMeshProUGUI playerBonusData;
    public TextMeshProUGUI floorScoreData;
    public TextMeshProUGUI salvageCutData;
    public TextMeshProUGUI totalPayoutData;
    public TextMeshProUGUI remainingBonusData;
    public TextMeshProUGUI totalSalvagedData;
    public TextMeshProUGUI finalRankData;


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

    [Header("----- Crafting Objects -----")]

    public TextMeshProUGUI FloorAvailData;
    public TextMeshProUGUI LevelScrapCollected;
    public TextMeshProUGUI SpentScrap;
    public GameObject DeclinedPurchasePopUp;

    public int spent;


    //public TextMeshProUGUI LevelScrapCollectedStore;
    //public TextMeshProUGUI SpentScrapStore;
    //public GameObject DeclinedPurchasePopUpStore;
    //public TextMeshProUGUI FinalFloorScoreDataStore;

    [Header("----- Player Stats Objects -----")]
    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI maxHealthValue;
    public TextMeshProUGUI shieldRechargeValue;
    public TextMeshProUGUI shieldHealthValue;
    public TextMeshProUGUI jetpackRechargeValue;
    public TextMeshProUGUI jetpackConsumptionValue;
    public TextMeshProUGUI DamageValue;
    public TextMeshProUGUI rateOfFireValue;
    public TextMeshProUGUI salvageSpreadValue;
    public TextMeshProUGUI salvageRangeValue;
    public TextMeshProUGUI salvageDetectorCondition;
    public TextMeshProUGUI staminaRegenValue;
    public TextMeshProUGUI staminaDrainValue;
    [Header("----- Player Inventory Objects -----")]
    public GameObject InventroyParent;

    public Toggle invShown;
    public bool craftingOpen;
    public Image BioMass;
    public Image IntactOrgan;
    public Image ElectronicComponent;
    public Image DataCore;
    public Image DenseMetalPlate;
    public Image HighTensileAlloy;
    public Image GlassPane;
    public Image HPLightDiode;
    public Image ElectricMotor;
    public Image CeramicPlate;
    public Image GoldAlloy;
    public Image ValuableLoot;
    public TextMeshProUGUI BioMassAmt;
    public TextMeshProUGUI IntactOrganAmt;
    public TextMeshProUGUI ElectricCompAmt;
    public TextMeshProUGUI DataCoreAmt;
    public TextMeshProUGUI DenseMetalPlateAmt;
    public TextMeshProUGUI HighTensileAlloyAmt;
    public TextMeshProUGUI GlassPaneAmt;
    public TextMeshProUGUI HPLDAmt;
    public TextMeshProUGUI ElectricMotorAmt;
    public TextMeshProUGUI CeramicPlateAmt;
    public TextMeshProUGUI GoldAlloyAmt;
    public TextMeshProUGUI ValuableLootAmt;

    [Header("----- Options Settings -----")]
    public Slider horizontalSens;
    public Slider verticalSens;
    public Slider musicVolume;
    public TextMeshProUGUI horSensValue;
    public TextMeshProUGUI vertSensValue;
    public TextMeshProUGUI musicVolumeValue;
    public Toggle dynamicFOVToggle;
    public Toggle voiceoversToggle;
    public Toggle subtitlesToggle;


    [Header("----- End Game Beam -----")]
    public TextMeshProUGUI hazardPayData;
    public TextMeshProUGUI QuestCompletionPayData;
    [Header("----- Loading Screens -----")]

    public GameObject lvl2;
    public GameObject lvl3;
    public GameObject lvl4;
    [Header("----- End Game Beam -----")]
    public GameObject endGameBeam;

    [Header("----- Player Death Overlay -----")]
    public GameObject playerDeathOverlay;
    public GameObject blackOverlayForDeathParent;
    public Image blackOverlayForDeath;

    [Header("----- Subtitles -----")]
    public GameObject subtitleParentObject;
    public TextMeshProUGUI subtitleText;
    public subtitleManager.VoiceLine[] currentVoiceLine;

    [Header("----- Intro Script Vars -----")]
    public bool introVOPlaying;
    public bool skipped;
    public GameObject eleDoor;

    //public GameObject salvagingObjectParent;


    //[Header("-----Turret Stuff-----")]

    //public GameObject turret;
    //[Header("-----Rat Stuff-----")]
    //public GameObject rat;
    //public int enemiesRemaining;



    public bool isPaused;
    public float timeScaleOriginal;
    public Scene currentScene;
    bool voWasPlaying;
    bool npcWasTalking;

    AudioClip currentVOPlaying;

    void Awake()
    {
        instance = this;
        isPaused = false;
        blackOverlayForDeathParent.SetActive(false);

        // setting our current scene
        currentScene = SceneManager.GetActiveScene();
        if (currentScene == SceneManager.GetSceneByName("Lvl 1"))
        {
            StartCoroutine(SplashScreen());
        }


        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Object is named" + player.name);
        //playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        timeScaleOriginal = Time.timeScale;
        bossHealthBarParent.SetActive(false);

        // setting our sensitivity sliders to equal the values set in our camera controller script
        //horizontalSens.value = playerScript.playerCam.GetComponent<cameraControls>().sensHorizontal;
        //verticalSens.value = playerScript.playerCam.GetComponent<cameraControls>().sensVertical;

        // setting our sensitivity to be the value saved in player prefs on open IF the keys exist (they only won't exist if the player doesn't adjust sensitivity)
        if (PlayerPrefs.HasKey("HorizontalSensitivity") && PlayerPrefs.HasKey("VerticalSensitivity"))
        {
            horizontalSens.value = PlayerPrefs.GetInt("HorizontalSensitivity");
            verticalSens.value = PlayerPrefs.GetInt("VerticalSensitivity");
        }
        // if the keys do not exist in player prefs, give the sensitivity a default value
        else
        {
            horizontalSens.value = 300;
            verticalSens.value = 300;
            Debug.Log("Player prefs sensitivity keys do not exist");
        }


        // playerScript.playerCam.GetComponent<cameraControls>().sensHorizontal = (int)horizontalSens.value;
        // playerScript.playerCam.GetComponent<cameraControls>().sensVertical = (int)verticalSens.value;


        // setting the sensitivity labels equal the sliders current values
        horSensValue.text = horizontalSens.value.ToString();
        vertSensValue.text = verticalSens.value.ToString();


        // if our player prefs has the dynamic FOV key
        if (PlayerPrefs.HasKey("DynamicFOV"))
        {
            // if our key is currently set to 1 (on) in player prefs, turn dynamic FOV on
            if (PlayerPrefs.GetInt("DynamicFOV") == 1)
            {
                dynamicFOVToggle.isOn = true;
            }
            // else if our key is currently set to 0 (off) in player prefabs, turn dynamic FOV off
            else if (PlayerPrefs.GetInt("DynamicFOV") == 0)
            {
                dynamicFOVToggle.isOn = false;
            }
        }
        else
        {
            dynamicFOVToggle.isOn = true;
        }


        // if our player prefs has the voice overs key
        if (PlayerPrefs.HasKey("UseVoiceovers"))
        {
            // if our key is currently set to 1 (on) in player prefs, turn voice overs on
            if (PlayerPrefs.GetInt("UseVoiceovers") == 1)
            {
                voiceoversToggle.isOn = true;
            }
            // else if our key is currently set to 0 (off) in player prefabs, turn voice overs off
            else if (PlayerPrefs.GetInt("UseVoiceovers") == 0)
            {
                voiceoversToggle.isOn = false;
            }
        }
        else
        {
            voiceoversToggle.isOn = true;
        }

        // if player prefs has the volume key
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        // else set music volume to default value
        else
        {
            musicVolume.value = 65;
        }

        musicVolumeValue.text = musicVolume.value.ToString();
        // initial volume value for music is assigned in level audio manager

        if (PlayerPrefs.HasKey("Subtitles"))
        {
            if (PlayerPrefs.GetInt("Subtitles") == 1)
            {
                subtitlesToggle.isOn = true;
            }
            else if (PlayerPrefs.GetInt("Subtitles") == 0)
            {
                subtitlesToggle.isOn = false;
            }
        }
        else
        {
            subtitlesToggle.isOn = true;
        }

        if (currentScene == SceneManager.GetSceneByName("Boss Lvl"))
        {
            endGameBeam.SetActive(false);
        }

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
        //activating splash screens 

        activeMenu = RSGSplash;
        RSGSplash.SetActive(true);
        yield return new WaitForSeconds(5);
        if (!levelAudioManager.instance.voiceOverAudioSource.isPlaying && voiceoversToggle.isOn)
        {
            // playing intro voice over
            levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOIntro);
            introVOPlaying = true;

            // if our subtitle toggle is on
            if (subtitlesToggle.isOn)
            {
                StartCoroutine(gameManager.instance.StartSubtitles(subtitleManager.instance.lvl1IntroVoiceLines));
            }
        }
        if (!levelAudioManager.instance.elevatorAudioSource.isPlaying)
        {
            levelAudioManager.instance.elevatorAudioSource.PlayOneShot(levelAudioManager.instance.elevatorUp);
            StartCoroutine(StopElevatorWait());
        }
        RSGSplash.SetActive(false);
        activeMenu = ApocSplash;
        ApocSplash.SetActive(true);
        yield return new WaitForSeconds(5);
        ApocSplash.SetActive(false);
        activeMenu = ControlsSplash;
        ControlsSplash.SetActive(true);
        yield return new WaitForSeconds(5);
        ControlsSplash.SetActive(false);


        activeMenu = null;



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
        if (levelAudioManager.instance.voiceOverAudioSource.isPlaying)
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
        if (Input.GetButtonDown("Skip") && levelAudioManager.instance.voiceOverAudioSource.isPlaying)
        {
            if (introVOPlaying == true)
            {
                Debug.Log("Entered skip to elvator stop");
                introVOPlaying = false;
                skipped = true;
                levelAudioManager.instance.elevatorAudioSource.Stop();
                levelAudioManager.instance.elevatorAudioSource.PlayOneShot(levelAudioManager.instance.elevatorStop);
                eleDoor.SetActive(false);

            }
            levelAudioManager.instance.voiceOverAudioSource.Stop();
            subtitleParentObject.SetActive(false);
        }

        if (isPaused)
        {
            // changing labels to match the value of the slider while they slide it up and down
            horSensValue.text = horizontalSens.value.ToString();
            vertSensValue.text = verticalSens.value.ToString();
            musicVolumeValue.text = musicVolume.value.ToString();
        }
        if (Time.fixedTime - timeOfLastMessage >= timeToClearGamelog)
        {
            gamelogMain.SetActive(false);
            for (int i = 0; i < gamelog.Count; i++)
            {
                Destroy(gamelog[i].textObject.gameObject);
            }
            gamelog.Clear();
        }
        if (Inventory.Instance)
        {
            UpdateInventory();
        }
        if (Input.GetButtonDown("Tab") && !craftingOpen && activeMenu == null)
        {
            craftingOpen = true;


            CueCrafting();

        }
        else if (Input.GetButtonDown("Tab") && craftingOpen && activeMenu == craftingMenu)
        {


            craftingOpen = false;
            CloseCrafting();
        }
        if (invShown.isOn && craftingOpen)
        {

            TurnOnInventoryUI();

        }
        else
        {
            TurnOffInventoryUI();
        }



    }

    IEnumerator StopElevatorWait()
    {
        yield return new WaitForSeconds(levelAudioManager.instance.VOIntro.length);

        if (!skipped)
        {
            Debug.Log("Entering 'not skipped' elevator block removal");
            introVOPlaying = false;
            levelAudioManager.instance.elevatorAudioSource.Stop();
            levelAudioManager.instance.elevatorAudioSource.PlayOneShot(levelAudioManager.instance.elevatorStop);
            yield return new WaitForSeconds(6);
            eleDoor.SetActive(false);
        }
    }
    public void CueCrafting()
    {
        // we don't need the below line as calling pause state method pauses the vo that was playing already
        // levelAudioManager.instance.voiceOverAudioSource.Stop();
        PauseState();
        isPaused = true;





        activeMenu = craftingMenu;
        activeMenu.SetActive(true);
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

        if (currentScene == SceneManager.GetSceneByName("Lvl 2") && npcAudioManager.instance != null)
        {
            npcAudioManager.instance.PauseAllAudio();
        }

        if (currentScene == SceneManager.GetSceneByName("Boss Lvl"))
        {
            gameManager.instance.endGameBeam.GetComponent<AudioSource>().Stop();
        }

        // if we are playing a voice over, pause when in pause state and set flag to true
        if (levelAudioManager.instance.voiceOverAudioSource.isPlaying)
        {
            levelAudioManager.instance.voiceOverAudioSource.Pause();
            voWasPlaying = true;
        }
        // else set flag to false
        else
        {
            voWasPlaying = false;
        }

        if (currentScene == SceneManager.GetSceneByName("Lvl 2") && npcAudioManager.instance != null)
        {
            if (npcAudioManager.instance.npcVoiceAudioSource.isPlaying)
            {
                npcAudioManager.instance.npcVoiceAudioSource.Pause();
                npcWasTalking = true;
            }
        }
        else
        {
            npcWasTalking = false;
        }

    }

    public void UnpauseState()
    {
        totalScoreLabel.SetActive(true);
        playerBonusLabel.SetActive(true);
        floorScoreLabel.SetActive(true);
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = null;

        if (currentScene == SceneManager.GetSceneByName("Boss Lvl"))
        {
            gameManager.instance.endGameBeam.GetComponent<AudioSource>().Play();
        }

        // unpausing level audio
        levelAudioManager.instance.UnpauseAllAudio();

        // if we were playing a vo when we paused, resume that vo
        if (voWasPlaying && voiceoversToggle.isOn)
        {
            levelAudioManager.instance.voiceOverAudioSource.UnPause();
        }

        if (currentScene == SceneManager.GetSceneByName("Lvl 2") && npcWasTalking && npcAudioManager.instance != null)
        {
            npcAudioManager.instance.npcVoiceAudioSource.UnPause();
        }
    }

    public void UpdateGameGoal()
    {
        // PlayerWins();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        PauseState();
        isPaused = true;
        WinGame();
    }

    public void PlayerDead()
    {
        blackOverlayForDeathParent.SetActive(true);
        playerScript.isDead = true;
        Debug.Log("Player Dead");
        playerDeathOverlay.SetActive(true);
        // stop any voice over audio that might already be playing
        levelAudioManager.instance.voiceOverAudioSource.Stop();

        // stop music
        levelAudioManager.instance.musicAudioSource.Stop();

        StartCoroutine(FadeImage());

        if (voiceoversToggle.isOn)
        {
            levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOPlayerDead);

            if (subtitlesToggle.isOn)
            {
                StartCoroutine(StartSubtitles(subtitleManager.instance.playerDeathVoiceLines));
            }
        }

        StartCoroutine(PlayerDeadMenuDelay());
    }

    IEnumerator FadeImage()
    {
        for (float i = 0; i <= 5; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackOverlayForDeath.color = new Color(0, 0, 0, i / 5);
            yield return null;
        }
    }

    IEnumerator PlayerDeadMenuDelay()
    {
        yield return new WaitForSeconds(levelAudioManager.instance.VOPlayerDead.length);
        PauseState();

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
        salvageCollected.text = amtSalvaged.ToString();
        if (amtSalvaged > 4501)
        {

            playerGrade = 'S';
        }
        else if (amtSalvaged > 4001 && amtSalvaged <= 4500)
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
    public string SetGradeText(char grade)
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

    //public void CueStore()
    //{

    //    levelAudioManager.instance.voiceOverAudioSource.Stop();
    //    PauseState();
    //    isPaused = true;
    //    char rank = Rank();
    //    int clearPercent = (int)((playerScript.playerFloorScore / playerScript.totalLevelSalvage) * 100);
    //    int bonus = Bonus(rank);
    //    FinalFloorScoreData.text = playerScript.playerFloorScore.ToString();
    //    FloorAvailData.text = playerScript.totalLevelSalvage.ToString();



    //    if (rank == 'F')
    //    {
    //        if (voiceoversToggle.isOn)
    //        {
    //            levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFloorFail);

    //            if (subtitlesToggle.isOn)
    //            {
    //                StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.floorFailVoiceLines));
    //            }
    //        }

    //        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Lvl 1"))
    //        {
    //            AudioSource next = levelAudioManager.instance.voiceOverAudioSource;
    //            next.clip = levelAudioManager.instance.VOStoreTutorial;
    //            if (voiceoversToggle.isOn)
    //            {

    //                if (subtitlesToggle.isOn)
    //                {
    //                    StartCoroutine(DelayForStoreSubtitlesFailFloor());
    //                }

    //                next.PlayDelayed(13);
    //            }
    //        }
    //        else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Lvl 1"))
    //        {
    //            AudioSource next = levelAudioManager.instance.voiceOverAudioSource;
    //            next.clip = levelAudioManager.instance.VOBonusSpendIt;

    //            if (voiceoversToggle.isOn)
    //            {
    //                if (subtitlesToggle.isOn)
    //                {
    //                    StartCoroutine(DelayForStoreSubtitlesFailFloor());
    //                }

    //                next.PlayDelayed(13);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (voiceoversToggle.isOn)
    //        {
    //            levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFloorPass);

    //            if (subtitlesToggle.isOn)
    //            {
    //                StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.floorPassVoiceLines));
    //            }
    //        }

    //        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Lvl 1"))
    //        {
    //            AudioSource next = levelAudioManager.instance.voiceOverAudioSource;
    //            next.clip = levelAudioManager.instance.VOStoreTutorial;
    //            if (voiceoversToggle.isOn)
    //            {
    //                if (subtitlesToggle.isOn)
    //                {
    //                    StartCoroutine(DelayForStoreSubtitlesPassFloor());
    //                }

    //                next.PlayDelayed(6);
    //            }
    //        }
    //        else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Lvl 1"))
    //        {
    //            AudioSource next = levelAudioManager.instance.voiceOverAudioSource;
    //            next.clip = levelAudioManager.instance.VOBonusSpendIt;
    //            if (voiceoversToggle.isOn)
    //            {
    //                if (subtitlesToggle.isOn)
    //                {
    //                    StartCoroutine(DelayForStoreSubtitlesPassFloor());
    //                }

    //                next.PlayDelayed(6);
    //            }
    //        }
    //    }
    //}

    IEnumerator DelayForStoreSubtitlesFailFloor()
    {
        yield return new WaitForSecondsRealtime(13);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Lvl 1"))
        {
            StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.storeInfoVoiceLines));
        }
        else
        {
            StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.bonusSpendItVoiceLines));
        }
    }
    IEnumerator DelayForStoreSubtitlesPassFloor()
    {
        yield return new WaitForSecondsRealtime(6);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Lvl 1"))
        {
            StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.storeInfoVoiceLines));
        }
        else
        {
            StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.bonusSpendItVoiceLines));
        }
    }

    public void CloseCrafting()
    {
        // levelAudioManager.instance.voiceOverAudioSource.Stop();
        craftingMenu.SetActive(false);
        UnpauseState();
        isPaused = false;
    }
    public char Rank()
    {
        int percentClear = (int)((playerScript.playerFloorScore / playerScript.totalLevelSalvage) * 100);

        if (percentClear > 90)
        {
            return 'S';
        }
        else if (percentClear > 80 && percentClear <= 90)
        {
            return 'A';
        }
        else if (percentClear > 70 && percentClear <= 80)
        {
            return 'B';
        }
        else if (percentClear > 60 && percentClear <= 70)
        {
            return 'C';
        }
        else if (percentClear > 50 && percentClear <= 60)
        {
            return 'D';
        }
        else if (percentClear < 50)
        {
            return 'F';
        }

        else return 'Z';
    }

    //public int Bonus(char rank)
    //{
    //    switch (rank)
    //    {
    //        case 'S':
    //            return 1000;
    //        case 'A':
    //            return 800;
    //        case 'B':
    //            return 600;
    //        case 'C':
    //            return 400;
    //        case 'D':
    //            return 200;
    //        case 'F':
    //            return 0;
    //        default: return 0;
    //    }
    //}


    public void NextLevel()
    {
        PauseState();
        instance.playerScript.SavePlayerStats();
        Inventory.Instance.InvSnapshot();
        if (SceneManager.GetActiveScene().name == "Lvl 1")
        {
            //start coroutine to load next level and show loading screen for 5 seconds
            StartCoroutine(Lvl2LoadScreen());

        }
        else if (SceneManager.GetActiveScene().name == "Lvl 2")
        {
            StartCoroutine(Lvl3LoadScreen());

        }
        else if (SceneManager.GetActiveScene().name == ("Lvl 3"))
        {
            StartCoroutine(BossLvlLoadScreen());
        }
        else if (SceneManager.GetActiveScene().name == "Boss Lvl")
        {
            WinGame();
        }
        else
        {
            Debug.Log("Level failed to load");
        }
    }


    //    switch (SceneManager.GetActiveScene().name)
    //    {
    //        case "Lvl 1":
    //            StartCoroutine(Lvl2LoadScreen());
    //            SceneManager.LoadScene("Lvl 2");
    //            break;

    //        case "Lvl 2":
    //            StartCoroutine(Lvl3LoadScreen());
    //            SceneManager.LoadScene("Lvl 3");
    //            break;

    //        case "Lvl 3":
    //            StartCoroutine(Lvl4LoadScreen());
    //            SceneManager.LoadScene("Boss Lvl");
    //            break;

    //        case "Boss Lvl":

    //            WinGame();
    //            break;

    //        default:
    //            Debug.Log("Level failed to load");
    //            break;
    //    }


    IEnumerator Lvl2LoadScreen()
    {

        lvl2.SetActive(true);
        Debug.Log("WFS Started");
        yield return new WaitForSecondsRealtime(5);

        Debug.Log("WFS Done");
        lvl2.SetActive(false);
        SceneManager.LoadScene("Lvl 2");
        UnpauseState();
    }
    IEnumerator Lvl3LoadScreen()
    {

        lvl3.SetActive(true);
        Debug.Log("WFS Started");
        yield return new WaitForSecondsRealtime(5);

        Debug.Log("WFS Done");

        lvl3.SetActive(false);
        SceneManager.LoadScene("Lvl 3");
        UnpauseState();
    }
    IEnumerator BossLvlLoadScreen()
    {

        lvl4.SetActive(true);
        Debug.Log("WFS Started");
        yield return new WaitForSecondsRealtime(5);

        Debug.Log("WFS 5 Done");
        lvl4.SetActive(false);
        SceneManager.LoadScene("Boss Lvl");
        UnpauseState();
    }
    public void WinGame()
    {
        // updating total score value
        playerScript.playerTotalScore += playerScript.playerFloorScore;
        hazardPayData.text = playerScript.hazardPay.ToString();
        QuestCompletionPayData.text = playerScript.questPay.ToString();
        playerScript.playerBonus += 50;

        // updating total salvage for all levels
        totalSalvagedData.text = playerScript.playerTotalScore.ToString();

        // updating salvage cut value
        int salvageCut = (int)(playerScript.playerTotalScore * 0.03);
        salvageCutData.text = salvageCut.ToString();

        // updating remaining bonus
        remainingBonusData.text = playerScript.playerBonus.ToString();

        // updating total payout
        int totalPayout = salvageCut + playerScript.playerBonus;
        totalPayout += playerScript.hazardPay +
                       playerScript.questPay;
        totalPayoutData.text = totalPayout.ToString();

        char rank = EndGameRank();

        // updating final rank
        finalRankData.text = rank.ToString();

        // this switch is only to determine the voice over, so if our toggle is off we don't need to go through any of this
        if (voiceoversToggle.isOn)
        {
            switch (rank)
            {
                case 'S':
                    levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFinishWithS);

                    if (subtitlesToggle.isOn)
                    {
                        StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.finishWithSRankVoiceLines));
                    }

                    break;

                case 'A':
                    levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFinishWithA);

                    if (subtitlesToggle.isOn)
                    {
                        StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.finishWithARankVoiceLines));
                    }

                    break;

                case 'B':
                    levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFinishWithB);

                    if (subtitlesToggle.isOn)
                    {
                        StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.finishWithBRankVoiceLines));
                    }

                    break;

                case 'C':
                    levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFinishWithC);

                    if (subtitlesToggle.isOn)
                    {
                        StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.finishWithCRankVoiceLines));
                    }

                    break;

                case 'D':
                    levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFinishWithD);

                    if (subtitlesToggle.isOn)
                    {
                        StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.finishWithDRankVoiceLines));
                    }

                    break;

                case 'F':
                    levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOFinishWithF);

                    if (subtitlesToggle.isOn)
                    {
                        StartCoroutine(StartSubtitlesInPausedState(subtitleManager.instance.finishWithFRankVoiceLines));
                    }

                    break;
            }
        }

    }

    public char EndGameRank()
    {
        float totalPossibleSalvage = 50654.0f;

        int percentClear = (int)((playerScript.playerTotalScore / totalPossibleSalvage) * 100);

        if (percentClear > 90)
        {
            return 'S';
        }
        else if (percentClear > 80 && percentClear <= 90)
        {
            return 'A';
        }
        else if (percentClear > 70 && percentClear <= 80)
        {
            return 'B';
        }
        else if (percentClear > 60 && percentClear <= 70)
        {
            return 'C';
        }
        else if (percentClear > 50 && percentClear <= 60)
        {
            return 'D';
        }
        else if (percentClear < 50)
        {
            return 'F';
        }

        else return 'Z';
    }

    public void OpenOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        // changing our sens back to what it was before if we press back or cancel before we save settings
        horizontalSens.value = playerScript.playerCam.GetComponent<cameraControls>().sensHorizontal;
        verticalSens.value = playerScript.playerCam.GetComponent<cameraControls>().sensVertical;

        // updating sens labels
        horSensValue.text = ((int)horizontalSens.value).ToString();
        vertSensValue.text = ((int)verticalSens.value).ToString();

        // if we close our options menu without pressing save, reassign our dynamic FOV toggle based on what is currently in player prefs
        if (PlayerPrefs.GetInt("DynamicFOV") == 1)
        {
            dynamicFOVToggle.isOn = true;
        }
        else
        {
            dynamicFOVToggle.isOn = false;
        }

        // if we close our options menu without pressing save, reassign our dynamic FOV toggle based on what is currently in player prefs
        if (PlayerPrefs.GetInt("UseVoiceovers") == 1)
        {
            voiceoversToggle.isOn = true;
        }
        else
        {
            voiceoversToggle.isOn = false;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicVolume.value = 65;
        }

        if (PlayerPrefs.GetInt("Subtitles") == 1)
        {
            subtitlesToggle.isOn = true;
        }
        else
        {
            subtitlesToggle.isOn = false;
        }

        musicVolumeValue.text = musicVolume.value.ToString();

        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void CloseControls()
    {
        ControlsSplashFromOptions.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void SaveSettingsOptionsMenu()
    {
        // saving our camera sensitivity when we press the save settings button
        // the value we are actually setting here is the sensitivity multiplier in our camera controller script
        playerScript.playerCam.GetComponent<cameraControls>().sensHorizontal = (int)horizontalSens.value;
        playerScript.playerCam.GetComponent<cameraControls>().sensVertical = (int)verticalSens.value;

        PlayerPrefs.SetInt("HorizontalSensitivity", (int)horizontalSens.value);
        PlayerPrefs.SetInt("VerticalSensitivity", (int)verticalSens.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        levelAudioManager.instance.musicAudioSource.volume = musicVolume.value / 100;

        // if our dynamic fov is selected, set the key value to 1
        if (dynamicFOVToggle.isOn)
        {
            PlayerPrefs.SetInt("DynamicFOV", 1);
        }
        // else set it to 0
        else
        {
            PlayerPrefs.SetInt("DynamicFOV", 0);
        }

        // if our use voiceovers is selected, set the key value to 1
        if (voiceoversToggle.isOn)
        {
            PlayerPrefs.SetInt("UseVoiceovers", 1);
        }
        // else set it to 0
        else
        {
            PlayerPrefs.SetInt("UseVoiceovers", 0);
        }

        // if our subtitles is selected, set the key value to 1
        if (subtitlesToggle.isOn)
        {
            PlayerPrefs.SetInt("Subtitles", 1);
        }
        // else set it to 0
        else
        {
            PlayerPrefs.SetInt("Subtitles", 0);
        }

        CloseOptionsMenu();
    }
    public void ViewControls()
    {
        optionsMenu.SetActive(false);
        ControlsSplashFromOptions.SetActive(true);
    }
    public void OpenPlayerStatsMenu()
    {
        craftingMenu.SetActive(false);
        playerStatsScreen.SetActive(true);
        SetPlayerStats();
    }
    public void ClosePlayerStatsMenu()
    {
        playerStatsScreen.SetActive(false);
        craftingMenu.SetActive(true);

    }
    public void SetPlayerStats()
    {
        healthValue.text = playerScript.HP.ToString();
        maxHealthValue.text = playerScript.HPMax.ToString();
        if (playerScript.shielded)
        {
            shieldRechargeValue.text = playerScript.shieldRate.ToString();
            shieldHealthValue.text = playerScript.shieldValue.ToString();
        }
        else
        {
            shieldRechargeValue.text = ("Shield Not Found");
            shieldHealthValue.text = ("Shield Not Found");
        }
        jetpackRechargeValue.text = (playerScript.fuelRefillRate * 100).ToString();

        jetpackConsumptionValue.text = (playerScript.fuelConsumptionRate * 100).ToString();
        DamageValue.text = playerScript.shootDamage.ToString();
        rateOfFireValue.text = playerScript.shootRate.ToString();
        salvageSpreadValue.text = playerScript.salvageSpread.ToString();
        salvageRangeValue.text = playerScript.salvageRange.ToString();
        if (playerScript.salvDetector)
        {
            salvageDetectorCondition.text = ("Active");
        }
        else
        {
            salvageDetectorCondition.text = ("Inactive");
        }
        staminaDrainValue.text = ((int)playerScript.staminaDrain * 100).ToString();
        staminaRegenValue.text = ((int)playerScript.staminaRefillRate * 100).ToString();

    }
    public void TurnOnInventoryUI()
    {
        //activate the Inventory UI
        InventroyParent.SetActive(true);
    }
    public void TurnOffInventoryUI()
    {
        //activate the Inventory UI
        InventroyParent.SetActive(false);
    }
    public void UpdateInventory()
    {
        LevelScrapCollected.text = amtSalvaged.ToString();
        //if (gameManager.instance.playerScript.salvDetector)
        //{
        //    FloorAvailData.text = playerScript.totalLevelSalvage.ToString();
        //}
        //else
        //{
        //    FloorAvailData.text = "Need Salv Detector";
        //}
        SpentScrap.text = spent.ToString();
        BioMassAmt.text = Inventory.Instance._iBioMass.ToString();
        IntactOrganAmt.text = Inventory.Instance._iIntactOrgan.ToString();
        ElectricCompAmt.text = Inventory.Instance._iElectronicComponents.ToString();
        DataCoreAmt.text = Inventory.Instance._iDataProcessingCore.ToString();
        DenseMetalPlateAmt.text = Inventory.Instance._iDenseMetalPlate.ToString();
        HighTensileAlloyAmt.text = Inventory.Instance._iHighTensileAlloyPlate.ToString();
        ValuableLootAmt.text = Inventory.Instance._iValuableLoot.ToString();
        GlassPaneAmt.text = Inventory.Instance._iGlassPane.ToString();
        HPLDAmt.text = Inventory.Instance._iHighPoweredLightDiode.ToString();
        ElectricMotorAmt.text = Inventory.Instance._iElectricMotor.ToString();
        CeramicPlateAmt.text = Inventory.Instance._iCeramicPlate.ToString();
        GoldAlloyAmt.text = Inventory.Instance._iGoldAlloy.ToString();

    }

    #region Message Log functions and classes
    [System.Serializable]
    public class Message
    {
        public string text;
        public TextMeshProUGUI textObject;
    }

    public void SendMessageToLog(string text)
    {
        gamelogMain.SetActive(true);
        if (gamelog.Count > maxMessages)
        {
            Destroy(gamelog[0].textObject.gameObject);
            gamelog.Remove(gamelog[0]);
        }


        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, gamelogPanel.transform);

        //newText.GetComponent<TextMeshProUGUI>().text = text;
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = newMessage.text;


        gamelog.Add(newMessage);
        timeOfLastMessage = Time.fixedTime;
    }
    #endregion

    public IEnumerator StartSubtitles(subtitleManager.VoiceLine[] subtitleSelection)
    {
        yield return new WaitForSeconds(0.6f);

        subtitleParentObject.SetActive(true);

        foreach (var voiceLine in subtitleSelection)
        {
            subtitleText.text = voiceLine.text;

            yield return new WaitForSeconds(voiceLine.time);
        }

        subtitleParentObject.SetActive(false);
    }

    public IEnumerator StartSubtitlesInPausedState(subtitleManager.VoiceLine[] subtitleSelection)
    {
        yield return new WaitForSecondsRealtime(0.6f);

        subtitleParentObject.SetActive(true);

        foreach (var voiceLine in subtitleSelection)
        {
            subtitleText.text = voiceLine.text;

            yield return new WaitForSecondsRealtime(voiceLine.time);
        }

        subtitleParentObject.SetActive(false);
    }
}


