using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class faceplayer : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    // allows us to cast the ray from anywhere but we choice to cast it from the head
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;

    Vector3 playerDir;
    bool playerInRange;
    float angleToPlayer;
    float stoppingDistOrig;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.updatGameGoal(1);
        // sets the stoppingDistOrig to the current stopping distance
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if (agent.isActiveAndEnabled)
        {
            // anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            //agent.SetDestination(gameManager.instance.player.transform.position);
            //// only start following and shooting if player is in range of enemy
            if (playerInRange && !CanSeePlayer())
            {
                //StartCoroutine(Roam());


            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                //StartCoroutine(Roam());
            }


        }
    }

    bool CanSeePlayer()
    {
        //player direction
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward); 
        //draws the raysfrom enemy to player
        Debug.DrawRay(headPos.position, playerDir, Color.red);
        Debug.Log(angleToPlayer);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            //check to see if ray cast hit the player and the angle of the player is less than something we set 
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                // if enemy see you he stopping distance will go back to original value
                agent.stoppingDistance = stoppingDistOrig;
                // has enemy following player
                // maybe this one will turn his head
                agent.SetDestination(gameManager.instance.player.transform.position);
                // how far he is from destination
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }

                return true;
            }
        }
        return false;
    }

    // syntax to be able to use Trigger function in the Sphere Collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    // When the player leaves the enemies range this is what the enemy will do
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            agent.stoppingDistance = 0;
        }
    }

    //public void TakeDamage(float dmg)
    //{
    //    HP -= (int)dmg;
    //    //rb.AddForce(playerDir * 5f, ForceMode.Impulse);
    //    agent.SetDestination(gameManager.instance.player.transform.position);
    //    agent.stoppingDistance = 0;


    //    StartCoroutine(FlashColor());

    //    //if (HP <= 0)
    //    //{
    //    //    StopAllCoroutines();
    //    //    // enemy will drop a GameOjbect after being killing
    //    //    Instantiate(drop, transform.position, drop.transform.rotation);
    //    //    //gameManager.instance.updatGameGoal(-1);
    //    //    //die
    //    //    anim.SetBool("Dead", true);
    //    //    GetComponent<CapsuleCollider>().enabled = false;
    //    //    agent.enabled = false;
    //    //    Destroy(gameObject);
    //    //}
    //    //else
    //    //{
    //    //    anim.SetTrigger("Damage");
    //    //    // when shot the enemy will turn towards the shooter
    //    //    agent.SetDestination(gameManager.instance.player.transform.position);
    //    //    agent.stoppingDistance = 0;
    //    //    StartCoroutine(FlashColor());
    //    //}
    //    if (HP <= 0)
    //    {
    //        StopAllCoroutines();
    //        if (drop)
    //        {
    //            Instantiate(drop, transform.position, drop.transform.rotation);
    //        }

    //        agent.enabled = false;
    //        GetComponent<CapsuleCollider>().enabled = false;

    //        //Destroy(gameObject);
    //    }
    //    else
    //    {
    //        agent.SetDestination(gameManager.instance.player.transform.position);
    //        //agent.stoppingDistance = 0;
    //        StartCoroutine(FlashColor());
    //    }
    //}

    // Function Flashes enemy red 
    //IEnumerator FlashColor()
    //{
    //    // turns enemy red
    //    model.material.color = Color.red;
    //    // waits a few seconds
    //    yield return new WaitForSeconds(0.1f);
    //    // returns enemy back to white
    //    model.material.color = Color.white;
    //}

    // fixes Bug that enemy does not turn when not moving
    void FacePlayer()
    {
        transform.LookAt(gameManager.instance.player.transform.position);
    }
}
