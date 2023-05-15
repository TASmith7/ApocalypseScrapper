using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class roamDrone : MonoBehaviour, IDamage
{

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    // allows us to cast the ray from anywhere but we choice to cast it from the head
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] SphereCollider droneDetection;
    //[SerializeField] AnimationClip wakeAnimation;
    [SerializeField] playerController playerScript;
    [Header("----- Enemy Stats -----")]
    // Health Points
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDist;
    [Range(10, 200)][SerializeField] float droneSleep; //(sleep)
    [Range(10, 200)][SerializeField] float droneActive; //(active)
    public float droneRadius; //(radius)

    [Header("----- Audio -----")]
    [SerializeField] AudioSource droneAudSource;
    [SerializeField] AudioClip hoverAudio;
    [SerializeField] AudioClip shotAudio;
    [SerializeField] AudioClip damageAudio;


    //[Header("----- Enemy Gun -----")]

    //Lecture three
    [Header("----- Gun Stats -----")]
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(0.01f, 5)][SerializeField] float shootRate;
    [Range(1, 100)][SerializeField] int shootDist;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;

    [SerializeField] GameObject drop;

    //direction of the player is in
    Vector3 playerDir;
    bool playerInRange;
    float angleToPlayer;
    bool isShooting;
    float stoppingDistOrig;
    bool destinationChosen;
    Vector3 startingPos;
    bool droneAudioWasPlaying;

    //Animation 
    //private bool canShoot = false;
    //private float wakeAnimationLength;


    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.updatGameGoal(1);
        // sets the stoppingDistOrig to the current stopping distance

        droneRadius = droneSleep;
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;

        //getting the length of the wake animation clip
        //wakeAnimationLength = wakeAnimation.length;

    }

    // Update is called once per frame
    void Update()
    {

        if (agent.isActiveAndEnabled)
        {
            //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetCurrentAnimatorStateInfo(0).IsName(wakeAnimation.name))
            //{
            //    canShoot = true;
            //}
            if (playerInRange && CanSeePlayer())
            {
                anim.SetBool("playerInRange", true);
            }
            // anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            //agent.SetDestination(gameManager.instance.player.transform.position);
            // only start following and shooting if player is in range of enemy

            //Checking if the wake clip is finished

            if (playerInRange && !CanSeePlayer())
            {
                StartCoroutine(Roam());
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(Roam());
            }

            

            // if we pause, stop all drone audio
            if (droneAudSource.isPlaying && gameManager.instance.isPaused)
            {
                droneAudioWasPlaying = true;
                droneAudSource.Pause();
            }
            if (droneAudioWasPlaying && !gameManager.instance.isPaused)
            {
                droneAudioWasPlaying = false;
                droneAudSource.UnPause();
            }

            // if audio source isn't playing and we are not paused, play hover audio
            if (!droneAudSource.isPlaying && !gameManager.instance.isPaused)
            {
                droneAudSource.PlayOneShot(hoverAudio);
            }


        }
    }

    // (Roaming)
    IEnumerator Roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;

            yield return new WaitForSeconds(roamPauseTime);

            // casting a sphere choosing a random spot inside the sphere an that is where ai will walk to
            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);

            agent.SetDestination(hit.position);
            destinationChosen = false;
        }
    }

    bool CanSeePlayer()
    {
        //player direction
        playerDir = (new Vector3(gameManager.instance.player.transform.position.x - headPos.position.x, gameManager.instance.player.transform.position.y + 1 - headPos.position.y, gameManager.instance.player.transform.position.z - headPos.position.z));
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward); // but i had change transform.forward to headPos.forward and enemy was only able to see me on his left side and my right
        //draws the raysfrom enemy to player
        Debug.DrawRay(headPos.position, playerDir, Color.red);
        // Debug.Log(angleToPlayer);

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

                if (!isShooting)
                {
                    //if (canShoot)
                    //{
                        StartCoroutine(Shoot());
                    //}
                }

                return true;
            }
        }
        return false;
    }

    IEnumerator Shoot()
    {
        if (!playerController.isDead)
        {
            isShooting = true;
            //anim.SetTrigger("Shoot");
            //Debug.Log("AI shot!");
            // to reference a bullet
            GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
            // to give bullet a velocity
            // this transform would need to be the camera (Camera.main.transform.forward) for player to shoot bullets
            bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            droneAudSource.PlayOneShot(shotAudio);
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    // syntax to be able to use Trigger function in the Sphere Collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            droneDetection.radius = droneActive;
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

    public void TakeDamage(float dmg)
    {
        HP -= (int)dmg;
        //rb.AddForce(playerDir * 5f, ForceMode.Impulse);

        droneAudSource.PlayOneShot(damageAudio, 0.75f);

        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }

        agent.stoppingDistance = 0;

        StartCoroutine(FlashColor());

        //if (HP <= 0)
        //{
        //    StopAllCoroutines();
        //    // enemy will drop a GameOjbect after being killing
        //    Instantiate(drop, transform.position, drop.transform.rotation);
        //    //gameManager.instance.updatGameGoal(-1);
        //    //die
        //    anim.SetBool("Dead", true);
        //    GetComponent<CapsuleCollider>().enabled = false;
        //    agent.enabled = false;
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    anim.SetTrigger("Damage");
        //    // when shot the enemy will turn towards the shooter
        //    agent.SetDestination(gameManager.instance.player.transform.position);
        //    agent.stoppingDistance = 0;
        //    StartCoroutine(FlashColor());
        //}
        if (HP <= 0)
        {
            StopAllCoroutines();
            //anim.SetBool("Dead", true);
            if (drop)
            {
                Instantiate(drop, transform.position, drop.transform.rotation);
            }

            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            Destroy(gameObject);
        }
        else
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(gameManager.instance.player.transform.position);
            //agent.stoppingDistance = 0;
            StartCoroutine(FlashColor());
        }
    }

    // Function Flashes enemy red 
    IEnumerator FlashColor()
    {
        // turns enemy red
        model.material.color = Color.red;
        // waits a few seconds
        yield return new WaitForSeconds(0.1f);
        // returns enemy back to white
        model.material.color = Color.white;
    }

    // fixes Bug that enemy does not turn when not moving
    void FacePlayer()
    {

        transform.LookAt(gameManager.instance.player.transform.position);

    }
}

//public class roamDrone : MonoBehaviour
//{
//    [Header("----- Components -----")]
//    [SerializeField] Renderer model;
//    [SerializeField] NavMeshAgent agent;
//    [SerializeField] Animator anim;
//    // allows us to cast the ray from anywhere but we choice to cast it from the head
//    [SerializeField] Transform headPos;
//    [SerializeField] Transform shootPos;
//    [SerializeField] SphereCollider droneDetection;

//    [Header("----- Enemy Stats -----")]
//    // Health Points
//    [SerializeField] int HP;
//    [SerializeField] int playerFaceSpeed;
//    [SerializeField] int sightAngle;
//    [SerializeField] int roamPauseTime;
//    [SerializeField] int roamDist;
//    [Range(10, 200)][SerializeField] float droneSleep; //(sleep)
//    [Range(10, 200)][SerializeField] float droneActive; //(active)
//    public float droneRadius; //(radius)

//    //[Header("----- Enemy Gun -----")]

//    //Lecture three
//    [Header("----- Gun Stats -----")]
//    [Range(1, 10)][SerializeField] int shootDamage;
//    [Range(0.01f, 5)][SerializeField] float shootRate;
//    [Range(1, 100)][SerializeField] int shootDist;
//    [SerializeField] GameObject bullet;
//    [SerializeField] int bulletSpeed;

//    [SerializeField] GameObject drop;

//    //direction of the player is in
//    Vector3 playerDir;
//    bool playerInRange;
//    float angleToPlayer;
//    bool isShooting;
//    float stoppingDistOrig;
//    bool destinationChosen;
//    Vector3 startingPos;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //gameManager.instance.updatGameGoal(1);
//        // sets the stoppingDistOrig to the current stopping distance

//        droneRadius = droneSleep;
//        stoppingDistOrig = agent.stoppingDistance;
//        startingPos = transform.position;

//    }

//    // Update is called once per frame
//    void Update()
//    {

//        if (agent.isActiveAndEnabled)
//        {
//            if (playerInRange && CanSeePlayer())
//            {
//                anim.SetBool("playerInRange", true);
//            }
//            // anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
//            //agent.SetDestination(gameManager.instance.player.transform.position);
//            // only start following and shooting if player is in range of enemy
//            if (playerInRange && !CanSeePlayer())
//            {
//                StartCoroutine(Roam());


//            }
//            else if (agent.destination != gameManager.instance.player.transform.position)
//            {
//                StartCoroutine(Roam());
//            }


//        }
//    }

//    // (Roaming)
//    IEnumerator Roam()
//    {
//        if (!destinationChosen && agent.remainingDistance < 0.05f)
//        {
//            destinationChosen = true;
//            agent.stoppingDistance = 0;

//            yield return new WaitForSeconds(roamPauseTime);

//            // casting a sphere choosing a random spot inside the sphere an that is where ai will walk to
//            Vector3 ranPos = Random.insideUnitSphere * roamDist;
//            ranPos += startingPos;

//            NavMeshHit hit;
//            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);

//            agent.SetDestination(hit.position);
//            destinationChosen = false;
//        }
//    }

//    bool CanSeePlayer()
//    {
//        //player direction
//        playerDir = (new Vector3(gameManager.instance.player.transform.position.x - headPos.position.x, gameManager.instance.player.transform.position.y + 1 - headPos.position.y, gameManager.instance.player.transform.position.z - headPos.position.z));
//        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward); // but i had change transform.forward to headPos.forward and enemy was only able to see me on his left side and my right
//        //draws the raysfrom enemy to player
//        Debug.DrawRay(headPos.position, playerDir, Color.red);
//        Debug.Log(angleToPlayer);

//        RaycastHit hit;
//        if (Physics.Raycast(headPos.position, playerDir, out hit))
//        {
//            //check to see if ray cast hit the player and the angle of the player is less than something we set 
//            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
//            {
//                // if enemy see you he stopping distance will go back to original value
//                agent.stoppingDistance = stoppingDistOrig;
//                // has enemy following player
//                // maybe this one will turn his head
//                agent.SetDestination(gameManager.instance.player.transform.position);
//                // how far he is from destination
//                if (agent.remainingDistance <= agent.stoppingDistance)
//                {
//                    FacePlayer();
//                }

//                if (!isShooting)
//                {
//                    StartCoroutine(Shoot());
//                }

//                return true;
//            }
//        }
//        return false;
//    }

//    IEnumerator Shoot()
//    {
//        isShooting = true;
//        //anim.SetTrigger("Shoot");
//        // to reference a bullet
//        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
//        // to give bullet a velocity                     this transform would need to be the camera (Camera.main.transform.forward) for player to shoot bullets
//        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
//        yield return new WaitForSeconds(shootRate);
//        isShooting = false;
//    }

//    // syntax to be able to use Trigger function in the Sphere Collider
//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            droneDetection.radius = droneActive;
//            playerInRange = true;
//        }
//    }
//    // When the player leaves the enemies range this is what the enemy will do
//    void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            playerInRange = false;

//            agent.stoppingDistance = 0;
//        }
//    }

//    public void TakeDamage(float dmg)
//    {
//        HP -= (int)dmg;
//        //rb.AddForce(playerDir * 5f, ForceMode.Impulse);

//        if (agent.isActiveAndEnabled)
//        {
//            agent.SetDestination(gameManager.instance.player.transform.position);
//        }

//        agent.stoppingDistance = 0;

//        StartCoroutine(FlashColor());

//        //if (HP <= 0)
//        //{
//        //    StopAllCoroutines();
//        //    // enemy will drop a GameOjbect after being killing
//        //    Instantiate(drop, transform.position, drop.transform.rotation);
//        //    //gameManager.instance.updatGameGoal(-1);
//        //    //die
//        //    anim.SetBool("Dead", true);
//        //    GetComponent<CapsuleCollider>().enabled = false;
//        //    agent.enabled = false;
//        //    Destroy(gameObject);
//        //}
//        //else
//        //{
//        //    anim.SetTrigger("Damage");
//        //    // when shot the enemy will turn towards the shooter
//        //    agent.SetDestination(gameManager.instance.player.transform.position);
//        //    agent.stoppingDistance = 0;
//        //    StartCoroutine(FlashColor());
//        //}
//        if (HP <= 0)
//        {
//            StopAllCoroutines();
//            anim.SetBool("Dead", true);
//            if (drop)
//            {
//                Instantiate(drop, transform.position, drop.transform.rotation);
//            }

//            agent.enabled = false;
//            GetComponent<CapsuleCollider>().enabled = false;

//            //Destroy(gameObject);
//        }
//        else
//        {
//            anim.SetTrigger("Damage");
//            agent.SetDestination(gameManager.instance.player.transform.position);
//            //agent.stoppingDistance = 0;
//            StartCoroutine(FlashColor());
//        }
//    }

//    // Function Flashes enemy red 
//    IEnumerator FlashColor()
//    {
//        // turns enemy red
//        model.material.color = Color.red;
//        // waits a few seconds
//        yield return new WaitForSeconds(0.1f);
//        // returns enemy back to white
//        model.material.color = Color.white;
//    }

//    // fixes Bug that enemy does not turn when not moving
//    void FacePlayer()
//    {

//        transform.LookAt(gameManager.instance.player.transform.position);

//    }
//}
