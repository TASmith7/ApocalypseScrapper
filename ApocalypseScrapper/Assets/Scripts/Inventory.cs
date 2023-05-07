using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("----- PLayer Inventory -----")]
    public int _iBioMass = 0;
    public int _iIntactOrgan = 0;
    public int _iElectronicComponents = 0;
    public int _iDataProcessingCore = 0;
    public int _iDenseMetalPlate = 0;
    public int _iHighTensileAlloyPlate = 0;
    public int _iGlassPane = 0;
    public int _iHighPoweredLightDiode = 0;
    public int _iElectricMotor = 0;
    public int _iCeramicPlate = 0;
    public int _iGoldAlloy = 0;
    public int _iValuableLoot = 0;

    [Header("----- PLayer Inventory Snapshot for level reset -----")]
    public int _sBioMass;
    public int _sIntactOrgan;
    public int _sElectronicComponents;
    public int _sDataProcessingCore;
    public int _sDenseMetalPlate;
    public int _sHighTensileAlloyPlate;
    public int _sGlassPane;
    public int _sHighPoweredLightDiode;
    public int _sElectricMotor;
    public int _sCeramicPlate;
    public int _sGoldAlloy;
    public int _sValuableLoot;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

   

    #region Add/Remove functions for components in inventory
    public void AddBM(int BM) { _iBioMass += BM; }
    public void RemBM(int BM) { _iBioMass -= BM; }
    public void AddIO(int IO) { _iIntactOrgan += IO; }
    public void RemIO(int IO) { _iIntactOrgan -= IO;}
    public void AddEC(int EC) { _iElectronicComponents += EC; }
    public void RemEC(int EC) { _iElectronicComponents -= EC; }
    public void AddDPC(int DPC) { _iDataProcessingCore += DPC; }
    public void RemDPC(int DPC) { _iDataProcessingCore -= DPC; }
    public void AddDMP(int DMP) { _iDenseMetalPlate += DMP; }
    public void RemDMP(int DMP) { _iDenseMetalPlate -= DMP; }
    public void AddHTAP(int HTAP) { _iHighTensileAlloyPlate += HTAP; }
    public void RemHTAP(int HTAP) { _iHighTensileAlloyPlate -= HTAP; }
    public void AddGP(int GP) { _iGlassPane += GP; }
    public void RemGP(int GP) { _iGlassPane -= GP; }
    public void AddHPLD(int HPLD) { _iHighPoweredLightDiode += HPLD; }
    public void RemHPLD(int HPLD) { _iHighPoweredLightDiode -= HPLD; }
    public void AddEM(int EM) { _iElectricMotor += EM; }
    public void RemEM(int EM) { _iElectricMotor -= EM; }
    public void AddCP(int CP) { _iCeramicPlate += CP; }
    public void RemCP(int CP) { _iCeramicPlate -= CP; }
    public void AddGA(int GA) { _iGoldAlloy += GA; }
    public void RemGA(int GA) { _iGoldAlloy -= GA; }
    public void AddVL(int VL) { _iValuableLoot += VL; }
    public void RemVL(int VL) { _iValuableLoot -= VL; }
    #endregion

    #region Snapshot functions for level and mission restarts
    //Use at level load to save status of inventory for rolling back on level restart
    public void InvSnapshot()
    {
        _sBioMass = _iBioMass;
        _sIntactOrgan = _iIntactOrgan;
        _sElectronicComponents = _iElectronicComponents;
        _sDataProcessingCore = _iDataProcessingCore;
        _sDenseMetalPlate = _iDenseMetalPlate;
        _sHighTensileAlloyPlate = _iHighTensileAlloyPlate;
        _sGlassPane = _iGlassPane;
        _sHighPoweredLightDiode = _iHighPoweredLightDiode;
        _sElectricMotor = _iElectricMotor;
        _sCeramicPlate = _iCeramicPlate;
        _sGoldAlloy = _iGoldAlloy;
        _sValuableLoot = _iValuableLoot;
    }

    public void InvLevelRestart()
    {
        _iBioMass = _sBioMass;
        _iIntactOrgan = _sIntactOrgan;
        _iElectronicComponents = _sElectronicComponents;
        _iDataProcessingCore = _sDataProcessingCore;
        _iDenseMetalPlate = _sDenseMetalPlate;
        _iHighTensileAlloyPlate = _sHighTensileAlloyPlate;
        _iGlassPane = _sGlassPane;
        _iHighPoweredLightDiode = _sHighPoweredLightDiode;
        _iElectricMotor = _sElectricMotor;
        _iCeramicPlate = _sCeramicPlate;
        _iGoldAlloy = _sGoldAlloy;
        _iValuableLoot = _sValuableLoot;
    }

    public void InvMissionRestart()
    {
        _iBioMass = 0;
        _iIntactOrgan = 0;
        _iElectronicComponents = 0;
        _iDataProcessingCore = 0;
        _iDenseMetalPlate = 0;
        _iHighTensileAlloyPlate = 0;
        _iGlassPane = 0;
        _iHighPoweredLightDiode = 0;
        _iElectricMotor = 0;
        _iCeramicPlate = 0;
        _iGoldAlloy = 0;
        _iValuableLoot = 0;
        InvSnapshot();
    }

    #endregion
}
