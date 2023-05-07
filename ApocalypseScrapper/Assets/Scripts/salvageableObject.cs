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
    [Range(0, 5)][SerializeField] public int _rollDPC;
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
        if (_rollBM > 0 && _dcBioMass > 0)
        {
            int found=0;
            while (_rollBM > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcBioMass) { found++; }
                _rollBM--;
            }
            if (found > 0) { Inventory.Instance.AddBM(found); }
            //msg
        }

        if (_rollIO > 0 && _dcIntactOrgan > 0)
        {
            int found = 0;
            while (_rollIO > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcIntactOrgan) { found++; }
                _rollIO--;
            }
            if (found > 0) { Inventory.Instance.AddIO(found); }
            //msg
        }

        if (_rollEC > 0 && _dcElectronicComponents > 0)
        {
            int found = 0;
            while (_rollEC > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcElectronicComponents) { found++; }
                _rollEC--;
            }
            if (found > 0) { Inventory.Instance.AddEC(found); }
            //msg
        }

        if (_rollDPC > 0 && _dcDataProcessingCore > 0)
        {
            int found = 0;
            while (_rollDPC > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcDataProcessingCore) { found++; }
                _rollDPC--;
            }
            if (found > 0) { Inventory.Instance.AddDPC(found); }
            //msg
        }

        if (_rollDMP > 0 && _dcDenseMetalPlate > 0)
        {
            int found = 0;
            while (_rollDMP > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcDenseMetalPlate) { found++; }
                _rollDMP--;
            }
            if (found > 0) { Inventory.Instance.AddDMP(found); }
            //msg
        }

        if (_rollHTAP > 0 && _dcHighTensileAlloyPlate > 0)
        {
            int found = 0;
            while (_rollHTAP > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcHighTensileAlloyPlate) { found++; }
                _rollHTAP--;
            }
            if (found > 0) { Inventory.Instance.AddHTAP(found); }
            //msg
        }

        if (_rollGP > 0 && _dcGlassPane > 0)
        {
            int found = 0;
            while (_rollGP > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcGlassPane) { found++; }
                _rollGP--;
            }
            if (found > 0) { Inventory.Instance.AddGP(found); }
            //msg
        }

        if (_rollHPLD > 0 && _dcHighPoweredLightDiode > 0)
        {
            int found = 0;
            while (_rollHPLD > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcHighPoweredLightDiode) { found++; }
                _rollHPLD--;
            }
            if (found > 0) { Inventory.Instance.AddHPLD(found); }
            //msg
        }

        if (_rollEM > 0 && _dcElectricMotor > 0)
        {
            int found = 0;
            while (_rollEM > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcElectricMotor) { found++; }
                _rollEM--;
            }
            if (found > 0) { Inventory.Instance.AddEM(found); }
            //msg
        }

        if (_rollCP > 0 && _dcCeramicPlate > 0)
        {
            int found = 0;
            while (_rollCP > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcCeramicPlate) { found++; }
                _rollCP--;
            }
            if (found > 0) { Inventory.Instance.AddCP(found); }
            //msg
        }

        if (_rollGA > 0 && _dcGoldAlloy > 0)
        {
            int found = 0;
            while (_rollGA > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcGoldAlloy) { found++; }
                _rollGA--;
            }
            if (found > 0) { Inventory.Instance.AddGA(found); }
            //msg
        }

        if (_rollVL > 0 && _dcValuableLoot > 0)
        {
            int found = 0;
            while (_rollVL > 0)
            {
                int roll = Random.Range(1, 100);
                if (roll <= _dcValuableLoot) { found++; }
                _rollVL--;
            }
            if (found > 0) { Inventory.Instance.AddVL(found); }
            //msg
        }
    }
}
