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

    [Header("----- Player Controller -----")]
    [SerializeField] CharacterController playerCont;

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
        if(Input.GetKey(KeyCode.LeftShift) && gameManager.instance.staminaFillBar.fillAmount > 0)
        {
            gunAnimator.speed = sprintBobSpeed;
        }
        // else set it back to walking speed
        else
        {
            gunAnimator.speed = walkBobSpeed;
        }

        // if we are pressing W, start bobbing gun
        if(Input.GetKey(KeyCode.W) && playerCont.isGrounded)
        {
            gunAnimator.SetTrigger("Bob");
            gunAnimator.ResetTrigger("Stop Bobbing");
        }
        // else if we are pressing S, start bobbing gun
        else if (Input.GetKey(KeyCode.S) && playerCont.isGrounded)
        {
            gunAnimator.SetTrigger("Bob");
            gunAnimator.ResetTrigger("Stop Bobbing");
        }
        // if we stop moving forwards or backwards, stop bobbing gun
        else
        {
            gunAnimator.SetTrigger("Stop Bobbing");
            gunAnimator.ResetTrigger("Bob");
        }
        
    }

}
