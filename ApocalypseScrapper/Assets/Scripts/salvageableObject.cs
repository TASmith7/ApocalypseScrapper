using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class salvageableObject : MonoBehaviour, ISalvageable
{
    [SerializeField] public float salvageValue;
    public float salvageTime;

    [Header("----- Component Drop Chances -----")]
    [Range(0.0f, 5.0f)][SerializeField] public float _dcBioMass;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcIntactOrgan;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcElectronicComponents;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcDataProcessingCore;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcDenseMetalPlate;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcHighTensileAlloyPlate;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcGlassPane;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcHighPoweredLightDiode;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcElectricMotor;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcCeramicPlate;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcGoldAlloy;
    [Range(0.0f, 5.0f)][SerializeField] public float _dcValuableLoot;


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
