using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponMovement : MonoBehaviour
{
    [Header("----- Sway Settings -----")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    [Header("----- Animation Settings -----")]
    public Animator gunAnimator;
    public float sprintBobSpeed;
    public float walkBobSpeed;
    public float crouchBobSpeed;

    [Header("----- Player Controller -----")]
    [SerializeField] CharacterController playerCont;
    [SerializeField] GameObject player;
    [SerializeField] playerController playerScript;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCont = player.GetComponent<CharacterController>();
        playerScript = player.GetComponent<playerController>();
    }

    private void Update()
    {
        // getting mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

        // if we are sprinting, speed up the gun bob animation
        if (Input.GetKey(KeyCode.LeftShift) && gameManager.instance.staminaFillBar.fillAmount > 0 && !playerScript.isCrouching)
        {
            gunAnimator.speed = sprintBobSpeed;
        }
        else if(playerScript.isCrouching)
        {
            gunAnimator.speed = crouchBobSpeed;
        }
        // else set it back to walking speed
        else
        {
            gunAnimator.speed = walkBobSpeed;
        }

        // if we are pressing W, start bobbing gun
        if(Input.GetKey(KeyCode.W) && playerCont.isGrounded && !playerScript.isMeleeing)
        {
            gunAnimator.SetTrigger("Bob");
            gunAnimator.ResetTrigger("Stop Bobbing");
        }
        // else if we are pressing S, start bobbing gun
        else if (Input.GetKey(KeyCode.S) && playerCont.isGrounded && !playerScript.isMeleeing)
        {
            gunAnimator.SetTrigger("Bob");
            gunAnimator.ResetTrigger("Stop Bobbing");
        }
        // if we stop moving forwards or backwards, stop bobbing gun
        else if (!playerScript.isMeleeing)
        {
            gunAnimator.SetTrigger("Stop Bobbing");
            gunAnimator.ResetTrigger("Bob");
        }
        
    }

}
