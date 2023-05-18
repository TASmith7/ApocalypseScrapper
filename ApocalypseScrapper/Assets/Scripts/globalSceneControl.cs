using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class globalSceneControl : MonoBehaviour
{
    public static globalSceneControl Instance;

    #region Mission Start player stats. 
    // <summary>
    //To be used of player restarts mission. Hardcoded for demo, but designed to
    //be updated at end of mission or when buying permanent upgrades at hub.
    // </summary>
    public int   _MSHP = 100;
    public int   _MSHPMax = 100;

    public float _MSsalvageRate = 1.0f;
    public int   _MSsalvageRange = 5;
    //public float _MSsalvageSpread = 0.001f;

    public float _MSthrustPower = 8;
    public float _MSfuelConsumptionRate = 0.5f;
    public float _MSfuelRefillRate = 0.2f;

    public float _MSstaminaDrain = 0.296f;
    public float _MSstaminaRegen = 0.1f;

    public int   _MSshootDamage = 1;
    public float _MSshootRate = 0.33f;
    public int   _MSshootDistance = 100;
    public float _MSshootSpread = 0.75f;

    public bool  _MSsalvDetector = false;
    public bool  _MSshielded = false;
    public int   _MSshieldValue = 0;
    public int   _MSshieldMax = 0;
    public int   _MSshieldCD = 5;
    public int   _MSshieldRate = 1;

    public int _MSplayerScrapCollected = 0;
    public int _MSspentScrap = 0;
    public int _MSquestPay = 0;
    public int _MShazardPay = 0;
    //public int _MSplayerBonus = 0;

    public int _MStotalScrapAvailable = 0;



    #endregion

    #region Player statistics to be ported to next level

    public int   HP = 100;
    public int   HPMax = 100;
    
    public float salvageRate = 0.75f;
    public int   salvageRange = 5;
    public float salvageSpread = 0.01f;

    public float staminaDrain = 0.296f;
    public float staminaRegen = 0.1f;

    public float thrustPower = 8;
    public float fuelConsumptionRate = 0.5f;
    public float fuelRefillRate = 0.2f;

    public int   shootDamage = 1;
    public float shootRate = 0.33f;
    public int   shootDistance = 100;

    public bool  salvDetector = false;
    public bool  shielded = false;
    public int   shieldValue = 0;
    public int   shieldMax = 0;
    public int   shieldCD = 5;
    public int   shieldRate;

    public int playerScrapCollected = 0;
    //public int playerBonus = 0;
    public int spentScrap = 0;
    public int questPay = 0;



    #endregion

    private Scene currentScene;

    private void Awake()
    {
        if (Instance == null) 
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) 
            { Destroy(gameObject); 
        }

        

        // setting our current scene
        currentScene = SceneManager.GetActiveScene();
    }

    private void Start()
    {
        if (currentScene == SceneManager.GetSceneByName("Lvl 1"))
        {
            DefaultStats();
        }
    }

    public void DefaultStats()
    {
        playerController.HP = _MSHP;
        playerController.HPMax = _MSHPMax;
        playerController.salvageRate = _MSsalvageRate;
        playerController.salvageRange = _MSsalvageRange;
        playerController.thrustPower = _MSthrustPower;
        playerController.fuelConsumptionRate = _MSfuelConsumptionRate;
        playerController.fuelRefillRate = _MSfuelRefillRate;
        playerController.staminaDrain = _MSstaminaDrain;
        playerController.staminaRefillRate = _MSstaminaRegen;
        playerController.shootDamage = _MSshootDamage;
        playerController.shootRate = _MSshootRate;
        playerController.salvDetector = _MSsalvDetector;
        playerController.shielded = _MSshielded;
        playerController.shieldValue = _MSshieldValue;
        playerController.shieldMax = _MSshieldMax;
        playerController.shieldCD = _MSshieldCD;
        playerController.shieldRate = _MSshieldRate;
        playerController.playerTotalSalvage = _MSplayerScrapCollected;
        playerController.spent = _MSspentScrap;
        playerController.questPay = _MSquestPay;
        playerController.hazardPay = _MShazardPay;
        playerController.shootSpread = _MSshootSpread;
        Inventory.Instance.InvDefault();
}
}
