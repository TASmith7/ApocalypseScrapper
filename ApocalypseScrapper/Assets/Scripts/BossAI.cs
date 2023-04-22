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
    [Range(1, 100)][SerializeField] int biteDistance;
    [SerializeField] int biteSpeed;
    [SerializeField] GameObject bite;
    [Header("-----Spit Stats-----")]
    [SerializeField] GameObject spit;
    [Range(1, 10)][SerializeField] int spitDamage;
    [Range(.1f, 5)][SerializeField] float spitRate;
    [Range(1, 100)][SerializeField] int spitDistance;
    [SerializeField] int spitSpeed;
    
    [Header("-----Drop Stats-----")]
    [SerializeField] GameObject drop;
    bool playerInRange;
    float angleToPlayer;
    float speed;
    bool isBiting;
    bool isSpitting;
    float stoppingDistanceOrig;
    bool destinationChosen;
    Vector3 startPos;
    
    
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
        HPOrig = HP;
        activeRadius = radiusSleep;
        agent.stoppingDistance = biteDistance;
        stoppingDistanceOrig = agent.stoppingDistance;
        //startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);

            if (playerInRange)
            {
                CanSeePlayer();

            }
            if (HP<(HPOrig-=(HPOrig*.75f)))
            {
                Flee();
            }






        }

    }
    IEnumerator shoot()
    {
        anim.SetTrigger("Shoot");
        isBiting = true;
        GameObject biteClone =Instantiate(bite, bitePos.position, bite.transform.rotation);
        biteClone.GetComponent<Rigidbody>().velocity = transform.forward * biteSpeed;
        yield return new WaitForSeconds(biteRate);
        isBiting = false;
    }
    IEnumerator Spit()
    {
        isSpitting = true;
        GameObject spitClone = Instantiate(spit, spitPos.position, transform.rotation);
        spitClone.GetComponent<Rigidbody>().velocity=transform.forward * spitSpeed;
        yield return new WaitForSeconds(spitRate);
        isSpitting = false;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
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



        // this returns the info of WHAT is HIT by the raycast
        RaycastHit hit;

        // this will shoot the raycast in the direction of the player at all times. Our 'out' variable is what object is getting hit
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            // if the object we are hitting is the player, AND the angle to our player is within our sight angle
            if (hit.collider.CompareTag("Player")&&angleToPlayer<=sightLine )
            {



                // this gets the enemy to move in the direction of our player
                agent.SetDestination(gameManager.instance.player.transform.position);


                FacePlayerAlways();

                if (!isSpitting&&!isBiting&&hit.distance<=biteDistance)
                    StartCoroutine(shoot());
                if(!isSpitting&&!isBiting&&hit.distance>=biteDistance)
                        StartCoroutine(Spit() );

                return true;
            }
        }

        return false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false ;
            agent.stoppingDistance = 0;
        }
    }
    public void TakeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            StopAllCoroutines();
            anim.SetBool("Dead", true);
            if (drop) 
            { 
                Instantiate(drop, transform.position, drop.transform.rotation);
            }
                
                agent.enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;

        }
        else
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(gameManager.instance.player.transform.position);
            agent.stoppingDistance = 0;
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

        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x,0,playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    public void Flee()
    {
        agent.SetDestination(-playerDir);
        if(!CanSeePlayer()&& angleToPlayer >= sightLine)
        {
            Heal();
        }
    }
    void Heal()
    {
        HP +=healAmt;
    }
}