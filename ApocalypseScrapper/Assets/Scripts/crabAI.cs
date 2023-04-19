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
    [SerializeField] SphereCollider ratCollWake;

    [Header("-----Rat Stats-----")]
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
    
    [Header("-----Bite Stats-----")]
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(.1f, 5)][SerializeField] float shootRate;
    [Range(1, 100)][SerializeField] int shootDistance;
    [SerializeField] int bulletSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject drop;
    bool playerInRange;
    float angleToPlayer;
    float speed;
    bool isShooting;
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
        activeRadius = radiusSleep;
        agent.stoppingDistance = shootDistance;
        stoppingDistanceOrig = agent.stoppingDistance;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);
            if(playerInRange)
            {
                CanSeePlayer();
            }
            //if (playerInRange && CanSeePlayer())
            //{

            //    StartCoroutine(Roam());

            //}
            
            
           
                
              
            
        }
        
    }
    IEnumerator shoot()
    {
        anim.SetTrigger("Shoot");
        isShooting = true;
        GameObject bulletClone =Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ratCollWake.radius = radiusActive;
            activeRadius = ratCollWake.radius;
            playerInRange = true;

        }
    }
    bool CanSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - transform.position);
        angleToPlayer=Vector3.Angle(new Vector3(playerDir.x,playerDir.y+1.6f,playerDir.z),transform.forward);
        Debug.DrawRay(transform.position, playerDir);
        Debug.Log(angleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir,out hit))
        {
            if(hit.collider.CompareTag("Player")&&angleToPlayer<=sightLine)
            {
                agent.stoppingDistance = stoppingDistanceOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
               
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayerAlways();
                }
                if (!isShooting)
                {
                    if(agent.remainingDistance<=shootDistance)
                        StartCoroutine(shoot());
                }

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

        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x,playerDir.y,playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    public void Flee()
    {
    }
}
