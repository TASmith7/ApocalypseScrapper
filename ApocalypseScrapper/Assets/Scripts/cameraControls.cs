using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] public int sensHorizontal;
    [SerializeField] public int sensVertical;

    [SerializeField] GameObject player;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    [SerializeField] Camera cam;
    // public bool dynamicFOV;

    float standardFOV;
    float sprintFOV;

    float xRotation;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // setting our original FOV stats
        standardFOV = cam.fieldOfView;
        sprintFOV = standardFOV + 20;
    }
    
    void Update()
    {
        // Get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVertical;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHorizontal;

        // Convert input to rotation float 
        if (invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        if(gameManager.instance.dynamicFOVToggle.isOn)
        {
            DynamicFOV();
        }    
        
        // Clamp camera rotation
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

        // Rotate the camera on the x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate the player on the y axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    public void DynamicFOV()
    {
        // if our player is holding shift, they are grounded, and we have stamina left change the FOV
        if (Input.GetKey(KeyCode.LeftShift) && player.GetComponent<playerController>().controller.isGrounded && !player.GetComponent<playerController>().isCrouching && gameManager.instance.staminaFillBar.fillAmount > 0)
        {
            if (gameManager.instance.playerScript.isSprinting)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFOV, 10f * Time.deltaTime);
            }
        }
        else
        {
            if (!gameManager.instance.playerScript.isSprinting)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, standardFOV, 10f * Time.deltaTime);
            }
                
        }
    }
}
