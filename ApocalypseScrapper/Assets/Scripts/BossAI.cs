using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;


public class BossAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Transform bitePos;
    [SerializeField] Transform spitPos;
    [SerializeField] SphereCollider crabWakeColl;

    [SerializeField] GameObject[] crabSpawners;
    [SerializeField] GameObject[] droneSpawners;

    [Header("-----Crab Stats-----")]
    [SerializeField] float HP;
    public float HPOrig;
    [SerializeField] int healAmt;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightLine;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;
    [SerializeField] float animTransSpeed;
    [Range(10, 200)][SerializeField] float radiusSleep;
    [Range(10, 1000)][SerializeField] float radiusActive;
    public float activeRadius;
    Vector3 playerDir;

    [Header("-----Bite Stats-----")]
    [Range(1, 10)][SerializeField] int biteDamage;
    [Range(.1f, 5)][SerializeField] float biteRate;
    public float biteDistance;
    
    [SerializeField] int biteSpeed;
    [SerializeField] GameObject bite;
    [Header("-----Spit Stats-----")]
    [SerializeField] GameObject spit;
    [Range(1, 10)][SerializeField] int spitDamage;
    [Range(.1f, 5)][SerializeField] float spitRate;
    [SerializeField] float spitDistance;
    
    [SerializeField] int spitSpeed;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource bossAudioSource;
    [SerializeField] AudioClip[] bossFootsteps;
    [SerializeField] AudioClip[] bossDamage;
    [SerializeField] AudioClip[] bossBite;
    [SerializeField] AudioClip bossSpit;

    [Range(0.05f, 1)][SerializeField] float timeBetweenFootsteps;
    float timeBetweenFootstepsOrig;


    bool playerInRange;
    float angleToPlayer;
    int wave;
    float speed;
    bool isBiting;
    bool isSpitting;
    float takeDamageTimer;

    bool hasPlayedEndGameAudio = false;

    //float stoppingDistanceOrig;
    bool destinationChosen;
    Vector3 startPos;
    [Header("----- Crab Spawn Stats-----")]
    [SerializeField] GameObject crab;
    [Header("----- Drone Spawn Stats-----")]
    [SerializeField] GameObject drone;


    //IEnumerator Roam()
    //{
    //    if (destinationChosen != true && agent.remainingDistance < 0.05)
    //    {
    //        destinationChosen = true;
    //        agent.stoppingDistance = 0;
    //        yield return new WaitForSeconds(roamPauseTime);

    //        Vector3 runTo = UnityEngine.Random.insideUnitSphere * roamDistance;
    //        runTo += startPos;

    //        NavMeshHit hit;
    //        NavMesh.SamplePosition(runTo, out hit, roamDistance, 1);

    //        agent.SetDestination(hit.position);
    //        destinationChosen = false;
    //    }
    //}    


    // Start is called before the first frame update
    void Start()
    {
        
        wave = 0;
        HPOrig = HP;
        activeRadius = radiusSleep;
        biteDistance = agent.stoppingDistance;
        gameManager.instance.TurnOnBossHPUI();
        BossHPUIUpdate();
        //stoppingDistanceOrig = agent.stoppingDistance;
        //startPos = transform.position;
        crabSpawners = GameObject.FindGameObjectsWithTag("Crab Spawn");

        droneSpawners = GameObject.FindGameObjectsWithTag("Drone Spawn");


        timeBetweenFootstepsOrig = timeBetweenFootsteps;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {

            CueFootstepAudio();

            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);

            if (playerInRange)
            {
                CanSeePlayer();

            }
            //else if (HP <= (HPOrig * .75f) && !CanSeePlayer())
            //{
            //    StartCoroutine(Heal());
            //}






        }

    }
    IEnumerator Bite()
    {
        if (!gameManager.instance.playerScript.isDead)
        {
            anim.SetTrigger("Shoot");
            isBiting = true;
            GameObject biteClone = Instantiate(bite, bitePos.position, bite.transform.rotation);
            biteClone.GetComponent<Rigidbody>().velocity = transform.forward * biteSpeed;
            bossAudioSource.PlayOneShot(bossBite[UnityEngine.Random.Range(0, bossBite.Length)], 0.6f);
            yield return new WaitForSeconds(biteRate);
            isBiting = false;
        }
    }
    IEnumerator Spit()
    {
        if (!gameManager.instance.playerScript.isDead)
        {
            isSpitting = true;
            GameObject spitClone = Instantiate(spit, spitPos.position, spit.transform.rotation);
            spitClone.GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x, transform.forward.y + .5f, transform.forward.z) * spitSpeed;
            bossAudioSource.PlayOneShot(bossSpit);
            yield return new WaitForSeconds(spitRate);
            isSpitting = false;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            gameManager.instance.bossHealthBarParent.SetActive(true);
            crabWakeColl.radius = radiusActive;
            activeRadius = crabWakeColl.radius;
            playerInRange = true;

        }
    }
    bool CanSeePlayer()
    {


        // this tells us what direction our player is in relative to our enemy
        playerDir = (new Vector3(gameManager.instance.player.transform.position.x - headPos.position.x, gameManager.instance.player.transform.position.y + 1 - headPos.position.y, gameManager.instance.player.transform.position.z - headPos.position.z));

        // this calculates the angle between where our player is and where we (the enemy) are looking
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, playerDir, Color.red);


        // this returns the info of WHAT is HIT by the raycast
        RaycastHit hit;

        // this will shoot the raycast in the direction of the player at all times. Our 'out' variable is what object is getting hit
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            // if the object we are hitting is the player, AND the angle to our player is within our sight angle
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightLine)
            {



                // this gets the enemy to move in the direction of our player
                agent.SetDestination(gameManager.instance.player.transform.position);



                FacePlayerAlways();

                if (!isSpitting && !isBiting && hit.distance <= biteDistance)
                    StartCoroutine(Bite());
                if (!isSpitting && !isBiting && hit.distance >= spitDistance)
                    StartCoroutine(Spit());

                return true;
            }
        }

        return false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            gameManager.instance.bossHealthBarParent.SetActive(false);
            //agent.stoppingDistance = 0;
        }

    }
    void BossHPUIUpdate()
    {
        // updating the Boss health bar
        gameManager.instance.bossHealthBar.fillAmount = (float)HP / (float)HPOrig;
    }
    public void TakeDamage(float dmg)
    {
        HP -= dmg;
        BossHPUIUpdate();

        // cue take damage audio
        if(agent.isActiveAndEnabled && Time.fixedTime - takeDamageTimer > 1)
        {
            bossAudioSource.PlayOneShot(bossDamage[UnityEngine.Random.Range(0, bossDamage.Length)]);
            takeDamageTimer = Time.fixedTime;
        }

        if (HP <= 0)
        {
            StopAllCoroutines();
            anim.SetBool("Dead", true);

            if(!hasPlayedEndGameAudio)
            {
                levelAudioManager.instance.voiceOverAudioSource.PlayOneShot(levelAudioManager.instance.VOKillBoss);

                if(gameManager.instance.subtitlesToggle.isOn)
                {
                    StartCoroutine(gameManager.instance.StartSubtitles(subtitleManager.instance.killBossVoiceLines));
                }
                hasPlayedEndGameAudio = true;
            }

            gameManager.instance.endGameBeam.SetActive(true);
            gameManager.instance.playerScript.hazardPay += 1500;
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

        }
        else
        {
            WaveSet();

            anim.SetTrigger("Damage");
            agent.SetDestination(gameManager.instance.player.transform.position);

            StartCoroutine(Hurt());
        }
    }
    IEnumerator Hurt()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    void FacePlayerAlways()
    {

        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, playerDir.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    //public void Flee()
    //{
    //    agent.SetDestination(new Vector3(playerDir.x, 0, playerDir.z));


    //        Heal();

    //}
    //IEnumerator Heal()
    //{
    //    if (HP != HPOrig && !isSpitting && !isBiting)
    //    {
    //        HP += healAmt;
    //        yield return new WaitForSeconds(15);

    //    }
    //}
    public void Wave1()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject crabClone = Instantiate(crab, crabSpawners[i].transform.position, transform.rotation);
        }
        HP += (HP / 5);
    }
    public void Wave2()
    {

        for (int i = 0; i < 5; i++)
        {
            //GameObject droneClone = Instantiate(drone, droneSpawners[i].transform.position, transform.rotation);
        }
            HP += (HP / 4);
            
    }
    public void Wave3()
    {

        for (int i = 0; i < 10; i++)
        {
            GameObject crabClone = Instantiate(crab, crabSpawners[i].transform.position, transform.rotation);
        }
        for (int i = 0; i < 5; i++)
        {
            //GameObject droneClone = Instantiate(drone, droneSpawners[i].transform.position, transform.rotation);
        }
        


            HP += (HP / 2);


        

        
    }
    public void WaveSet()
    {

        if (wave == 0)
        {

            if (HP <= (HPOrig * .75f) && HP >= (HPOrig / 2))
            {
                wave = 2;

                if (crab)
                {
                    Wave1();

                }

            }
        }
        else if (wave == 2)

        {


            if (HP <= (HPOrig / 2))
            {
                wave = 3;
                if (drone)
                {


                    

                    Wave2();


                }
            }
        }
        else if (wave == 3)
        {
            wave = 4;
            if (HP <= (HPOrig * .25f))
            {
                if (crab && drone)
                {


                    
                    Wave3();


                }
            }
        }


    }

    void CueFootstepAudio()
    {
        // if we are not on the ground or not moving, return
        if (!agent.isOnNavMesh) return;
        if (agent.velocity.magnitude <= 0) return;

        // reducing the time between footsteps each frame
        timeBetweenFootsteps -= Time.deltaTime;

        // once we reach 0, play audio for footsteps
        if (timeBetweenFootsteps <= 0)
        {
            bossAudioSource.PlayOneShot(bossFootsteps[UnityEngine.Random.Range(0, bossFootsteps.Length)], 0.4f);
            timeBetweenFootsteps = timeBetweenFootstepsOrig;
        }

    }
}

