using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// transform.position is the players(you) location


public class protodrone : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    //Layer Mask default
    public LayerMask Default;
    //Layer Mask Player
    public LayerMask Player;

    public float hp;

    //Roaming
        // walk point 
    public Vector3 wayPoint;
        // check if way point is set
    bool wayPointDes;
        // control way point range
    public float wayPointDist;

    //Attack
        // time between attacks
    public float timedAttacks;
        // to check if already attacked
    bool Attacking;
    public GameObject projectile;

    //Current
        // Sight Range
    public float inRange;
        // Attack Range
    public float attackDist;
        // Bool to see if player is in range
    public bool playerInRange;
        // Bool to see if player is withing attack range
    public bool enemyAttackRange;

    private void Awake()
    {
        // GameObject.Find() searches for in this case the player
        player = GameObject.Find("Player").transform;

        // GetComponent is used for assigning in this case the NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check to see if player inside the range sphere 
                                            //center            //Range  //Layer Mask
        playerInRange = Physics.CheckSphere(transform.position, inRange, Player);
        //Check to see if player inside the attack sphere
                                              //Center            //Range     //Layer Mask
        enemyAttackRange = Physics.CheckSphere(transform.position, attackDist, Player);

        // Not in Range sphere nor Attack sphere
        if(!playerInRange && !enemyAttackRange)
        {
            // ai roams the map
            Roaming();
        }
        // Player inside range sphere and not inside attack sphere
        if(playerInRange && !enemyAttackRange)
        {
            //ai chases player
            Chase();
        }
        // Player inside range sphere and attack sphere
        if(enemyAttackRange && playerInRange)
        {
            // ai attacks player
            Attack();
        }
    }

    private void Roaming()
    {
        //walk point not set
        if(!wayPointDes)
        {
            //search for way point for AI
            AiWayPoint();
        }

        //walk point set
        if(wayPointDes)
        {
            // Using NavMeshAgent to move the AI to the random location that was giving after checking all the safe guards
            agent.SetDestination(wayPoint);
        }

        // Calculating the Distance to the wayPoint
        Vector3 wayPointDist = transform.position- wayPoint;

        //Walkpoint reached if less that 1
        if (wayPointDist.magnitude < 1f)
        {
            // Sets back to false and automatically searches for a new point
            wayPointDes = false;
        }
    }
    private void AiWayPoint()
    {
        //Calculate random point in range
            //Returns Random Value depending how high your random range is for z
                                   //neg range     //pos range
        float zDest = Random.Range(-wayPointDist, wayPointDist);
            //Returns Random value depending how high your random range is for x
                                   //neg way range //pos way range
        float xDest = Random.Range(-wayPointDist, wayPointDist);

        //Setting walking point to setting bellow (setting for random point for ai to walk)
        wayPoint = new Vector3(transform.position.x + xDest, transform.position.y,transform.position.z + zDest);

        //Checking with Raycast to make sure AI does not fall of the map
        if(Physics.Raycast(wayPoint, -transform.up, 2f, Default))
        {
            // set wayPointDes to true if the random point is in fact on the map
            wayPointDes = true;
        }
    }
    private void Chase()
    {
        // Uses NavMesh agent to set the destination to the players position
        agent.SetDestination(player.position);
    }
    private void Attack()
    {
        //Stops the enemy from moving when attacking
        agent.SetDestination(transform.position);

        // Makes AI look at the player while attacking
        transform.LookAt(player);

        // Tracks if the AI has attacked
        if(!Attacking)
        {
            ///Attack code here
                //Used to make AI shoot projectiles
            Rigidbody rigidB = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rigidB.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rigidB.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///

            Attacking = true;
            // invokes AttackReset function and uses timedAttacks as a delay to attack
            Invoke(nameof(AttackReset), timedAttacks);
        }
    }

    private void AttackReset()
    {
        Attacking = false;
    }

    public void TakeDamage(int dmaage)
    {
        hp -= dmaage;

        if (hp <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, inRange);
    }
}
