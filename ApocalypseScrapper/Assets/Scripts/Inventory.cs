using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

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
}
