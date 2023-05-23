using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class crabAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform deathGoopPos;
    [SerializeField] SphereCollider crabWakeColl;
    [SerializeField] CapsuleCollider crabCapsuleCollider;
    public GameObject drop;
    [SerializeField] playerController playerScript;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource crabAudioSource;
    [SerializeField] AudioClip[] biteAudioClip;
    [SerializeField] AudioClip[] damageAudioClip;
    [SerializeField] AudioClip[] stepAudioClip;
    [SerializeField] AudioClip dieAudioClip;

    [Header("-----Crab Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightLine;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;
    [SerializeField] float animTransSpeed;
    [Range(10, 200)][SerializeField] float radiusSleep;
    [Range(10, 1000)][SerializeField] float radiusActive;
    public float activeRadius;
    Vector3 playerDir;
    [Range(0.05f, 1)][SerializeField] float timeBetweenFootsteps;
    float timeBetweenFootstepsOrig;
    

    [Header("-----Bite Stats-----")]
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(.1f, 5)][SerializeField] float shootRate;
    [Range(1, 100)][SerializeField] int shootDistance;
    [SerializeField] int bulletSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject deathGoop;
    bool playerInRange;
    float angleToPlayer;
    float speed;
    bool isShooting;
    bool hasDroppedDeadCrab;
    float stoppingDistanceOrig;
    bool destinationChosen;
    Vector3 startPos;


    IEnumerator Roam()
    {
        if (destinationChosen != true && agent.remainingDistance < 0.05)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 runTo = UnityEngine.Random.insideUnitSphere * roamDistance;
            runTo += startPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(runTo, out hit, roamDistance, 1);

            agent.SetDestination(hit.position);
            destinationChosen = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        agent.radius = UnityEngine.Random.Range(agent.radius, agent.radius + 2f);
        agent.speed = UnityEngine.Random.Range(agent.speed, agent.speed + .5f);
        activeRadius = radiusSleep;
        agent.stoppingDistance = shootDistance;
        stoppingDistanceOrig = agent.stoppingDistance;
        startPos = transform.position;
        timeBetweenFootstepsOrig = timeBetweenFootsteps;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.isActiveAndEnabled)
        {
            if (agent.CompareTag("Minion"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
               
            }
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);

            if (playerInRange)
            {
                
                    
                CanSeePlayer();

            }
            

            CueFootstepAudio();
        }

    }
    IEnumerator shoot()
    {
        if (!playerController.isDead)
        {
            isShooting = true;
            anim.SetTrigger("Shoot");
            GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
            crabAudioSource.PlayOneShot(biteAudioClip[UnityEngine.Random.Range(0, biteAudioClip.Length)]);
            bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Player"))
            {
                crabWakeColl.radius = radiusActive;
                activeRadius = crabWakeColl.radius;
                playerInRange = true;

            }
        
    }

    bool CanSeePlayer()
    {


        // this tells us what direction our player is in relative to our enemy
        playerDir = (new Vector3(gameManager.instance.player.transform.position.x - headPos.position.x, gameManager.instance.player.transform.position.y + 1.6f - headPos.position.y, gameManager.instance.player.transform.position.z - headPos.position.z));
        Debug.DrawRay(headPos.position, playerDir, Color.red);
        // this calculates the angle between where our player is and where we (the enemy) are looking
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        // this returns the info of WHAT is HIT by the raycast
        RaycastHit hit;

        // this will shoot the raycast in the direction of the player at all times. Our 'out' variable is what object is getting hit
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            // if the object we are hitting is the player, AND the angle to our player is within our sight angle
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightLine)
            {

                if (!agent.gameObject.CompareTag("Minion"))
                {
                    agent.SetDestination(gameManager.instance.player.transform.position);
                }

                // this gets the enemy to move in the direction of our player



                FacePlayerAlways();

                if (!isShooting && hit.distance <= shootDistance)
                    StartCoroutine(shoot());

                return true;
            }
        }

        return false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // this is the line that was causing our crabs to bug out when you went inside of them
            // it's calling on trigger exit on any collider on the crab, not just the detection collider
            // honestly this line isn't even necessary, if a crab starts seeing the player,
            // they will now never not see the player, which isn't that big of a deal

            //playerInRange = false;
            
        }
    }
    public void TakeDamage(float dmg)
    {
        HP -= (int) dmg;

        if (HP <= 0 && agent.isActiveAndEnabled)
        {
            agent.enabled = false;
            StopAllCoroutines();
            anim.SetBool("Dead", true);
            // now blood drop effect
            if (deathGoop)
            {
                Instantiate(deathGoop,deathGoopPos.transform.position,transform.rotation);
            }
            if (drop && !hasDroppedDeadCrab)
            {
                hasDroppedDeadCrab = true;
                Instantiate(drop, transform.position, transform.rotation);
            }
            // cue death audio
            if (agent.isActiveAndEnabled)
            {
                crabAudioSource.PlayOneShot(dieAudioClip, 0.4f);
            }
            
            
            crabCapsuleCollider.enabled = false;
            Destroy(gameObject);
            
            

        }
        else if(HP>0 && agent.isActiveAndEnabled)
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (agent.isActiveAndEnabled)
            {
                // cue damage audio
                crabAudioSource.PlayOneShot(damageAudioClip[UnityEngine.Random.Range(0, damageAudioClip.Length)], 0.4f);
            }

            //agent.stoppingDistance = 0;
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
    public void Flee()
    {
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
            crabAudioSource.PlayOneShot(stepAudioClip[UnityEngine.Random.Range(0, stepAudioClip.Length)], 0.4f);
            timeBetweenFootsteps = timeBetweenFootstepsOrig;
        }

    }
}
