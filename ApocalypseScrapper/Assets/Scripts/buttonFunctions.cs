using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [Header("----- Audio Sources -----")]
    public AudioSource buttonHoverAudioSource;
    public AudioSource buttonClickAudioSource;

    [Header("----- Audio Clips -----")]
    public AudioClip buttonHoverAudio;
    public AudioClip buttonClickAudio;


    [Header("----- Volume -----")]
    [Range(0f, 1.0f)][SerializeField] float buttonHoverVolume;
    [Range(0f, 1.0f)][SerializeField] float buttonClickVolume;


    [Header("----- Pitch -----")]
    [Range(0f, 3.0f)][SerializeField] float buttonClickPitch;
    
    private void Start()
    {
        buttonHoverAudioSource = gameObject.AddComponent<AudioSource>();
        buttonClickAudioSource = gameObject.AddComponent<AudioSource>();


        buttonClickAudioSource.clip = buttonClickAudio;
        buttonHoverAudioSource.clip = buttonHoverAudio;

        buttonClickAudioSource.volume = buttonClickVolume;
        buttonClickAudioSource.pitch = buttonClickPitch;
        buttonClickAudioSource.playOnAwake = false;

        buttonHoverAudioSource.volume = buttonHoverVolume;
        buttonHoverAudioSource.playOnAwake = false;

    }
    public void Resume()
    {
        gameManager.instance.UnpauseState();
        
    }
    public void RestartLevel()
    {
        Inventory.Instance.InvLevelRestart();
        gameManager.instance.playerScript.RestartLevel();
        gameManager.instance.UnpauseState();
        

    }
    public void RestartMission()
    {
        gameManager.instance.UnpauseState();
        Inventory.Instance.InvMissionRestart();
        gameManager.instance.playerScript.RestartMission();
    }
    public void Options()
    {
        gameManager.instance.OpenOptionsMenu();
    }

    public void CancelOptions()
    {
        gameManager.instance.CloseOptionsMenu();
    }

    public void BackOptions()
    {
        gameManager.instance.CloseOptionsMenu();
    }

    public void BackControls()
    {
        gameManager.instance.CloseControls();
    }

    public void SaveOptions()
    {
        gameManager.instance.SaveSettingsOptionsMenu();
    }

    public void ViewControlsOptions()
    {
        gameManager.instance.ViewControls();
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void PlayButtonHoverAudio()
    {
        buttonHoverAudioSource.Play();
    }

    public void PlayButtonClickAudio()
    {
        buttonClickAudioSource.Play();
    }

    public void RoleCredits()
    {
        SceneManager.LoadScene("EndCredits");
    }

    public void LoadLevelButton()
    {
        mainMenuManager.instance.CueLoadLevelMenu();
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Lvl 1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Lvl 2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Lvl 3");
    }
    public void LoadBossLevel()
    {
        SceneManager.LoadScene("Boss Lvl");
    }

    public void BackMainMenu()
    {
        mainMenuManager.instance.CueMainMenuFromLoadLevel();
    }

    public static void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    #region Store Menu Buttons
    public void SmallHeal()
    {
        if (gameManager.instance.amtSalvaged >= 750 && Inventory._iBioMass >= 1 && playerController.HP != playerController.HPMax)
        {
            
            if (playerController.HPMax - playerController.HP <= 25)
            {
                playerController.HP = playerController.HPMax;
            }
            else playerController.HP += 25;

            gameManager.instance.playerScript.PlayerUIUpdate();

            gameManager.instance.amtSalvaged -= 750;
            gameManager.instance.spent += 750;
            Inventory.Instance.RemBM(1);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
        
    }

    public void LargeHeal()
    {
        if (gameManager.instance.amtSalvaged >= 2500 && Inventory._iIntactOrgan>= 2 && playerController.HP != playerController.HPMax)
        {
            
            if (playerController.HPMax - playerController.HP <= 100)
            {
                playerController.HP = playerController.HPMax;
            }
            else playerController.HP += 100;

            gameManager.instance.playerScript.PlayerUIUpdate();

            gameManager.instance.amtSalvaged -= 2500;
            gameManager.instance.spent += 2500;
             Inventory.Instance.RemIO(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }
    public void MaxHealth()
    {
        if (gameManager.instance.amtSalvaged >= 5000&& Inventory._iBioMass>=1&&Inventory._iIntactOrgan>=2)
        {

            playerController.HPMax += 25;
            playerController.HP += 25;
            
            gameManager.instance.amtSalvaged -= 5000;
            gameManager.instance.spent += 5000;
            Inventory.Instance.RemBM(1);
            Inventory.Instance.RemIO(2);
            gameManager.instance.UpdateInventory();

            gameManager.instance.playerScript.PlayerUIUpdate();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void GetShield()
    {
        if (gameManager.instance.amtSalvaged >= 2000&& Inventory._iHighPoweredLightDiode>=2)
        {
            gameManager.instance.playerScript.PlayerUIUpdate();

            playerController.shielded = true;
            playerController.shieldMax += 25;
            playerController.shieldValue += 25;
            gameManager.instance.amtSalvaged -= 2000;
            gameManager.instance.spent += 2000;
            Inventory.Instance.RemHPLD(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void ModShield()
    {
        if (gameManager.instance.amtSalvaged >= 5000 &&Inventory._iElectronicComponents>=2&&Inventory._sDataProcessingCore>=4&& playerController.shielded == true)
        {
            gameManager.instance.playerScript.PlayerUIUpdate();
            
            playerController.shieldRate += 1;
            playerController.shieldCD -= 1;

            gameManager.instance.amtSalvaged -= 5000;
            gameManager.instance.spent += 5000;
            Inventory.Instance.RemEC(2);
            Inventory.Instance.RemDPC(4);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void PlusWeapDmg()
    {
        if (gameManager.instance.amtSalvaged >= 4000 && Inventory._iElectronicComponents >= 2)
        {

            playerController.shootDamage += 1;
            
            gameManager.instance.amtSalvaged -= 4000;
            gameManager.instance.spent += 4000;
            Inventory.Instance.RemEC(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void OverchargeWeapDmg()
    {
        if (gameManager.instance.amtSalvaged >= 3000 && Inventory._iElectricMotor >= 1 && Inventory._iDataProcessingCore >= 1)
        {
            playerController.shootDamage += 5;
            playerController.shootRate += 0.33f;
            
            gameManager.instance.amtSalvaged -= 3000;
            gameManager.instance.spent += 3000;
            Inventory.Instance.RemEC(1);
            Inventory.Instance.RemDPC(1);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void JPFuel()
    {
        if (gameManager.instance.amtSalvaged >= 2000 && Inventory._iHighPoweredLightDiode >= 1 && Inventory._iGoldAlloy >= 2)
        {
            playerController.fuelConsumptionRate -= 0.1f;
            

            gameManager.instance.amtSalvaged -= 2000;
            gameManager.instance.spent += 2000;
            Inventory.Instance.RemHPLD(1);
            Inventory.Instance.RemGA(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void JPRecharge()
    {
        if (gameManager.instance.amtSalvaged >= 4000&&Inventory._iHighTensileAlloyPlate>=1&& Inventory._iGlassPane>=2)
        {
            playerController.fuelRefillRate += 0.1f;
            gameManager.instance.amtSalvaged -= 4000;
            gameManager.instance.spent += 4000;
            Inventory.Instance.RemHTAP(1);
            Inventory.Instance.RemGP(2);
            gameManager.instance.UpdateInventory();

        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    public void GetDetector()
    {
        if (gameManager.instance.amtSalvaged >= 15000&&Inventory._iElectricMotor>=3&& Inventory._iDataProcessingCore>=4)
        {
            playerController.salvDetector = true;
                
            gameManager.instance.amtSalvaged -= 15000;
            gameManager.instance.spent += 15000;
            Inventory.Instance.RemEM(3);
            Inventory.Instance.RemDPC(4);
            gameManager.instance.UpdateInventory();

        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }
    public void UpgradeSalvageRange()
    {
        if(gameManager.instance.amtSalvaged >=3000&& Inventory._iElectricMotor>=2)
        {
            playerController.salvageRange += 1;
            gameManager.instance.amtSalvaged -= 3000;
            gameManager.instance.spent += 3000;
            Inventory.Instance.RemEM(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }
    //public void UpgradeSalvageSpread()
    //{
    //    if (gameManager.instance.amtSalvaged >= 1000&&Inventory.Instance._iElectricMotor>=4&&Inventory.Instance._iDataProcessingCore>=8)
    //    {
            
    //        gameManager.instance.playerScript.salvageSpread += .03f;
    //        gameManager.instance.amtSalvaged -= 400;
    //        gameManager.instance.spent += 400;
    //    }
    //    else
    //    {
    //        StartCoroutine(DeclinedPurchase());
    //    }
    //}
    public void UpgradeSalvageEfficiency()
    {
        if (gameManager.instance.amtSalvaged >=4000&&Inventory._iDataProcessingCore>=2&&Inventory._iElectronicComponents>=2)
        {
            
            playerController.salvageRate += 0.1f;
            gameManager.instance.amtSalvaged -= 4000;
            gameManager.instance.spent += 4000;
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }
    
    public void UpgradeStaminaEfficiency()
    {
        if(gameManager.instance.amtSalvaged >=4000&&Inventory._iElectricMotor>=2&&Inventory._iElectronicComponents>=2)
        {
            
            playerController.staminaDrain -= 0.2f;
            gameManager.instance.amtSalvaged -= 4000;
            gameManager.instance.spent += 4000;
            Inventory.Instance.RemEM(2);
            Inventory.Instance.RemEC(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }
    public void UpgradeStaminaRecharge()
    {
        if (gameManager.instance.amtSalvaged >= 2000&&Inventory._iCeramicPlate>=2&&Inventory._iElectricMotor>=2)
        {
            playerController.staminaRefillRate += 0.2f;
            gameManager.instance.amtSalvaged -= 2000;
            gameManager.instance.spent += 2000;
            Inventory.Instance.RemCP(2);
            Inventory.Instance.RemEC(2);
            gameManager.instance.UpdateInventory();
        }
        else
        {
            StartCoroutine(DeclinedPurchase());
        }
    }

    #endregion


    IEnumerator DeclinedPurchase()
    {
       
        gameManager.instance.DeclinedPurchasePopUp.SetActive(true);
        Debug.Log("Declined True");
        Debug.Log("WFS Started");
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("WFS Over");
        gameManager.instance.DeclinedPurchasePopUp.SetActive(false);
        Debug.Log("Declined False");
       

    }
}


