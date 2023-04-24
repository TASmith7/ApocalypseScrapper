using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSceneControl : MonoBehaviour
{
    public static globalSceneControl Instance;

    #region Mission Start player stats. 
    // <summary>
    //To be used of player restarts mission. Hardcoded for demo, but designed to
    //be0 updated at end of mission or when buying permanent upgrades at hub.
    // </summary>



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
    public float shieldMax;
    public int   shieldCD;

    #endregion

    private void Awake()
    {
        if (Instance == null) 
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) 
            { Destroy(gameObject); }
    }
}
