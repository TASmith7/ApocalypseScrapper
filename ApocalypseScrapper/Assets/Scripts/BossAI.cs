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
    [Range(1, 100)] public float biteDistance;
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
    int wave;
    float speed;
    bool isBiting;
    bool isSpitting;
    bool hasHealed;
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
        hasHealed = false;
        wave = 0;
        HPOrig = HP;
        activeRadius = radiusSleep;
        biteDistance = agent.stoppingDistance;
        BossHPUIUpdate();
        //stoppingDistanceOrig = agent.stoppingDistance;
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
            //else if (HP <= (HPOrig * .75f) && !CanSeePlayer())
            //{
            //    StartCoroutine(Heal());
            //}






        }

    }
    IEnumerator Shoot()
    {
        anim.SetTrigger("Shoot");
        isBiting = true;
        GameObject biteClone = Instantiate(bite, bitePos.position, bite.transform.rotation);
        biteClone.GetComponent<Rigidbody>().velocity = transform.forward * biteSpeed;
        yield return new WaitForSeconds(biteRate);
        isBiting = false;
    }
    IEnumerator Spit()
    {
        isSpitting = true;
        GameObject spitClone = Instantiate(spit, spitPos.position, spit.transform.rotation);
        spitClone.GetComponent<Rigidbody>().velocity = transform.forward * spitSpeed;
        yield return new WaitForSeconds(spitRate);
        isSpitting = false;

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
        playerDir = (new Vector3(gameManager.instance.player.transform.position.x - headPos.position.x, gameManager.instance.player.transform.position.y+1 - headPos.position.y, gameManager.instance.player.transform.position.z - headPos.position.z));

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
                    StartCoroutine(Shoot());
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
        //if (crab)
        //{W
        //    for (int i = 0; i < 5; i++)
        //    {
        //    }

        //}
        int randNum=UnityEngine.Random.Range(5, 15);
        playerDir = (new Vector3(gameManager.instance.player.transform.position.x+randNum, gameManager.instance.player.transform.position.y, gameManager.instance.player.transform.position.z+randNum));
        GameObject crabClone = Instantiate(crab, new Vector3(playerDir.x,transform.position.y,playerDir.z), transform.rotation);
        if (!hasHealed)
        {


            HP += (HP / 4);


        }

        hasHealed = true;
    }
    public void Wave2()
    {        

        playerDir = (new Vector3(gameManager.instance.player.transform.position.x, gameManager.instance.player.transform.position.y, gameManager.instance.player.transform.position.z));
        GameObject droneClone = Instantiate(drone, new Vector3(playerDir.x, transform.position.y, playerDir.z), transform.rotation);
        if (!hasHealed)
        {


            HP += (HP / 4);


        }
        hasHealed = true;
    }
    public void Wave3()
    {

        playerDir = (new Vector3(gameManager.instance.player.transform.position.x, gameManager.instance.player.transform.position.y, gameManager.instance.player.transform.position.z));
        GameObject crabClone = Instantiate(crab, new Vector3(playerDir.x, transform.position.y, playerDir.z), transform.rotation); 

        playerDir = (new Vector3(gameManager.instance.player.transform.position.x, gameManager.instance.player.transform.position.y, gameManager.instance.player.transform.position.z));
        GameObject droneClone = Instantiate(drone, new Vector3(playerDir.x, transform.position.y, playerDir.z), transform.rotation);
        if (!hasHealed)
        {


            HP += (HP / 4);


        }

        hasHealed= true;
    }
    public void WaveSet()
    {
        
        if (wave != 1 && wave != 3)
        {

            if (HP <= (HPOrig * .75f) && HP >= (HPOrig / 2))
            {
                wave = 1;

                if (crab)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        
                        Wave1();
                        
                    }

                }

            }
        }
        else if (wave != 2)

        {


            if (HP <= (HPOrig / 2))
            {
                wave = 2;
                if (drone)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        hasHealed = false;
                        
                        Wave2();
                    }

                }
            }
        }
        else if (wave != 3)
        {
            wave = 3;
            if (HP <= (HPOrig * .25f))
            {
                if (crab && drone)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        hasHealed = false;
                        Wave3();
                    }

                }
            }
        }
        

    }
}
    
