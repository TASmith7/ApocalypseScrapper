using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("----- PLayer Inventory -----")]
    public static int _iBioMass = 0;
    public static int _iIntactOrgan = 0;
    public static int _iElectronicComponents = 0;
    public static int _iDataProcessingCore = 0;
    public static int _iDenseMetalPlate = 0;
    public static int _iHighTensileAlloyPlate = 0;
    public static int _iGlassPane = 0;
    public static int _iHighPoweredLightDiode = 0;
    public static int _iElectricMotor = 0;
    public static int _iCeramicPlate = 0;
    public static int _iGoldAlloy = 0;
    public static int _iValuableLoot = 0;

    //[Header("----- PLayer Inventory Snapshot for level reset -----")]
    //public static int _sBioMass;
    //public static int _sIntactOrgan;
    //public static int _sElectronicComponents;
    //public static int _sDataProcessingCore;
    //public static int _sDenseMetalPlate;
    //public static int _sHighTensileAlloyPlate;
    //public static int _sGlassPane;
    //public static int _sHighPoweredLightDiode;
    //public static int _sElectricMotor;
    //public static int _sCeramicPlate;
    //public static int _sGoldAlloy;
    //public static int _sValuableLoot;

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
        //Older snapshot method (before using PlayerPrefs)
        //_sBioMass = _iBioMass;
        //_sIntactOrgan = _iIntactOrgan;
        //_sElectronicComponents = _iElectronicComponents;
        //_sDataProcessingCore = _iDataProcessingCore;
        //_sDenseMetalPlate = _iDenseMetalPlate;
        //_sHighTensileAlloyPlate = _iHighTensileAlloyPlate;
        //_sGlassPane = _iGlassPane;
        //_sHighPoweredLightDiode = _iHighPoweredLightDiode;
        //_sElectricMotor = _iElectricMotor;
        //_sCeramicPlate = _iCeramicPlate;
        //_sGoldAlloy = _iGoldAlloy;
        //_sValuableLoot = _iValuableLoot;

        int levelToSaveTo = gameManager.instance.level + 1;
        PlayerPrefs.SetInt(levelToSaveTo + "BioMass", _iBioMass);
        PlayerPrefs.SetInt(levelToSaveTo + "IntactOrgan", _iIntactOrgan);
        PlayerPrefs.SetInt(levelToSaveTo + "ElectronicComponents", _iElectronicComponents);
        PlayerPrefs.SetInt(levelToSaveTo + "DataProcessingCore", _iDataProcessingCore);
        PlayerPrefs.SetInt(levelToSaveTo + "DenseMetalPlate", _iDenseMetalPlate);
        PlayerPrefs.SetInt(levelToSaveTo + "HighTensileAlloyPlate", _iHighTensileAlloyPlate);
        PlayerPrefs.SetInt(levelToSaveTo + "GlassPane", _iGlassPane);
        PlayerPrefs.SetInt(levelToSaveTo + "HighPoweredLightDiode", _iHighPoweredLightDiode);
        PlayerPrefs.SetInt(levelToSaveTo + "ElectricMotor", _iElectricMotor);
        PlayerPrefs.SetInt(levelToSaveTo + "CeramicPlate" , _iCeramicPlate);
        PlayerPrefs.SetInt(levelToSaveTo + "GoldAlloy" , _iGoldAlloy);
        PlayerPrefs.SetInt(levelToSaveTo + "ValuableLoot" , _iValuableLoot);
    }

    public void InvLoad(int level)
    {
        _iBioMass = PlayerPrefs.GetInt(level+ "BioMass");
        _iIntactOrgan = PlayerPrefs.GetInt(level + "IntactOrgan");
        _iElectronicComponents = PlayerPrefs.GetInt(level + "ElectronicComponents");
        _iDataProcessingCore = PlayerPrefs.GetInt(level + "DataProcessingCore");
        _iDenseMetalPlate = PlayerPrefs.GetInt(level + "DenseMetalPlate");
        _iHighTensileAlloyPlate = PlayerPrefs.GetInt(level + "HighTensileAlloyPlate");
        _iGlassPane = PlayerPrefs.GetInt(level + "GlassPane");
        _iHighPoweredLightDiode = PlayerPrefs.GetInt(level + "HighPoweredLightDiode");
        _iElectricMotor = PlayerPrefs.GetInt(level + "ElectricMotor");
        _iCeramicPlate = PlayerPrefs.GetInt(level + "CeramicPlate");
        _iGoldAlloy = PlayerPrefs.GetInt(level + "GoldAlloy");
        _iValuableLoot = PlayerPrefs.GetInt(level + "ValuableLoot");
    }

    public void InvDefault()
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
        
    }

    #endregion
}
