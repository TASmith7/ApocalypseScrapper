using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

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
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }
    public void RestartLevel()
    {
        Inventory.Instance.InvLevelRestart();
        gameManager.instance.playerScript.RestartLevel();
        gameManager.instance.UnpauseState();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;

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

    #region Store Menu Buttons
    public void SmallHeal()
    {
        if (gameManager.instance.spendable >= 75 && gameManager.instance.playerScript.HP != gameManager.instance.playerScript.HPMax)
        {

            if (gameManager.instance.playerScript.HPMax - gameManager.instance.playerScript.HP <= 25)
            {
                gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HPMax;
            }
            else gameManager.instance.playerScript.HP += 25;

            gameManager.instance.spendable -= 75;
            gameManager.instance.spent += 75;
        }
        
    }

    public void LargeHeal()
    {
        if (gameManager.instance.spendable >= 200 && gameManager.instance.playerScript.HP != gameManager.instance.playerScript.HPMax)
        {

            if (gameManager.instance.playerScript.HPMax - gameManager.instance.playerScript.HP <= 100)
            {
                gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HPMax;
            }
            else gameManager.instance.playerScript.HP += 100;

            gameManager.instance.spendable -= 200;
            gameManager.instance.spent += 200;
        }
    }
    public void MaxHealth()
    {
        if (gameManager.instance.spendable >= 150)
        {

            gameManager.instance.playerScript.HPMax += 25;
            gameManager.instance.playerScript.HP += 25;

            gameManager.instance.spendable -= 150;
            gameManager.instance.spent += 150;
        }
    }

    public void GetShield()
    {
        if (gameManager.instance.spendable >= 200)
        {

            gameManager.instance.playerScript.shielded = true;
            gameManager.instance.playerScript.shieldMax += 25;
            gameManager.instance.playerScript.shieldValue += 25;

            gameManager.instance.spendable -= 200;
            gameManager.instance.spent += 200;
        }
    }

    public void ModShield()
    {
        if (gameManager.instance.spendable >= 150 && gameManager.instance.playerScript.shielded == true)
        {
            gameManager.instance.playerScript.shieldRate += 1;
            gameManager.instance.playerScript.shieldCD -= 1;

            gameManager.instance.spendable -= 150;
            gameManager.instance.spent += 150;
        }
    }

    public void PlusWeapDmg()
    {
        if (gameManager.instance.spendable >= 100)
        {

            gameManager.instance.playerScript.shootDamage += 1;

            gameManager.instance.spendable -= 100;
            gameManager.instance.spent += 100;
        }
    }

    public void OverchargeWeapDmg()
    {
        if (gameManager.instance.spendable >= 300)
        {
            gameManager.instance.playerScript.shootDamage += 5;
            gameManager.instance.playerScript.shootRate += 0.33f;

            gameManager.instance.spendable -= 300;
            gameManager.instance.spent += 300;
        }
    }

    public void JPFuel()
    {
        if (gameManager.instance.spendable >= 200)
        {
            gameManager.instance.playerScript.fuelConsumptionRate -= 0.1f;


            gameManager.instance.spendable -= 200;
            gameManager.instance.spent += 200;
        }
    }

    public void JPRecharge()
    {
        if (gameManager.instance.spendable >= 150)
        {
            gameManager.instance.playerScript.fuelRefillRate += 0.1f;

            gameManager.instance.spendable -= 150;
            gameManager.instance.spent += 150;

        }
    }

    public void GetDetector()
    {
        if (gameManager.instance.spendable >= 650)
        {
            gameManager.instance.playerScript.salvDetector = true;

            gameManager.instance.spendable -= 650;
            gameManager.instance.spent += 650;
        }
    }
    public void UpgradeSalvageRange()
    {
        if(gameManager.instance.spendable>=150)
        {
            gameManager.instance.playerScript.salvageRange += 1;
            gameManager.instance.spendable -= 150;
            gameManager.instance.spent += 150;
        }
    }
    public void UpgradeSalvageSpread()
    {
        if (gameManager.instance.spendable >= 400)
        {
            gameManager.instance.playerScript.salvageSpread += 0.1f;
            gameManager.instance.spendable -= 400;
            gameManager.instance.spent += 400;
        }
    }
    public void UpgradeSalvageEfficiency()
    {
        if (gameManager.instance.spendable >=200)
        {
            gameManager.instance.playerScript.salvageRate += 0.1f;
            gameManager.instance.spendable -= 200;
            gameManager.instance.spent += 200;
        }
    }
    public void FinishUpgrade()
    {
        gameManager.instance.playerScript.playerTotalScore += gameManager.instance.playerScript.playerFloorScore;
        gameManager.instance.playerScript.playerBonus += gameManager.instance.spendable;
        gameManager.instance.UnpauseState();
        gameManager.instance.NextLevel();
    }
    public void UpgradeStaminaEfficiency()
    {
        if(gameManager.instance.spendable>=400)
        {
            gameManager.instance.playerScript.staminaDrain -= 0.2f;
            gameManager.instance.spendable -= 400;
            gameManager.instance.spent += 400;
        }    
    }
    public void UpgradeStaminaRecharge()
    {
        if (gameManager.instance.spendable >= 400)
        {
            gameManager.instance.playerScript.staminaRefillRate += 0.2f;
            gameManager.instance.spendable -= 400;
            gameManager.instance.spent += 400;
        }
    }

    #endregion


}


