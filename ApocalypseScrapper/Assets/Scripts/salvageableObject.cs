using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class salvageableObject : MonoBehaviour, ISalvageable
{

    [SerializeField] public float salvageValue;
    public float salvageTime;
    
    [Header("----- Component Drop Chance % and # of Rolls-----")]
    [Range(0, 100)][SerializeField] public int _dcBioMass;
    [Range(0, 5)][SerializeField] public int _rollBM;
    [Range(0, 100)][SerializeField] public int _dcIntactOrgan;
    [Range(0, 5)][SerializeField] public int _rollIO;
    [Range(0, 100)][SerializeField] public int _dcElectronicComponents;
    [Range(0, 5)][SerializeField] public int _rollEC;
    [Range(0, 100)][SerializeField] public int _dcDataProcessingCore;
    [Range(0, 5)][SerializeField] public int _rollDCP;
    [Range(0, 100)][SerializeField] public int _dcDenseMetalPlate;
    [Range(0, 5)][SerializeField] public int _rollDMP;
    [Range(0, 100)][SerializeField] public int _dcHighTensileAlloyPlate;
    [Range(0, 5)][SerializeField] public int _rollHTAP;
    [Range(0, 100)][SerializeField] public int _dcGlassPane;
    [Range(0, 5)][SerializeField] public int _rollGP;
    [Range(0, 100)][SerializeField] public int _dcHighPoweredLightDiode;
    [Range(0, 5)][SerializeField] public int _rollHPLD;
    [Range(0, 100)][SerializeField] public int _dcElectricMotor;
    [Range(0, 5)][SerializeField] public int _rollEM;
    [Range(0, 100)][SerializeField] public int _dcCeramicPlate;
    [Range(0, 5)][SerializeField] public int _rollCP;
    [Range(0, 100)][SerializeField] public int _dcGoldAlloy;
    [Range(0, 5)][SerializeField] public int _rollGA;
    [Range(0, 100)][SerializeField] public int _dcValuableLoot;
    [Range(0, 5)][SerializeField] public int _rollVL;
    

    void Start()
    { 
        gameObject.tag = "Salvage";
        salvageTime = salvageValue / 1000.0f;
        if (salvageTime < 0.5f)
            salvageTime = 0.5f;
    }

    public void AssignDrops()
    {
 
    }
}
