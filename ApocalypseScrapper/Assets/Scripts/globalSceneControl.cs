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
    public int   _MSshieldMax = 0;
    public int   _MSshieldCD = 5;
    public int   _MSshieldRate = 1;

    public int   _MSplayerTotalScore = 0;
    public int   _MSplayerBonus = 0;



    #endregion

    #region Player statistics to be ported to next level

    public int   HP = 100;
    public int   HPMax = 100;
    
    public float salvageRate = 1;
    public int   salvageRange = 5;

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

    public int playerTotalScore = 0;
    public int playerBonus = 0;

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
            shieldRate = _MSshieldRate;

            playerTotalScore = _MSplayerTotalScore;
            playerBonus = _MSplayerBonus;
            
        }
    }
}
