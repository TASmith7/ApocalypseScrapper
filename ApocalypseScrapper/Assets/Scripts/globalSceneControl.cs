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
    //be0 updated at end of mission or when buying permanent upgrades at hub.
    // </summary>
    public int   _MSHP = 100;
    public int   _MSHPMax = 100;

    public float _MSsalvageRate = 1;
    public int   _MSsalvageRange = 5;

    public float _MSthrustPower = 8;
    public float _MSfuelConsumptionRate = 0.5f;
    public float _MSfuelRefillRate = 0.2f;

    public int   _MSshootDamage = 1;
    public float _MSshootRate = 0.33f;
    public int   _MSshootDistance = 100;

    public bool  _MSsalvDetector = false;
    public bool  _MSshielded = false;
    public int   _MSshieldValue = 0;
    public int _MSshieldMax = 0;
    public int   _MSshieldCD = 5;
    public int _MSplayerTotalScore = 0;
    public int _MSplayerBonus = 0;



    #endregion

    #region Player statistics to be ported to next level

    public int   HP;
    public int   HPMax;
    
    public float salvageRate;
    public int   salvageRange;

    public float thrustPower;
    public float fuelConsumptionRate;
    public float fuelRefillRate;

    public int   shootDamage;
    public float shootRate;
    public int   shootDistance;

    public bool  salvDetector;
    public bool  shielded;
    public int   shieldValue;
    public int shieldMax;
    public int   shieldCD;

    public int playerTotalScore;
    public int playerBonus;

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
            { Destroy(gameObject); }

        

        // setting our current scene
        currentScene = SceneManager.GetActiveScene();

        if (currentScene == SceneManager.GetSceneByName("Lvl 1"))
        {
            //Reset player stats to mission start values
            HP = _MSHP;
            HPMax = _MSHPMax;

            salvageRate = _MSsalvageRate;
            salvageRange = _MSsalvageRange;

            thrustPower = _MSthrustPower;
            fuelConsumptionRate = _MSfuelConsumptionRate;
            fuelRefillRate = _MSfuelRefillRate;

            shootDamage = _MSshootDamage;
            shootRate = _MSshootRate;             
            shootDistance = _MSshootDistance;

            salvDetector = _MSsalvDetector;
            shielded = _MSshielded;
            shieldValue = _MSshieldValue;
            shieldMax = _MSshieldMax;
            shieldCD = _MSshieldCD;
            
            playerTotalScore = _MSplayerTotalScore;
            playerBonus = _MSplayerBonus;

        }
    }
}
