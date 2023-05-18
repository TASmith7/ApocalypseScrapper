using System.Collections;
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
        Inventory.Instance.InvLoad(gameManager.instance.level);
        gameManager.instance.playerScript.RestartLevel();
        gameManager.instance.UnpauseState();


    }
    public void RestartMission()
    {
        gameManager.instance.UnpauseState();
        Inventory.Instance.InvDefault();
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
        if (playerController.playerTotalSalvage >= 750 && Inventory._iBioMass >= 1 && playerController.HP != playerController.HPMax)
        {

            if (playerController.HPMax - playerController.HP <= 25)
            {
                playerController.HP = playerController.HPMax;
            }
            else playerController.HP += 25;

            gameManager.instance.playerScript.PlayerUIUpdate();

            playerController.playerTotalSalvage -= 750;
            playerController.spent += 750;
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
        if (playerController.playerTotalSalvage >= 2500 && Inventory._iIntactOrgan >= 2 && playerController.HP != playerController.HPMax)
        {

            if (playerController.HPMax - playerController.HP <= 100)
            {
                playerController.HP = playerController.HPMax;
            }
            else playerController.HP += 100;

            gameManager.instance.playerScript.PlayerUIUpdate();

            playerController.playerTotalSalvage -= 2500;
            playerController.spent += 2500;
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
        if (playerController.HPMax >= 250)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {


            if (playerController.playerTotalSalvage >= 5000 && Inventory._iBioMass >= 1 && Inventory._iIntactOrgan >= 2)
            {

                playerController.HPMax += 25;
                playerController.HP += 25;

                playerController.playerTotalSalvage -= 5000;
                playerController.spent += 5000;
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
    }

    public void GetShield()
    {
        if (playerController.shieldMax >= 150)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 2000 && Inventory._iHighPoweredLightDiode >= 2)
            {
                gameManager.instance.playerScript.PlayerUIUpdate();

                playerController.shielded = true;
                playerController.shieldMax += 25;
                playerController.shieldValue += 25;
                playerController.playerTotalSalvage -= 2000;
                playerController.spent += 2000;
                Inventory.Instance.RemHPLD(2);
                gameManager.instance.UpdateInventory();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void ModShield()
    {
        if (playerController.shieldCD <= 0)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 5000 && Inventory._iElectronicComponents >= 2 && Inventory._iDataProcessingCore >= 4 && playerController.shielded == true)
            {
                gameManager.instance.playerScript.PlayerUIUpdate();

                playerController.shieldRate += 1;
                playerController.shieldCD -= 1;

                playerController.playerTotalSalvage -= 5000;
                playerController.spent += 5000;
                Inventory.Instance.RemEC(2);
                Inventory.Instance.RemDPC(4);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void PlusWeapDmg()
    {
        if (playerController.shootDamage >= 15)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 10000 && Inventory._iElectronicComponents >= 2)
            {

                playerController.shootDamage += 1;

                playerController.playerTotalSalvage -= 10000;
                playerController.spent += 10000;
                Inventory.Instance.RemEC(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void OverchargeWeapDmg()
    {
        if (playerController.shootRate <= .03f)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {


            if (playerController.playerTotalSalvage >= 5000 && Inventory._iElectricMotor >= 3 && Inventory._iDataProcessingCore >= 3 && Inventory._iGoldAlloy >= 2 && playerController.shootRate >= 0.1f)
            {
                playerController.shootSpread -= .15f;
                playerController.shootRate -= 0.05f;

                playerController.playerTotalSalvage -= 5000;
                playerController.spent += 5000;
                Inventory.Instance.RemEC(3);
                Inventory.Instance.RemDPC(3);
                Inventory.Instance.RemGA(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void JPFuel()
    {
        if (playerController.fuelConsumptionRate <= 0.1f)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 2000 && Inventory._iHighPoweredLightDiode >= 1 && Inventory._iGoldAlloy >= 2)
            {
                playerController.fuelConsumptionRate -= 0.1f;


                playerController.playerTotalSalvage -= 2000;
                playerController.spent += 2000;
                Inventory.Instance.RemHPLD(1);
                Inventory.Instance.RemGA(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void JPRecharge()
    {
        if (playerController.fuelRefillRate >= 1)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 4000 && Inventory._iHighTensileAlloyPlate >= 1 && Inventory._iGlassPane >= 2)
            {
                playerController.fuelRefillRate += 0.1f;
                playerController.playerTotalSalvage -= 4000;
                playerController.spent += 4000;
                Inventory.Instance.RemHTAP(1);
                Inventory.Instance.RemGP(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();

            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void GetDetector()
    {
        if (playerController.salvDetector == true)
        {
            StartCoroutine(SalvDetectorBought());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 15000 && Inventory._iElectricMotor >= 3 && Inventory._iDataProcessingCore >= 4)
            {
                playerController.salvDetector = true;

                playerController.playerTotalSalvage -= 15000;
                playerController.spent += 15000;
                Inventory.Instance.RemEM(3);
                Inventory.Instance.RemDPC(4);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();

            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }
    public void UpgradeSalvageRange()
    {
        if (playerController.salvageRange >= 25)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 3000 && Inventory._iElectricMotor >= 2)
            {
                playerController.salvageRange += 1;
                playerController.playerTotalSalvage -= 3000;
                playerController.spent += 3000;
                Inventory.Instance.RemEM(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
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
        if (playerController.salvageRate <= 0.1f)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 4000 && Inventory._iDataProcessingCore >= 2 && Inventory._iElectronicComponents >= 2 && playerController.salvageRate > 0.1f)
            {

                playerController.salvageRate -= 0.1f;
                playerController.playerTotalSalvage -= 4000;
                playerController.spent += 4000;
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }

    public void UpgradeStaminaEfficiency()
    {
        if (playerController.staminaDrain <= 0.2f)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 4000 && Inventory._iElectricMotor >= 2 && Inventory._iElectronicComponents >= 2)
            {

                playerController.staminaDrain -= 0.2f;
                playerController.playerTotalSalvage -= 4000;
                playerController.spent += 4000;
                Inventory.Instance.RemEM(2);
                Inventory.Instance.RemEC(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
        }
    }
    public void UpgradeStaminaRecharge()
    {
        if (playerController.staminaRefillRate >= 1)
        {
            StartCoroutine(MaxLevelReached());
        }
        else
        {
            if (playerController.playerTotalSalvage >= 2000 && Inventory._iCeramicPlate >= 2 && Inventory._iElectricMotor >= 2)
            {
                playerController.staminaRefillRate += 0.2f;
                playerController.playerTotalSalvage -= 2000;
                playerController.spent += 2000;
                Inventory.Instance.RemCP(2);
                Inventory.Instance.RemEC(2);
                gameManager.instance.UpdateInventory();
                gameManager.instance.UpdateSalvageScore();
            }
            else
            {
                StartCoroutine(DeclinedPurchase());
            }
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
    IEnumerator MaxLevelReached()
    {
        gameManager.instance.MaxLevelReachedPopUp.SetActive(true);
        Debug.Log("Max Level True");
        Debug.Log("WFS Started");
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("WFS Over");
        gameManager.instance.MaxLevelReachedPopUp.SetActive(false);
        Debug.Log("Max Level False");

    }
    IEnumerator SalvDetectorBought()
    {
        gameManager.instance.SalvBoughtPopUp.SetActive(true);
        Debug.Log("Salv Bought Pop Up True");
        Debug.Log("WFS Started");
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("WFS Over");
        gameManager.instance.SalvBoughtPopUp.SetActive(false);
        Debug.Log("Salv Bought Pop Up False");

    }
}


