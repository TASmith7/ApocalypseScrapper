using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class npcAI : MonoBehaviour
{
    [Header("----- NPC Components -----")]
    [SerializeField] Transform stomachPos;
    [SerializeField] GameObject player;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform exitDestination;
    
    Animator anim;

    [Header("---- NPC States -----")]
    public bool isTrapped;
    public bool playerInRange;
    public bool rescueCoroutineStarted;
    public bool isStanding;
    public bool isExiting;
    public bool rescuedAudioStarted;
    public bool rescueLinesAreFinished;


    [Header("---- NPC Values -----")]
    [Range(1, 10)] public float timeBetweenHelpMeLines;
    [Range(1, 10)] public float playerFaceSpeed;
    [Range(0, 2)] public float timeBetweenFootsteps;
    float timeBetweenFootstepsOrig;


    private void Start()
    {
        player = gameManager.instance.player;
        anim = GetComponent<Animator>();
        isTrapped = true;
        anim.SetBool("isTrapped", isTrapped);
        timeBetweenFootstepsOrig = timeBetweenFootsteps;
    }

    private void Update()
    {
        // if we get untrapped
        if(!isTrapped)
        { 
            player.GetComponent<playerController>().questPay += 1000;

            // if we haven't already started the rescue coroutine
            if(!rescueCoroutineStarted)
            {
                anim.SetBool("isTrapped", isTrapped);
                // set rescue line bool to true and run the rescue lines
                rescueCoroutineStarted = true;
                StartCoroutine(PlayRescuedLines());
            }

            // if we are standing up and we aren't exiting yet
            if(isStanding && !isExiting)
            {
                FacePlayerAlways();
            }

            // if the audio source isn't playing anything and rescue audio had started, that means the rescue audio is finished playing
            if (!npcAudioManager.instance.npcVoiceAudioSource.isPlaying && rescuedAudioStarted && !gameManager.instance.isPaused)
            {
                rescueLinesAreFinished = true;
                isExiting = true;
            }

            // if we are exiting and our lines are finished
            if (isExiting && rescueLinesAreFinished)
            {
                // tell agent to move to exit destination
                agent.SetDestination(exitDestination.position);
                
                // set the exiting bool to true and cue footstep audio
                anim.SetBool("isExiting", isExiting);
                CueFootstepAudio();
            }

           

            // if distance between our agent and our exit is less than the threshold to exit, destroy the game object
            if(agent.transform.position.x -  exitDestination.position.x <= 0.2f  && agent.transform.position.z - exitDestination.position.z <= 0.2f)
            {
                Destroy(gameObject);
            }

        }

        Debug.DrawRay(stomachPos.position, Vector3.up, Color.red);

        // the if statement is only true when the raycast hits something
        if(Physics.Raycast(stomachPos.position, Vector3.up, 1))
        {
            isTrapped = true;
        }
        // so if the raycast doesn't hit something, then the item trapping the NPC is gone, so set isTrapped to false
        else
        {
            isTrapped = false;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        // if player enters area and the NPC is trapped
        if(other.gameObject.CompareTag("Player") && isTrapped)
        {
            // set range bool to true
            playerInRange = true;

            // cue help me voice lines
            StartCoroutine(PlayHelpMeVoiceLines());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if player exits range, set in range bool to false so help me voice lines stop playing
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator PlayHelpMeVoiceLines()
    {
        // while our player is in range and the NPC is trapped
        while (playerInRange && isTrapped)
        {
            // play random help me voice lines
            if (!npcAudioManager.instance.npcVoiceAudioSource.isPlaying)
            {
                npcAudioManager.instance.npcVoiceAudioSource.PlayOneShot(npcAudioManager.instance.npcHelpMeLines[Random.Range(0, npcAudioManager.instance.npcHelpMeLines.Length)], 
                    npcAudioManager.instance.npcVoiceAudioVolume);
            }

            // wait x amount of seconds between each voice line
            yield return new WaitForSeconds(timeBetweenHelpMeLines);
        }
    }

    IEnumerator PlayRescuedLines()
    {
        yield return new WaitForSeconds(1.5f);
        isStanding = true;

        // stop any audio that may be playing
        npcAudioManager.instance.npcVoiceAudioSource.Stop();

        // play rescue line
        if (!npcAudioManager.instance.npcVoiceAudioSource.isPlaying)
        {
            npcAudioManager.instance.npcVoiceAudioSource.PlayOneShot(npcAudioManager.instance.npcRescuedLines, npcAudioManager.instance.npcVoiceAudioVolume);

            rescuedAudioStarted = true;
        }

    }
    void FacePlayerAlways()
    {
        Vector3 playerDir = (new Vector3(player.transform.position.x - stomachPos.position.x, player.transform.position.y + 1 - stomachPos.position.y, player.transform.position.z - stomachPos.position.z));

        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
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
            npcAudioManager.instance.npcFootstepAudioSource.PlayOneShot(npcAudioManager.instance.npcFootstepAudio[Random.Range(0, npcAudioManager.instance.npcFootstepAudio.Length)], 
                npcAudioManager.instance.npcFootstepVolume);

            timeBetweenFootsteps = timeBetweenFootstepsOrig;
        }

    }
}
