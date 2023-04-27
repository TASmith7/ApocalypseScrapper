using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour, IDamage, ISalvageable
{
    #region Player variables

    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;
    //[SerializeField] Rigidbody rb;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Camera playerCam;


    [Header("----- Player Stats -----")
    [Range(1, 100)][SerializeField] public int HP;
    [SerializeField] public int HPMax;
    [SerializeField] public float playerSpeed;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(0.3f, 1.0f)][SerializeField] float walkingFootstepRate;
    [Range(0.2f, 1.0f)][SerializeField] float runningFootstepRate;
    private Vector3 playerVelocity;
    private Vector3 horizontalVelocity;
    private float horizontalSpeed;
    private bool groundedPlayer;
    Vector3 move;
    Vector3 lastPosition;
    private float timeBetweenFootsteps;
    private float minimumMovement;

    [Header("----- Salvage Stats -----")]

    [SerializeField] public int salvageRange;
    [Range(0.1f, 1)][SerializeField] public float salvageRate;
    bool isSalvaging;
    


    [Header("----- Animation Stats -----")]
    [SerializeField] float animTransSpeed;
    
    [Header("----- Jetpack Stats -----")]
    [Range(1, 8)][SerializeField] public float thrustPower;
    [Range(0, 1)][SerializeField] public float fuelConsumptionRate;
    [Range(0, 0.5f)][SerializeField] public float fuelRefillRate;
    [Range(1, 100)][SerializeField] int timeToTurnOffFuelBar;
    bool isThrusting;
    float timeOfLastThrust;

    [Header("----- Stamina Stats -----")]
    [Range(10, 20)][SerializeField] public float sprintSpeed;
    [Range(0, 1)][SerializeField] public float staminaDrain;
    [Range(0, 0.5f)][SerializeField] public float stmainaRefillRate;
    [Range(1, 100)][SerializeField] int timeToTurnOffStaminaBar;
    [Range(5, 10)][SerializeField] public float walkSpeed;


    public bool isSprinting;
    float timeOfLastSprint;
    bool jetpackPowerDownAudioPlayed;
    bool outOfBreathAudioPlayed;

    [Header("----- Gun Stats -----")]
    public List<GunStats> gunList = new List<GunStats>();
    [Range(1, 100)][SerializeField] public int shootDamage;
    [Range(0.1f, 5)][SerializeField] public float shootRate;
    [Range(1, 100)][SerializeField] public int shootDistance;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    public MeshRenderer gunMaterial;
    public MeshFilter gunModel;
    public int selectedGun;
    bool isShooting;


    [Header("-----Upgrades-----")]
    [SerializeField] public bool salvDetector;
    [SerializeField] public bool shielded;
    [SerializeField] public int shieldValue;
    [SerializeField] public int shieldMax;
    [SerializeField] public int shieldCD; 
    [SerializeField] public int shieldRate;
    bool shieldOnCD;
    bool isShieldRegen;
    float tookDamage;

    [Header("----- Headbob Settings -----")]
    [SerializeField] float walkBobSpeed;
    [SerializeField] float walkBobAmount;
    [SerializeField] float sprintBobSpeed;
    [SerializeField] float sprintBobAmount;
    float defaultYPosForCam;
    float headBobTimer;


    [Header("----- Score Stats -----")]
    [SerializeField] public float totalLevelSalvage;
    [SerializeField] public int playerFloorScore;
    [SerializeField] public int playerTotalScore;
    [SerializeField] public int playerBonus;

    #endregion

    private void Start()
    {

        SetPlayerStats();
        playerSpeed = walkSpeed;
        
        PlayerUIUpdate();
        playerFloorScore = 0;
        
        SpawnPlayer();
        StartCoroutine(FindTotalLevelSalvage());
        
        jetpackPowerDownAudioPlayed = false;
        outOfBreathAudioPlayed = false;
        timeBetweenFootsteps = walkingFootstepRate;

        // setting default y position for main camera
        defaultYPosForCam = playerCam.transform.localPosition.y;
    }

    void Update()
    {

        horizontalVelocity = controller.velocity;
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        horizontalSpeed = horizontalVelocity.magnitude;

        if (gameManager.instance.activeMenu == null)
        {
            anim.SetFloat("Speed", Input.GetAxis("Vertical"));
            //float vel = rb.velocity.normalized.magnitude;

            //if (vel >= 0 && vel <= 1)
            //{
            //    speed = Mathf.Lerp(speed, vel, Time.deltaTime * animTransSpeed);
            //    
            //}
            //SelectGun();
            Movement();
            Shielding();
            CueHeadBobMovement();
            CueFootstepAudio();


            if (Input.GetButton("Shoot") && !isShooting)
                StartCoroutine(Shoot());

            if (!isSalvaging && Input.GetButton("Salvage"))
            {
                StartCoroutine(Salvage());
            }
            // else if I am not salvaging and not pressing right click and my salvaging audio is playing, turn off the audio
            else if (!isSalvaging && playerAudioManager.instance.salvagingAudioSource.isPlaying)
            {
                playerAudioManager.instance.salvagingAudioSource.Stop();

                // if we let go of salvage button, reset reticle fill
                gameManager.instance.salvagingObjectReticle.fillAmount = 0;
            }
            else
            {
                isSalvaging = false;
            }

        }

    }

    void FixedUpdate()
    {
        RaycastHit hit;

        lastPosition = transform.position;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, salvageRange))
        {
            // if the object we are looking at is salvageable
            ISalvageable salvageable = hit.collider.GetComponent<ISalvageable>();

            // if the above^ has the component ISalvageable (i.e. it's not null)
            if (salvageable != null && !isSalvaging)
            {
                // update our salvage value on salvageable reticle to the value of the object being looked at
                if (hit.collider.GetComponent<salvageableObject>() != null)
                {
                    gameManager.instance.salvageValueText.text = hit.collider.GetComponent<salvageableObject>().salvageValue.ToString();
                }
                // change the reticle to salvageable reticle
                gameManager.instance.CueSalvageableReticle();
            }
            else if (salvageable == null)
            {
                // else if what we are looking at isn't salvageable, change/keep the reticle to main reticle
                gameManager.instance.CueMainReticle();

                // resetting my salvaging object reticle fill amount
                gameManager.instance.salvagingObjectReticle.fillAmount = 0;
            }
        }
        else
        {
            // else if our raycast isn't hitting anything, change/keep to main reticle
            gameManager.instance.CueMainReticle();

            // resetting my salvaging object reticle fill amount
            gameManager.instance.salvagingObjectReticle.fillAmount = 0;
        }

    }

    // might change this in the future to determine animation speed, but right now this works to tell if we are moving or not
    bool IsMoving
    {
        get
        {
            // check how far our player has moved since the last update
            float distance = Vector3.Distance(transform.position, lastPosition);

            // if the distance we have moved is greater than our min movement, it's true, if not, it's false
            return (distance > minimumMovement);
        }
    }

    bool AllowWeaponSway()
    {
        if(IsMoving && controller.isGrounded)
        {
            return true;
        }

        return false;
    }
    void Movement()
    {
        groundedPlayer = controller.isGrounded;

        // if the player is on the ground and their velocity in y is less than 0
        if (groundedPlayer && playerVelocity.y < 0)
        {
            // set the vertical velocity to 0
            playerVelocity.y = 0f;
        }
        if (groundedPlayer)
        {
            playerSpeed = walkSpeed;

            if (Input.GetButton("Sprint"))
            {

                // turn on our stamina bar
                gameManager.instance.TurnOnStaminaUI();

                // if we are not out of stamina
                if (gameManager.instance.staminaFillBar.fillAmount > 0)
                {
                    // while player holds down shift, give velocity in the z direction a value
                    playerSpeed = sprintSpeed;

                    timeOfLastSprint = Time.fixedTime;

                    outOfBreathAudioPlayed = false;
                }
                // else if we are out of stamina
                else if (gameManager.instance.staminaFillBar.fillAmount <= 0)
                {
                    // if not already playing our out of breath audio, and we haven't already played it once
                    if (!playerAudioManager.instance.outOfBreathAudioSource.isPlaying && outOfBreathAudioPlayed == false)
                    {
                        playerAudioManager.instance.outOfBreathAudioSource.Play();
                        outOfBreathAudioPlayed = true;
                    }
                }

                // reducing the stamina bar while the player is pressing shift
                StartCoroutine(ReduceStaminaUI());

            }


            // refilling the stamina bar when the player is not pressing shift until it's full
            if (gameManager.instance.staminaFillBar.fillAmount < 1 && !isSprinting)
            {
                playerSpeed = walkSpeed;
                StartCoroutine(RefillStaminaUI());
            }

            // if the elapsed time at our last sprint minus our current time elapsed is greater than or equal to 2 seconds
            if (Time.fixedTime - timeOfLastSprint >= timeToTurnOffStaminaBar)
            {
                // turn off stamina fuel UI
                gameManager.instance.TurnOffStaminaUI();
            }

        }


        // movement on the x and z axes
        move = (transform.right * Input.GetAxis("Horizontal")) +
               (transform.forward * Input.GetAxis("Vertical"));

        // calling the builtin move method on the player controller with frame rate independence
        controller.Move(playerSpeed * Time.deltaTime * move);

        if (Input.GetButton("Jump"))
        {
            playerSpeed = walkSpeed;

            // turn on our jetpack fuel bar
            gameManager.instance.TurnOnJetpackUI();

            // if we are not out of fuel
            if (gameManager.instance.jetpackFuelBar.fillAmount > 0)
            {
                // while player holds down space, give velocity in the y direction a value
                playerVelocity.y = thrustPower;

                // if our jetpack audio isn't already playing
                if (!playerAudioManager.instance.jetpackAudioSource.isPlaying)
                {
                    // play our jetpack audio
                    playerAudioManager.instance.jetpackAudioSource.Play();
                    jetpackPowerDownAudioPlayed = false;
                }


                timeOfLastThrust = Time.fixedTime;

            }
            // else if we are out of fuel
            else if (gameManager.instance.jetpackFuelBar.fillAmount <= 0)
            {
                // if we run out of fuel, stop our jetpack audio 
                playerAudioManager.instance.jetpackAudioSource.Stop();

                // if not already playing our power down audio, and we haven't already played it once
                if (!playerAudioManager.instance.jetpackPowerDownAudioSource.isPlaying && jetpackPowerDownAudioPlayed == false)
                {
                    playerAudioManager.instance.jetpackPowerDownAudioSource.Play();
                    jetpackPowerDownAudioPlayed = true;
                }
            }

            // reducing the fuel bar while the player is pressing space
            StartCoroutine(ReduceJetpackFuelUI());
        }
        // if we aren't pressing space and our jetpack audio is playing, turn it off
        else if (playerAudioManager.instance.jetpackAudioSource.isPlaying)
        {
            playerAudioManager.instance.jetpackAudioSource.Stop();
        }

        // refilling the fuel bar when the player is not pressing space until it's full
        if (gameManager.instance.jetpackFuelBar.fillAmount < 1 && !isThrusting)
        {
            StartCoroutine(RefillJetpackFuelUI());
        }

        // if the elapsed time at our last thrust minus our current time elapsed is greater than or equal to 2 seconds
        if (Time.fixedTime - timeOfLastThrust >= timeToTurnOffFuelBar)
        {
            // turn off jetpack fuel UI
            gameManager.instance.TurnOffJetpackUI();

        }


        // ensuring our players y velocity take gravity into effect
        playerVelocity.y -= gravityValue * Time.deltaTime;


        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Shielding()
    {
        
        if (shielded)
        {
            gameManager.instance.TurnOnShieldUI();

            if (shieldValue != shieldMax && !isShieldRegen)
            {
                StartCoroutine(ShieldFill());
            }
        }
        else gameManager.instance.TurnOffShieldUI();


    }

    void CueHeadBobMovement()
    {
        if (!controller.isGrounded) return;
        if(IsMoving)
        {
            headBobTimer += Time.deltaTime * (gameManager.instance.staminaFillBar.fillAmount > 0 && isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x,
                defaultYPosForCam + Mathf.Sin(headBobTimer) * (gameManager.instance.staminaFillBar.fillAmount > 0 && isSprinting ? sprintBobAmount : walkBobAmount), 
                playerCam.transform.localPosition.z);
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        // play shooting audio
        playerAudioManager.instance.gunAudioSource.Play();

        GameObject bulletClone = Instantiate(bullet, shootPos.position, Quaternion.identity);

        // Set the bullet's velocity to this 
        bulletClone.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * bulletSpeed;

        // Set the rotation of the bullet to match the direction the player is looking
        bulletClone.transform.rotation = Camera.main.transform.rotation;

        //we use this raycast to return the position of where our raycast hits
        //RaycastHit hit;

        ////If the ray going from the middle of our screen hits something, "out" the position of where it hits in our 'hit' variable,
        ////and it will shoot the specified distance via our variable
        //if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        //{
        //    //if the object we hit contains the IDamage interface
        //    IDamage damageable = hit.collider.GetComponent<IDamage>();

        //    //if the above^ has the component IDamage(i.e.it's not null), and it is not the player
        //    if (damageable != null && hit.collider.tag != "Player")
        //    {
        //        //take damage from the damageable object
        //        damageable.TakeDamage(shootDamage);
        //    }
        //}

        //The yield return will wait for the specified amount of seconds
        //before moving on to the next line.It does NOT exit the method.
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator Salvage()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, salvageRange))
        {
            // if the object we clicked on contains the ISalvageable interface
            ISalvageable salvageable = hit.collider.GetComponent<ISalvageable>();

            // if the object is salvageable
            if (salvageable != null && !hit.collider.CompareTag("Player"))
            {
                isSalvaging = true;

                gameManager.instance.salvagingObjectReticle.fillAmount += 1.0f / (salvageRate * hit.collider.GetComponent<salvageableObject>().salvageTime) * Time.deltaTime;

                // if our salvaging audio isn't already playing
                if (!playerAudioManager.instance.salvagingAudioSource.isPlaying)
                {
                    playerAudioManager.instance.salvagingAudioSource.Play();
                }

                if (gameManager.instance.salvagingObjectReticle.fillAmount == 1)
                {
                    SalvageObject(hit.collider.gameObject);
                    gameManager.instance.salvagingObjectReticle.fillAmount = 0;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            // else what we are looking at is not salvageable, so stop our salvaging audio and set isSalvaging bool to false
            else
            {
                playerAudioManager.instance.salvagingAudioSource.Stop();
                isSalvaging = false;
            }

        }
        else
        {
            isSalvaging = false;
        }
        yield return new WaitForSeconds(0.01f);


    }

    public void TakeDamage(float amount)
    {
        if (shieldValue >= (int)amount)
        {
            
            //StopCoroutine(ShieldCoolDown());
            shieldValue -= (int)amount;
            //if (shielded)
            //    StartCoroutine(ShieldCoolDown());
        }
        else if (shieldValue <= (int)amount && shieldValue > 0)
        {
            
            //StopCoroutine(ShieldCoolDown());
            int overflow = (int)amount - shieldValue;
            shieldValue = 0;
            HP -= overflow;

            // play shield breaking audio
            if (!playerAudioManager.instance.shieldBreakAudioSource.isPlaying)
            {
                playerAudioManager.instance.shieldBreakAudioSource.Play();
            }

            //if (shielded)
            //    StartCoroutine(ShieldCoolDown());
        }
        else
        {
            
            //StopCoroutine(ShieldCoolDown());
            HP -= (int)amount;
            //if(shielded)
                //StartCoroutine(ShieldCoolDown());
        }

        tookDamage = Time.time;
        PlayerUIUpdate();

        if(!playerAudioManager.instance.takeDamageAudioSource.isPlaying)
        {
            playerAudioManager.instance.takeDamageAudioSource.PlayOneShot(playerAudioManager.instance.takeDamageAudio[Random.Range
                (0, playerAudioManager.instance.takeDamageAudio.Length)], playerAudioManager.instance.takeDamageAudioVolume);
        }

        if (HP <= 0)
        {
            gameManager.instance.PlayerDead();
        }
    }

    //IEnumerator ShieldCoolDown()
    //{
    //    shieldOnCD = true;
    //    yield return new WaitForSeconds(shieldCD);
    //    shieldOnCD = false;
    //}

    IEnumerator ShieldFill()
    {
        isShieldRegen = true;
        while (shieldValue < shieldMax && Time.time > (tookDamage + shieldCD))
        {
            yield return new WaitForSeconds(1);
            shieldValue += shieldRate;
            PlayerUIUpdate ();
            
        }
        isShieldRegen = false;
    }

    void PlayerUIUpdate()
    {
        // updating the players health bar
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPMax;
        if (shielded)
        {
            gameManager.instance.shieldFillBar.fillAmount = (float)shieldValue / (float)shieldMax;
        }
    }

    IEnumerator ReduceJetpackFuelUI()
    {
        // this bool will be helpful for future development of thrusting capabilities. It currently has no effective use
        isThrusting = true;

        // stopping the refill coroutine while thrusting
        StopCoroutine(RefillJetpackFuelUI());

        // reducing the jetpack fuel bar
        gameManager.instance.jetpackFuelBar.fillAmount -= fuelConsumptionRate * Time.deltaTime;

        yield return new WaitForSeconds(0.25f);

        isThrusting = false;
    }

    IEnumerator RefillJetpackFuelUI()
    {
        yield return new WaitForSeconds(0.1f);

        if (!isThrusting)
        {
            // refilling the jetpack fuel bar
            gameManager.instance.jetpackFuelBar.fillAmount += fuelRefillRate * Time.deltaTime;
        }

    }

    public void SalvageObject(GameObject objectToSalvage)
    {
        // updating salvage score based on the objects salvage value assigned in inspector
        playerFloorScore += (int)objectToSalvage.GetComponent<salvageableObject>().salvageValue;

        // destroying object
        Destroy(objectToSalvage);

        playerAudioManager.instance.objectSalvagedAudioSource.Play();

        // updating salvage score UI
        gameManager.instance.UpdateSalvageScore(playerFloorScore);

        // resetting my salvaging object reticle fill amount
        gameManager.instance.salvagingObjectReticle.fillAmount = 0;

    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartMission()
    {
        SceneManager.LoadScene("Lvl 1");
    }
    
    public void SpawnPlayer()
    {
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
    }

    
    IEnumerator ReduceStaminaUI()
    {
        // this bool will be helpful for future development of thrusting capabilities. It currently has no effective use
        isSprinting = true;

        // stopping the refill coroutine while sprinting
        StopCoroutine(RefillStaminaUI());

        // reducing the stamina bar
        gameManager.instance.staminaFillBar.fillAmount -= staminaDrain * Time.deltaTime;

        yield return new WaitForSeconds(0.25f);

        isSprinting = false;
    }

    IEnumerator RefillStaminaUI()
    {
        yield return new WaitForSeconds(0.1f);

        if (!isSprinting)
        {
            // refilling the stamina bar
            gameManager.instance.staminaFillBar.fillAmount += staminaDrain * Time.deltaTime;
        }
    }

        
   public void SavePlayerStats()
    {
        if (globalSceneControl.Instance != null)
        {
            globalSceneControl.Instance.HP = HP;
            globalSceneControl.Instance.HPMax = HPMax;
            globalSceneControl.Instance.salvageRate = salvageRate;
            globalSceneControl.Instance.salvageRange = salvageRange;

            globalSceneControl.Instance.thrustPower = thrustPower;
            globalSceneControl.Instance.fuelConsumptionRate = fuelConsumptionRate;
            globalSceneControl.Instance.fuelRefillRate = fuelRefillRate;

            globalSceneControl.Instance.shootDamage = shootDamage;
            globalSceneControl.Instance.shootRate = shootRate;
            globalSceneControl.Instance.shootDistance = shootDistance;

            globalSceneControl.Instance.salvDetector = salvDetector;
            globalSceneControl.Instance.shielded = shielded;
            globalSceneControl.Instance.shieldValue = shieldValue;
            globalSceneControl.Instance.shieldCD = shieldCD;
            globalSceneControl.Instance.shieldRate = shieldRate;


            globalSceneControl.Instance.playerTotalScore = playerTotalScore;
            globalSceneControl.Instance.playerBonus = playerBonus;

            Debug.Log("Player Stats Saved");
        }
    }


    public void SetPlayerStats()
    {
        if (globalSceneControl.Instance != null)
        {
            HP = globalSceneControl.Instance.HP;
            HPMax = globalSceneControl.Instance.HPMax;
            salvageRate = globalSceneControl.Instance.salvageRate;
            salvageRange = globalSceneControl.Instance.salvageRange;
            thrustPower = globalSceneControl.Instance.thrustPower;
            fuelConsumptionRate = globalSceneControl.Instance.fuelConsumptionRate;
            fuelRefillRate = globalSceneControl.Instance.fuelRefillRate;
            shootDamage = globalSceneControl.Instance.shootDamage;
            shootRate = globalSceneControl.Instance.shootRate;
            shootDistance = globalSceneControl.Instance.shootDistance;
            salvDetector = globalSceneControl.Instance.salvDetector;
            shielded = globalSceneControl.Instance.shielded;
            shieldValue = globalSceneControl.Instance.shieldValue;
            shieldMax = globalSceneControl.Instance.shieldMax;
            shieldCD = globalSceneControl.Instance.shieldCD;
            shieldRate = globalSceneControl.Instance.shieldRate;
            playerTotalScore = globalSceneControl.Instance.playerTotalScore;
            gameManager.instance.totalScoreData.text = playerTotalScore.ToString();
            playerBonus = globalSceneControl.Instance.playerBonus;
            gameManager.instance.playerBonusData.text = playerBonus.ToString();
        }
        Debug.Log("Player stats loaded.");
    }

    IEnumerator FindTotalLevelSalvage()
    {
        yield return new WaitForSeconds(1);
        GameObject[] enemList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemList)
        {
            if (enemy.name.Contains("Drone"))
            {
                totalLevelSalvage += 150;
            }

            if (enemy.name.Contains("turret"))
            {
                totalLevelSalvage += 400;
            }
        }

        GameObject[] salvList = GameObject.FindGameObjectsWithTag("Salvage");
        Debug.Log("Salvagable objects: " + salvList.Length);
        foreach (GameObject obj in salvList)
        {
            totalLevelSalvage += obj.GetComponent<salvageableObject>().salvageValue;
        }
    }

    void CueFootstepAudio()
    {
        // if we are not on the ground or not moving, return
        if (!controller.isGrounded) return;
        if (!IsMoving) return;

        // reducing the time between footsteps each frame
        timeBetweenFootsteps -= Time.deltaTime;

        // once we reach 
        if (timeBetweenFootsteps <= 0)
        {
            
            if(gameManager.instance.staminaFillBar.fillAmount > 0  && isSprinting)
            {
                timeBetweenFootsteps = runningFootstepRate; 
                playerAudioManager.instance.footstepAudioSource.PlayOneShot(playerAudioManager.instance.footstepAudio[Random.Range(0, playerAudioManager.instance.footstepAudio.Length)]);
            }
            else
            {
                timeBetweenFootsteps = walkingFootstepRate;
                playerAudioManager.instance.footstepAudioSource.PlayOneShot(playerAudioManager.instance.footstepAudio[Random.Range(0, playerAudioManager.instance.footstepAudio.Length)]);
            }
        }
        
    }
}    