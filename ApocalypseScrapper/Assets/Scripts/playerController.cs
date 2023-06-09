using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour, IDamage, ISalvageable
{
    #region Player variables

    public bool GODMODE;

    [Header("----- Components -----")]
    [SerializeField] public CharacterController controller;
    // [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] public Camera playerCam;
    public LayerMask salvageLayer;

    [Header("----- Player Stats -----")]
    [Range(1, 999)][SerializeField] public static int HP;
    [SerializeField] public static int HPMax;
    [SerializeField] public float playerSpeed;
    [Range(10, 50)][SerializeField] float gravityValue;
    [Range(0.3f, 1.0f)][SerializeField] float walkingFootstepRate;
    [Range(0.2f, 1.0f)][SerializeField] float runningFootstepRate;
    [Range(0.5f, 1.0f)][SerializeField] float crouchingFootstepRate;
    private Vector3 playerVelocity;
    private Vector3 horizontalVelocity;
    private float horizontalSpeed;
    private bool groundedPlayer;
    static public bool isDead;
    Vector3 move;
    Vector3 lastPosition;
    private float timeBetweenFootsteps;
    private float minimumMovement;

    [Header("----- Salvage Stats -----")]

    [SerializeField] public static int salvageRange;
    [Range(0.1f, 1)][SerializeField] public static float salvageRate;
    [Range(0.01f, 1)][SerializeField] public float salvageSpread;
    bool isSalvaging;
    

    [Header("----- Animation Stats -----")]
    [SerializeField] float animTransSpeed;
    [SerializeField] GameObject[] bloodEffect;
    [SerializeField] GameObject beamEffect;
    [SerializeField] GameObject muzzleFlash;

    [Header("----- Jetpack Stats -----")]
    [Range(1, 8)][SerializeField] public static float thrustPower;
    [Range(0, 1)][SerializeField] public static float fuelConsumptionRate;
    [Range(0, 0.5f)][SerializeField] public static float fuelRefillRate;
    [Range(1, 100)][SerializeField] int timeToTurnOffFuelBar;
    bool isThrusting;
    float timeOfLastThrust;

    [Header("----- Stamina Stats -----")]
    [Range(10, 20)][SerializeField] public float sprintSpeed;
    [Range(0, 1)][SerializeField] public static float staminaDrain;
    [Range(0, 0.5f)][SerializeField] public static float staminaRefillRate;
    [Range(1, 100)][SerializeField] int timeToTurnOffStaminaBar;
    [Range(5, 10)][SerializeField] public float walkSpeed;
    public bool isSprinting;

    [Header("----- Slide Stats -----")]
    [Range(0.5f, 2)] public float slideTimeLength;
    public bool isSliding;

    [Header("----- Crouch Stats -----")]
    [SerializeField] private float crouchHeight;
    [Range(0, 10)][SerializeField] public float crouchSpeed;
    [SerializeField] private float standingHeight;
    [SerializeField] private float timeToCrouch = 0.25f;
    private Vector3 crouchingCenter = new Vector3(0, 0.7f, 0);
    private Vector3 standingCenter = new Vector3(0, 0.5f, 0);
    public bool isCrouching;
    bool duringCrouchAnimation;


    float timeOfLastSprint;
    bool jetpackPowerDownAudioPlayed;
    bool outOfBreathAudioPlayed;
    bool deathCued;

    [Header("----- Gun Stats -----")]
    public List<GunStats> gunList = new List<GunStats>();
    [Range(1, 100)][SerializeField] public static int shootDamage;
    public static float shootSpread;
    [Range(0.1f, 5)][SerializeField] public static float shootRate;
    [Range(1, 100)][SerializeField] public int shootDistance;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    public MeshRenderer gunMaterial;
    public MeshFilter gunModel;
    public GameObject playerGun;
    public CapsuleCollider playerGunMeleeCollider;
    public int selectedGun;
    bool isShooting;
    public bool isMeleeing = false;
    public bool canMelee = true;
    public float meleeResetTime;
    public float meleeDamage;


    weaponMovement weaponMovementScript;
    [SerializeField] Animator gunAnimator;


    [Header("-----Upgrades-----")]
    [SerializeField] public static bool salvDetector;
    [SerializeField] public static bool shielded;
    [SerializeField] public static int shieldValue;
    [SerializeField] public static int shieldMax;
    [SerializeField] public static int shieldCD;
    [SerializeField] public static int shieldRate;
    
    bool isShieldRegen;
    float tookDamage;

    [Header("----- Headbob Settings -----")]
    [SerializeField] float walkBobSpeed;
    [SerializeField] float walkBobAmount;
    [SerializeField] float sprintBobSpeed;
    [SerializeField] float sprintBobAmount;
    [SerializeField] float crouchBobSpeed;
    [SerializeField] float crouchBobAmount;

    float defaultYPosForCam;
    float headBobTimer;


    [Header("----- Score Stats -----")]
    [SerializeField] public static float totalLevelSalvage;
    [SerializeField] public static int playerFloorSalvage;
    [SerializeField] public static int playerTotalSalvage;
    //[SerializeField] public static int playerBonus;
    [SerializeField] public static int spent;
    [SerializeField] public static int hazardPay;
    [SerializeField] public static int questPay;
    public static string playerFloorGrade;

    #endregion

    private void Start()
    {
        isDead = false;
        SetPlayerStats();
        playerSpeed = walkSpeed;

        PlayerUIUpdate();
        playerFloorSalvage = 0;
        StartCoroutine(FindTotalLevelSalvage());
        gameManager.instance.UpdateSalvageScore();

        SpawnPlayer();

        jetpackPowerDownAudioPlayed = false;
        outOfBreathAudioPlayed = false;
        timeBetweenFootsteps = walkingFootstepRate;

        // setting default y position for main camera
        defaultYPosForCam = playerCam.transform.localPosition.y;

        standingHeight = controller.height;
        crouchHeight = 0.3f;

        playerGunMeleeCollider = playerGun.GetComponent<CapsuleCollider>();
        playerGunMeleeCollider.enabled = false;

        weaponMovementScript = playerGun.GetComponent<weaponMovement>();

        gunAnimator = weaponMovementScript.GetComponent<Animator>();

    }


    void Update()
    {

        if (GODMODE) { HP = 999; HPMax = 999; shootDamage = 999; salvDetector = true; shootRate = 0.01f; }


        horizontalVelocity = controller.velocity;
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        horizontalSpeed = horizontalVelocity.magnitude;

        if (gameManager.instance.activeMenu == null)
        {
            // anim.SetFloat("Speed", Input.GetAxis("Vertical"));
            //float vel = rb.velocity.normalized.magnitude;

            //if (vel >= 0 && vel <= 1)
            //{
            //    speed = Mathf.Lerp(speed, vel, Time.deltaTime * animTransSpeed);
            //    
            //}
            //SelectGun();
            if(!isDead)
            {
                Movement();
            }
            Shielding();
            CueHeadBobMovement();

            if (!isSliding)
            {
                CueFootstepAudio();
            }

            if (Input.GetButton("Shoot") && !isShooting && !isDead)
                StartCoroutine(Shoot());

            if (Input.GetKeyDown(KeyCode.E) && !isShooting && !isDead)
            {
                Melee();
            }

            if (!isSalvaging && Input.GetButton("Salvage") && !isDead)
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

            // if we die
            if(isDead && !deathCued)
            {
                deathCued = true;

                // disable all forms of movement
                controller.enabled = false;
                gunAnimator.enabled = false;
                weaponMovementScript.enabled = false;
                playerCam.GetComponent<cameraControls>().enabled = false;

                // add a rigid body and a box collider to the player and apply a force to the rigid body to assimilate being pushed/shot down
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(0.5f, 1.5f, 0.6f);
                boxCollider.center = new Vector3(0, 1.1f, 0);

                rb.AddForce(gameObject.transform.right * 7, ForceMode.Impulse);

                playerAudioManager.instance.outOfBreathAudioSource.PlayOneShot(playerAudioManager.instance.playerDeathAudio, 1);
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
    public bool IsMoving
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
        if (IsMoving && controller.isGrounded)
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
            if (isCrouching)
            {
                playerSpeed = crouchSpeed;
            }
            else
            {
                playerSpeed = walkSpeed;
            }

            if (Input.GetButton("Sprint") && !isCrouching)
            {
                // turn on our stamina bar
                gameManager.instance.TurnOnStaminaUI();

                // if we are not out of stamina
                if (gameManager.instance.staminaFillBar.fillAmount > 0)
                {
                    isSprinting = true;
                    // while player holds down shift, give velocity in the z direction a value
                    playerSpeed = sprintSpeed;

                    timeOfLastSprint = Time.fixedTime;

                    outOfBreathAudioPlayed = false;
                }
                // else if we are out of stamina
                else if (gameManager.instance.staminaFillBar.fillAmount <= 0)
                {
                    timeOfLastSprint = Time.fixedTime;

                    // if not already playing our out of breath audio, and we haven't already played it once
                    if (!playerAudioManager.instance.outOfBreathAudioSource.isPlaying && outOfBreathAudioPlayed == false)
                    {
                        playerAudioManager.instance.outOfBreathAudioSource.Play();
                        outOfBreathAudioPlayed = true;
                    }
                }

                // reducing the stamina bar while the player is pressing shift AND moving
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    StartCoroutine(ReduceStaminaUI());
                }

            }
            else
            {
                isSprinting = false;
            }

            // refilling the stamina bar when the player is not pressing shift until it's full
            if (gameManager.instance.staminaFillBar.fillAmount < 1 && !isSprinting)
            {
                isSprinting = false;
                playerSpeed = walkSpeed;
                StartCoroutine(RefillStaminaUI());
            }

            // if the elapsed time at our last sprint minus our current time elapsed is greater than or equal to 2 seconds
            if (Time.fixedTime - timeOfLastSprint >= timeToTurnOffStaminaBar)
            {
                // turn off stamina fuel UI
                gameManager.instance.TurnOffStaminaUI();
            }

            if (Input.GetButtonDown("Crouch") && !duringCrouchAnimation)
            {
                StartCoroutine(CrouchStand());
            }

        }


        // movement on the x and z axes
        move = (transform.right * Input.GetAxis("Horizontal")) +
               (transform.forward * Input.GetAxis("Vertical"));

        // calling the builtin move method on the player controller with frame rate independence
        controller.Move(playerSpeed * Time.deltaTime * move);

        // can't fly when you're crouched
        if (Input.GetButton("Jump") && !isCrouching)
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

    private void Melee()
    {
        // if not currently meleeing and can melee
        if (!isMeleeing && canMelee)
        {
            // enable the guns melee collider and set flags to true
            playerGunMeleeCollider.enabled = true;
            isMeleeing = true;
            canMelee = false;

            // play melee animation
            gunAnimator.SetTrigger("Melee");

            // play melee audio
            playerAudioManager.instance.meleeSwingAudioSource.PlayOneShot(playerAudioManager.instance.meleeSwingAudio[Random.Range(0, playerAudioManager.instance.meleeSwingAudio.Length)], 0.5f);
            playerAudioManager.instance.meleeGruntAudioSource.PlayOneShot(playerAudioManager.instance.meleeGruntAudio[Random.Range(0, playerAudioManager.instance.meleeGruntAudio.Length)]);
        }

        // reset my melee time using coroutine
        StartCoroutine(ResetMeleeTime());
    }

    private IEnumerator ResetMeleeTime()
    {
        // this line delays melees by a specified amount of seconds
        yield return new WaitForSeconds(meleeResetTime);
        gunAnimator.SetTrigger("Stop Meleeing");

        // reset flags and turn off the guns melee collider
        isMeleeing = false;
        playerGunMeleeCollider.enabled = false;
        canMelee = true;
    }
    private IEnumerator CrouchStand()
    {
        // if crouching and underneath something
        if (isCrouching && Physics.Raycast(playerCam.transform.position, Vector3.up, 1f))
        {
            yield break;
        }

        duringCrouchAnimation = true;

        // caching values to know which height and center we should be using and moving to
        float timeElapsed = 0f;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;


        while (timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        if (isSprinting && !isCrouching && IsMoving)
        {
            isSliding = true;

            // play sliding audio
            if (!playerAudioManager.instance.playerSlideAudioSource.isPlaying)
            {
                playerAudioManager.instance.playerSlideAudioSource.PlayOneShot(playerAudioManager.instance.playerSlideAudio, 0.65f);
            }

            while (timeElapsed < slideTimeLength)
            {
                isSprinting = false;
                transform.position += Vector3.Lerp(transform.localPosition, transform.forward / 20, 7);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

        }

        isSliding = false;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
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
        if (isDead) return;
        if (IsMoving && !isSliding)
        {
            headBobTimer += Time.deltaTime * (gameManager.instance.staminaFillBar.fillAmount > 0 && isSprinting ? sprintBobSpeed : isCrouching ? crouchBobSpeed : walkBobSpeed);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x,
                defaultYPosForCam + Mathf.Sin(headBobTimer) * (gameManager.instance.staminaFillBar.fillAmount > 0 && isSprinting ? sprintBobAmount : isCrouching ? crouchBobAmount : walkBobAmount),
                playerCam.transform.localPosition.z);
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        
        // play shooting audio
        playerAudioManager.instance.gunAudioSource.Play();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit tmpHit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out tmpHit))
        {
            targetPoint = tmpHit.point;
        }
        else { targetPoint = ray.GetPoint(75); }
        Vector3 directionNoSpread = targetPoint - shootPos.position;
        float x = Random.Range(-shootSpread, shootSpread);
        float y = Random.Range(-shootSpread, shootSpread);
        Vector3 directionWithSpread = directionNoSpread + new Vector3(x, y, 0);
        GameObject bulletClone = Instantiate(bullet, shootPos.position, Quaternion.identity);
        GameObject flashClone = Instantiate(muzzleFlash, shootPos.position, Quaternion.identity);

        // Set the rotation of the bullet to match the direction the player is looking
        bulletClone.transform.forward = directionNoSpread.normalized;
        // Set the bullet's velocity to this
        bulletClone.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletSpeed, ForceMode.Impulse);

        gunAnimator.Play("WeaponRecoil");

        

        //we use this raycast to return the position of where our raycast hits
        RaycastHit hit2;

        ////If the ray going from the middle of our screen hits something, "out" the position of where it hits in our 'hit' variable,
        ////and it will shoot the specified distance via our variable
        if (Physics.Raycast(shootPos.position, shootPos.forward, out hit2, shootDistance))
        {
            if (hit2.transform.CompareTag("Enemy"))
            {
                for (int i = 0; i < bloodEffect.Length; i++)
                {
                    int randoEffect = Random.Range(0, bloodEffect.Length);
                    GameObject bloodClone = Instantiate(bloodEffect[randoEffect], hit2.point, Quaternion.identity);
                    //Vector3 effectDir = hit2.point - transform.position;
                    //Quaternion rotation = Quaternion.LookRotation(effectDir);
                    //bloodClone.transform.rotation = rotation;
                    Destroy(bloodClone, .3f);
                }
            }
        }
        #region Old Raycast Code
        //    //if the object we hit contains the IDamage interface
        //    IDamage damageable = hit.collider.GetComponent<IDamage>();

        //    //if the above^ has the component IDamage(i.e.it's not null), and it is not the player
        //    if (damageable != null && hit.collider.tag != "Player")
        //    {
        //        //take damage from the damageable object
        //        damageable.TakeDamage(shootDamage);
        //    }
        #endregion
        Destroy(flashClone, .05f);
        //The yield return will wait for the specified amount of seconds
        //before moving on to the next line.It does NOT exit the method.
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator Salvage()
    {
        //RaycastHit[] hits;
        //hits = Physics.SphereCastAll(shootPos.position, salvageSpread, shootPos.transform.forward, salvageRange, salvageLayer) ;
        //GameObject beam = null;
        //for (int i = 0; i < hits.Length; i++)
        //{
        //    //Do something with the hit information

        //    // if the object we clicked on contains the ISalvageable interface
        //    ISalvageable salvageable = hits[i].collider.GetComponent<ISalvageable>();

        //    // if the object is salvageable
        //    if (salvageable != null && !hits[i].collider.CompareTag("Player"))
        //    {
        //        isSalvaging = true;
        //        beam = Instantiate(beamEffect, hits[i].point, Quaternion.identity);
        //        Vector3 effectDir = hits[i].point - shootPos.transform.position;
        //        Quaternion rotation = Quaternion.LookRotation(effectDir);
        //        beam.transform.rotation = rotation;
        //        gameManager.instance.salvagingObjectReticle.fillAmount += 1.0f / (salvageRate * hits[i].collider.GetComponent<salvageableObject>().salvageTime) * Time.deltaTime;

        //        // if our salvaging audio isn't already playing
        //        if (!playerAudioManager.instance.salvagingAudioSource.isPlaying)
        //        {
        //            playerAudioManager.instance.salvagingAudioSource.Play();
        //        }

        //        if (gameManager.instance.salvagingObjectReticle.fillAmount == 1)
        //        {
        //            SalvageObject(hits[i].collider.gameObject);

        //            gameManager.instance.salvagingObjectReticle.fillAmount = 0;
        //            yield return new WaitForSeconds(0.01f);
        //        }

        //    }
        //    // else what we are looking at is not salvageable, so stop our salvaging audio and set isSalvaging bool to false
        //    else
        //    {
        //        playerAudioManager.instance.salvagingAudioSource.Stop();
        //        isSalvaging = false;
        //    }


        //}
        //yield return new WaitForSeconds(1f);
        //if (beam != null)
        //{
        //    Destroy(beam, .01f);
        //}
        RaycastHit hit;
        GameObject beam = null;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, salvageRange))
        {

            // if the object we clicked on contains the ISalvageable interface
            ISalvageable salvageable = hit.collider.GetComponent<ISalvageable>();

            // if the object is salvageable
            if (salvageable != null && !hit.collider.CompareTag("Player"))
            {
                isSalvaging = true;
                beam = Instantiate(beamEffect, hit.point, Quaternion.identity);
                Vector3 effectDir = hit.point - shootPos.transform.position;
                Quaternion rotation = Quaternion.LookRotation(effectDir);
                beam.transform.rotation = rotation;
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
        if (beam != null)
        {
            Destroy(beam, .1f);
        }
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

        if(!playerAudioManager.instance.takeDamageAudioSource.isPlaying && HP > 0)
        {
            playerAudioManager.instance.takeDamageAudioSource.PlayOneShot(playerAudioManager.instance.takeDamageAudio[Random.Range
                (0, playerAudioManager.instance.takeDamageAudio.Length)], playerAudioManager.instance.takeDamageAudioVolume);
        }

        if (HP <= 0)
        {
            isDead = true;
            playerAudioManager.instance.footstepAudioSource.Stop();
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

    public void PlayerUIUpdate()
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
        playerFloorSalvage += (int)objectToSalvage.GetComponent<salvageableObject>().salvageValue;
        playerTotalSalvage += (int)objectToSalvage.GetComponent<salvageableObject>().salvageValue;

        //Assigning drops based off SalvageableObject Script
        objectToSalvage.GetComponent<salvageableObject>().AssignDrops();

        // destroying object
        Destroy(objectToSalvage);

        playerAudioManager.instance.objectSalvagedAudioSource.Play();

        // updating salvage score UI
        gameManager.instance.UpdateSalvageScore();

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

    //public void DefaultPlayerStats()
    //{
    //    HP = globalSceneControl.Instance._MSHP;
    //    HPMax = globalSceneControl.Instance._MSHPMax;

    //    salvageRate = globalSceneControl.Instance._MSsalvageRate;
    //    salvageRange = globalSceneControl.Instance._MSsalvageRange;
    //    salvageSpread= globalSceneControl.Instance._MSsalvageSpread;
    //    thrustPower = globalSceneControl.Instance._MSthrustPower;
    //    fuelConsumptionRate = globalSceneControl.Instance._MSfuelConsumptionRate;
    //    fuelRefillRate = globalSceneControl.Instance._MSfuelRefillRate;
    //    shootDamage = globalSceneControl.Instance._MSshootDamage;
    //    shootRate = globalSceneControl.Instance._MSshootRate;
    //    shootDistance = globalSceneControl.Instance._MSshootDistance;
    //    salvDetector = globalSceneControl.Instance._MSsalvDetector;
    //    shielded = globalSceneControl.Instance._MSshielded;
    //    shieldValue = globalSceneControl.Instance._MSshieldValue;
    //    shieldMax = globalSceneControl.Instance._MSshieldMax;
    //    shieldCD = globalSceneControl.Instance._MSshieldCD;
    //    shieldRate = globalSceneControl.Instance._MSshieldRate;

    //    playerTotalSalvage = globalSceneControl.Instance._MSplayerScrapCollected;
    //    //playerBonus = globalSceneControl.Instance._MSplayerBonus;
    //}
    
    public void SpawnPlayer()
    {
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
    }

    
    IEnumerator ReduceStaminaUI()
    {
            // stopping the refill coroutine while sprinting
            StopCoroutine(RefillStaminaUI());

            // reducing the stamina bar
            gameManager.instance.staminaFillBar.fillAmount -= staminaDrain * Time.deltaTime;

            yield return new WaitForSeconds(0.25f);
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
        Inventory.Instance.InvSnapshot();
        int levelToSaveTo = gameManager.instance.level + 1;
        
        PlayerPrefs.SetInt(levelToSaveTo+"HP" , HP);
        PlayerPrefs.SetInt(levelToSaveTo + "HPMax" , HPMax);
        PlayerPrefs.SetFloat(levelToSaveTo + "salvageRate", salvageRate);
        PlayerPrefs.SetInt(levelToSaveTo + "salvageRange", salvageRange);
        
        PlayerPrefs.SetFloat(levelToSaveTo + "thrustPower", thrustPower);
        PlayerPrefs.SetFloat(levelToSaveTo + "fuelConsumptionRate", fuelConsumptionRate);
        PlayerPrefs.SetFloat(levelToSaveTo + "fuelRefillRate", fuelRefillRate);

        PlayerPrefs.SetFloat(levelToSaveTo + "staminaDrain", staminaDrain);
        PlayerPrefs.SetFloat(levelToSaveTo + "staminaRefillRate", staminaRefillRate);

        PlayerPrefs.SetInt(levelToSaveTo + "shootDamage", shootDamage);
        PlayerPrefs.SetFloat(levelToSaveTo + "shootRate", shootRate);
        PlayerPrefs.SetInt(levelToSaveTo + "shootDistance", shootDistance);
        PlayerPrefs.SetFloat(levelToSaveTo + "shootSpread", shootSpread);

        if (salvDetector) { PlayerPrefs.SetInt(levelToSaveTo + "salvDetector", 1); }
        else PlayerPrefs.SetInt(levelToSaveTo + "salvDetector", 0);

        if (shielded) { PlayerPrefs.SetInt(levelToSaveTo + "shielded", 1); }
        else PlayerPrefs.SetInt(levelToSaveTo + "shielded", 0);


        PlayerPrefs.SetInt(levelToSaveTo + "shieldValue", shieldValue);
        PlayerPrefs.SetInt(levelToSaveTo + "shieldMax", shieldMax);
        PlayerPrefs.SetInt(levelToSaveTo + "shieldCD" , shieldCD);
        PlayerPrefs.SetInt(levelToSaveTo +"shieldRate" , shieldRate);
        PlayerPrefs.SetInt(levelToSaveTo + "playerTotalSalvage", playerTotalSalvage);

        

        

        Debug.Log("Player Stats Saved");
        
    }


    public void SetPlayerStats()
    {
        if (PlayerPrefs.HasKey(gameManager.instance.level + "HP"))
        {
            HP = PlayerPrefs.GetInt(gameManager.instance.level + "HP");
            HPMax = PlayerPrefs.GetInt(gameManager.instance.level + "HPMax");

            salvageRate = PlayerPrefs.GetFloat(gameManager.instance.level + "salvageRate");
            salvageRange = PlayerPrefs.GetInt(gameManager.instance.level + "salvageRange");
            
            thrustPower = PlayerPrefs.GetFloat(gameManager.instance.level + "thrustPower");
            fuelConsumptionRate = PlayerPrefs.GetFloat(gameManager.instance.level + "fuelConsumptionRate");
            fuelRefillRate = PlayerPrefs.GetFloat(gameManager.instance.level + "fuelRefillRate");
            
            staminaDrain = PlayerPrefs.GetFloat(gameManager.instance.level + "staminaDrain");
            staminaRefillRate = PlayerPrefs.GetFloat(gameManager.instance.level + "staminaRefillRate");
            
            shootDamage = PlayerPrefs.GetInt(gameManager.instance.level + "shootDamage");
            shootRate = PlayerPrefs.GetFloat(gameManager.instance.level + "shootRate");
            shootDistance = PlayerPrefs.GetInt(gameManager.instance.level + "shootDistance");
            shootSpread = PlayerPrefs.GetFloat(gameManager.instance.level + "shootSpread");

            if(PlayerPrefs.GetInt(gameManager.instance.level + "salvDetector") == 1) salvDetector = true;

            if (PlayerPrefs.GetInt(gameManager.instance.level + "shielded") == 1) shielded = true;

            shieldValue = PlayerPrefs.GetInt(gameManager.instance.level + "shieldValue");
            shieldMax = PlayerPrefs.GetInt(gameManager.instance.level + "shieldMax");
            shieldCD = PlayerPrefs.GetInt(gameManager.instance.level + "shieldCD");
            shieldRate = PlayerPrefs.GetInt(gameManager.instance.level + "shieldRate");

            playerTotalSalvage = PlayerPrefs.GetInt(gameManager.instance.level + "playerTotalSalvage");

            Inventory.Instance.InvLoad(gameManager.instance.level);

        }
        else globalSceneControl.Instance.DefaultStats();

        //HP = globalSceneControl.Instance.HP;
        //HPMax = globalSceneControl.Instance.HPMax;
        //salvageRate = globalSceneControl.Instance.salvageRate;
        //salvageRange = globalSceneControl.Instance.salvageRange;
        //thrustPower = globalSceneControl.Instance.thrustPower;
        //fuelConsumptionRate = globalSceneControl.Instance.fuelConsumptionRate;
        //fuelRefillRate = globalSceneControl.Instance.fuelRefillRate;
        //shootDamage = globalSceneControl.Instance.shootDamage;
        //shootRate = globalSceneControl.Instance.shootRate;
        //shootDistance = globalSceneControl.Instance.shootDistance;
        //salvDetector = globalSceneControl.Instance.salvDetector;
        //shielded = globalSceneControl.Instance.shielded;
        //shieldValue = globalSceneControl.Instance.shieldValue;
        //shieldMax = globalSceneControl.Instance.shieldMax;
        //shieldCD = globalSceneControl.Instance.shieldCD;
        //shieldRate = globalSceneControl.Instance.shieldRate;
        //playerTotalSalvage = globalSceneControl.Instance.playerScrapCollected;
        //gameManager.instance.totalScoreData.text = playerTotalSalvage.ToString();
        ////playerBonus = globalSceneControl.Instance.playerBonus;
        ////gameManager.instance.playerBonusData.text = playerBonus.ToString();
        
        Debug.Log("Player stats loaded.");
    }

    IEnumerator FindTotalLevelSalvage()
    {
        yield return new WaitForSeconds(1);
        float tls = 0f;
        GameObject[] enemList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemList)
        {
            if (enemy.name.Contains("Drone"))
            {
                tls += 150;
            }

            if (enemy.name.Contains("turret"))
            {
                tls += 400;
            }
        }

        GameObject[] salvList = GameObject.FindGameObjectsWithTag("Salvage");
        Debug.Log("Salvagable objects: " + salvList.Length);
        foreach (GameObject obj in salvList)
        {
            tls += obj.GetComponent<salvageableObject>().salvageValue;
        }
        totalLevelSalvage = tls;
    }

    void CueFootstepAudio()
    {
        // if we are not on the ground or not moving, return
        if (!controller.isGrounded) return;
        if (!IsMoving) return;
        if (isDead) return;

        // reducing the time between footsteps each frame
        timeBetweenFootsteps -= Time.deltaTime;

        // once we reach 
        if (timeBetweenFootsteps <= 0)
        {
            if(gameManager.instance.staminaFillBar.fillAmount > 0  && isSprinting && !isSliding)
            {
                timeBetweenFootsteps = runningFootstepRate; 
                playerAudioManager.instance.footstepAudioSource.PlayOneShot(playerAudioManager.instance.footstepAudio[Random.Range(0, playerAudioManager.instance.footstepAudio.Length)]);
            }
            else if(isCrouching && !isSliding)
            {
                timeBetweenFootsteps = crouchingFootstepRate;
                playerAudioManager.instance.footstepAudioSource.PlayOneShot(playerAudioManager.instance.footstepAudio[Random.Range(0, playerAudioManager.instance.footstepAudio.Length)]);
            }
            else if(!isSliding)
            {
                timeBetweenFootsteps = walkingFootstepRate;
                playerAudioManager.instance.footstepAudioSource.PlayOneShot(playerAudioManager.instance.footstepAudio[Random.Range(0, playerAudioManager.instance.footstepAudio.Length)]);
            }
        }
        
    }
}    