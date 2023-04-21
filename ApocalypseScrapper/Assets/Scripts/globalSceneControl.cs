using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSceneControl : MonoBehaviour
{
    public static globalSceneControl Instance;


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
